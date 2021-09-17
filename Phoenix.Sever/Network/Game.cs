using Phoenix.Common.Commands.Factory;
using Phoenix.Common.Commands.Request;
using Phoenix.Common.Commands.Response;
using Phoenix.Common.Commands.Updates;
using Phoenix.Common.Data;
using Phoenix.Common.Data.Types;
using Phoenix.Server.Connections;
using Phoenix.Server.Data;
using Phoenix.Server.Logs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;

namespace Phoenix.Server.Network
{
	public class Game
	{

        #region -- Game Initialization --

		/// <summary>
		/// Declaration of the Server.
		/// </summary>
        private Server server;

		/// <summary>
		/// Connected clients that are NOT authenticated yet.
		/// </summary>
		private readonly Dictionary<string, ConnectedClient> connectedClients = new();

		/// <summary>
		/// Connected clients that ARE authenticated.
		/// </summary>
		private readonly List<ConnectedAccount> connectedAccounts = new();

		/// <summary>
		/// Queue Command Library.
		/// </summary>
		private readonly SortedDictionary<double, List<ClientCommand>> actionQueue = new();

		/// <summary>
		/// Boolean for Game Thread.
		/// </summary>
		private readonly bool stopGameWorkerThread = false;

		/// <summary>
		/// Declaration of Game Thread that controls Game Logic.
		/// </summary>
		private Thread gameWorkerThread;

		/// <summary>
		/// Declaration of Concurrent Queue that handles queue of commands.
		/// </summary>
		private readonly ConcurrentQueue<ClientCommand> queuedCommand = new();

		/// <summary>
		/// Declaration of Loaded Rooms.
		/// </summary>
		private List<Room> rooms = new();
		
		/// <summary>
		/// Declaration of Connected Characters.
		/// </summary>
		private readonly List<Character> connectedCharacters = new();

		/// <summary>
		/// Declaration of Total Connections This Reboot.
		/// </summary>
		private int totalConnections = 0;

		/// <summary>
		/// Declaration of Maximum Players.
		/// </summary>
		private int maximumPlayers = 0;

		/// <summary>
		/// Start Server.
		/// </summary>
		public void Start()
		{
			this.gameWorkerThread = new Thread(new ThreadStart(GameWorkerThread));
			this.gameWorkerThread.Start();
		}

        #endregion

        #region -- Events --

		/// <summary>
		/// Handles Client Connection.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void Server_OnClientConnected(object sender, ConnectedClient e)
		{
			//store the connected client
			this.connectedClients.Add(e.Id, e);

			// Log Command
			Logger.ConsoleLog("Connection", $"{e.Id} has connected.");
		}

		/// <summary>
		/// Handles Incoming Client Commands.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Server_OnClientMessage(object sender, ConnectedClientMessage e)
		{
			var command = CommandFactory.ParseCommand(e.Message);
			double currentTimeStamp = DateTimeOffset.Now.ToUnixTimeSeconds();
			AddToQueue(false, currentTimeStamp, command, e.ConnectedClient.Id);
		}

		/// <summary>
		/// Handles Client Disconnection.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Server_OnClientDisconnected(object sender, ConnectedClient e)
		{
			//Remove from EITHER connected clients OR connected accounts (it could be in either)
			var connectedAccount = this.connectedAccounts.FirstOrDefault(c => c.Client.Id == e.Id);
			if (this.connectedClients.ContainsKey(e.Id))
				this.connectedClients.Remove(e.Id);
	
			//coding it this way to be safe, just in case we have a different instance of the same connection
			//I DOUBT we do though
			if (connectedAccount != null)
            {
				foreach (Room room in this.rooms)
				{
					if(connectedAccount.Account.Character != null)
						if (connectedAccount.Account.Character.RoomID == room.ID)
						{
							room.RoomCharacters.Remove(connectedAccount.Account.Character);
							foreach (Character character in room.RoomCharacters)
							{
								foreach (ConnectedAccount cAccount in connectedAccounts)
								{
									if (cAccount.Account.Character.Id == character.Id)
									{
										var roomPlayerUpdateCommand = new RoomPlayerUpdateCommand
										{
											Mode = 2,
											Character = connectedAccount.Account.Character
										};
										SendCommandToClient(cAccount.Client, roomPlayerUpdateCommand);
									}
								}
							}
						}
						this.connectedCharacters.Remove(connectedAccount.Account.Character);
						this.connectedAccounts.Remove(connectedAccount);
				}
			}

			// Log Command
			Logger.ConsoleLog("Connection", $"{e.Id} has disconnected.");
		}

		/// <summary>
		/// Handled Server Boot.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Server_OnServerStarted(object sender, EventArgs e)
		{
			// Log Command
			Logger.ConsoleLog("System", "Server Started.");
		}

		/// <summary>
		/// Handles Server Shutdown.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Server_OnServerStopped(object sender, EventArgs e)
		{
			// Log Command
			Logger.ConsoleLog("System", "Server Shutdown.");
		}

        #endregion

        #region -- Send Commands --

        private static void SendCommandToClient(ConnectedClient client, Command command)
		{
			var message = CommandFactory.FormatCommand(command);
			client.Send(message);
			Logger.ConsoleLog("Command", $"{command.CommandType} sent to {client.Id}");
		}

        #endregion

        #region -- Game Loop --

        /// <summary>
        /// Game Loop Thread.
        /// </summary>
        private void GameWorkerThread()
		{

            #region --  Initialize Thread & Turn Server On --

            Database.InitializeDatabse();
			Logger.ConsoleLog("System", "Loading Rooms.");
			this.rooms = Database.LoadRooms(Constants.GAME_MODE);

			foreach (Room room in rooms)
            {
				Logger.ConsoleLog("Debug", $"Room Name: {room.Name}");
				foreach (Entity entity in room.Entities)
                {
					Logger.ConsoleLog("Debug", $"Entity Name: {entity.Name}");
                }
            }

			this.server = new Server();

			this.server.OnClientConnected += Server_OnClientConnected;
			this.server.OnClientDisconnected += Server_OnClientDisconnected;
			this.server.OnClientMessage += Server_OnClientMessage;
			this.server.OnServerStarted += Server_OnServerStarted;
			this.server.OnServerStopped += Server_OnServerStopped;

			this.server.Start(IPAddress.IPv6Any, Constants.LIVE_PORT);

            #endregion

            while (!this.stopGameWorkerThread)
			{

				#region -- Queue Handling --

				var currentTimeStamp = DateTimeOffset.Now.ToUnixTimeSeconds();
				AddToQueue(true, currentTimeStamp);
				actionQueue.Remove(currentTimeStamp);

				if (!this.queuedCommand.TryDequeue(out ClientCommand cmd))
				{
					Thread.Sleep(10);
					continue;
				}

				var clientId = cmd.Id;
				var command = cmd.Command;
				var clientWhoSendCommand = GetClientById(clientId);
				var accountConnected = GetConnectedAccount(clientId);

                #endregion

                switch (command.CommandType)
				{

                    #region -- Auth Command --

                    case CommandType.Authenticate:
						{
							Logger.ConsoleLog("Command", $"{command.CommandType} from {clientId}.");

							var authCommand = command as AuthenticateCommand;
							var authenticated = false;
							int id = -1;
							int gold = -1;

							if (Database.GetAccountField(Constants.GAME_MODE, "Password", "Name", authCommand.Username) == authCommand.Password)
							{
								authenticated = true;
								id = Int32.Parse(Database.GetAccountField(Constants.GAME_MODE, "ID", "Name", authCommand.Username));
								gold = Int32.Parse(Database.GetAccountField(Constants.GAME_MODE, "Gold", "Name", authCommand.Username));
							}

							var authResponseCmd = new AuthenticateResponseCommand
							{
								Success = authenticated
							};

							SendCommandToClient(clientWhoSendCommand, authResponseCmd);

							if (authenticated)
							{
								var connectedAccount = new ConnectedAccount
								{
									Client = clientWhoSendCommand,
									Account = new Account
									{
										Id = id,
										Gold = gold
									}
								};
								this.connectedAccounts.Add(connectedAccount);
								this.connectedClients.Remove(clientId);
							}

							break;
						}
					#endregion

                    #region -- NewAccount Command --

                    case CommandType.NewAccount:
						{
							Logger.ConsoleLog("Command", $"{command.CommandType} from {clientId}.");
							var newAccountCommand = command as NewAccountCommand;
							var newAccountResponseCommand = new NewAccountResponseCommand();

							if (Database.GetAccountField(Constants.GAME_MODE, "Name", "Name", newAccountCommand.Username) == null)
							{
								Database.InsertNewAccount(Constants.GAME_MODE, newAccountCommand.Username, newAccountCommand.Password, newAccountCommand.Email);
								Logger.ConsoleLog("System", $"{clientId} has created a new account named: {newAccountCommand.Username}.");
								newAccountResponseCommand.Success = true;
								var connectedAccount = new ConnectedAccount
								{
									Client = clientWhoSendCommand,
									Account = new Account
									{
										Id = Int32.Parse(Database.GetAccountField(Constants.GAME_MODE, "ID", "Name", newAccountCommand.Username)),
										Gold = Int32.Parse(Database.GetAccountField(Constants.GAME_MODE, "Gold", "Name", newAccountCommand.Username))
									}
								};
								this.connectedAccounts.Add(connectedAccount);
								this.connectedClients.Remove(clientId);
							}
							else
							{
								Logger.ConsoleLog("System", $"{clientId} has failed to create a new account named: {newAccountCommand.Username}. Reason: Account Exists.");
								newAccountResponseCommand.Success = false;
							}
							SendCommandToClient(clientWhoSendCommand, newAccountResponseCommand);
							break;
						}
					#endregion

					#region -- NewCharacter Command --

					case CommandType.NewCharacter:
						{
							Logger.ConsoleLog("Command", $"{command.CommandType} from {clientId}.");
							var newCharacterCommand = command as NewCharacterCommand;

							var newCharacterResponseCmd = new NewCharacterResponseCommand();

							if (Database.GetCharacterField(Constants.GAME_MODE, "Name", "Name", newCharacterCommand.CharacterName) == null)
							{
								Database.InsertNewCharacter(Constants.GAME_MODE, newCharacterCommand.CharacterName, newCharacterCommand.Gender, newCharacterCommand.Philosophy, newCharacterCommand.Image, accountConnected.Account.Id);
								Logger.ConsoleLog("System", $"{clientId} has created a new character named: {newCharacterCommand.CharacterName}");
								newCharacterResponseCmd.Success = true;
							}
							else
							{
								Logger.ConsoleLog("System", $"{clientId} has failed to create new character named: {newCharacterCommand.CharacterName}. Reason: Character Exists.");
								newCharacterResponseCmd.Success = false;
							}
							SendCommandToClient(clientWhoSendCommand, newCharacterResponseCmd);
							break;
						}
					#endregion

					#region -- GetCharacterList Command --

					case CommandType.CharacterList:
						{
							Logger.ConsoleLog("Command", $"{command.CommandType} from {clientId}.");
							var newCharacterList = command as GetCharacterListCommand;

							List<Character> characters = Database.GetCharacterList(Constants.GAME_MODE, accountConnected.Account.Id);

							var newCharacterListResponseCmd = new CharacterListResponseCommand
							{
								Success = characters.Count > 0,
								Characters = characters
							};
							SendCommandToClient(clientWhoSendCommand, newCharacterListResponseCmd);

							break;
						}
					#endregion

					#region -- CharacterConnectCommand --

					case CommandType.CharacterLogin:
						{
							Logger.ConsoleLog("Command", $"{command.CommandType} from {clientId}.");
							var characterConnectionCommand = command as CharacterConnectCommand;

							Character loginCharacter = Database.GetCharacter(Constants.GAME_MODE, accountConnected.Account.Id, characterConnectionCommand.Name);

							var newCharacterConnectResponseCommand = new CharacterConnectResponseCommand
							{
								Success = loginCharacter != null,
								Character = loginCharacter
							};
							SendCommandToClient(clientWhoSendCommand, newCharacterConnectResponseCommand);
							if (loginCharacter != null)
                            {
								accountConnected.Account.Character = loginCharacter;
								this.connectedCharacters.Add(loginCharacter);

								foreach (Room room in this.rooms)
                                {
									if (loginCharacter.RoomID == room.ID)
                                    {
										foreach(Character character in room.RoomCharacters)
                                        {
											foreach (ConnectedAccount cAccount in connectedAccounts)
                                            {
												if(cAccount.Account.Character.Id == character.Id)
                                                {
													var roomPlayerUpdateCommand = new RoomPlayerUpdateCommand
													{
														Mode = 1,
														Character = loginCharacter		
													};
													SendCommandToClient(cAccount.Client, roomPlayerUpdateCommand);
                                                }
                                            }
                                        }
										room.RoomCharacters.Add(loginCharacter);
										break;
									}
                                }

								totalConnections++;
								if (maximumPlayers < connectedCharacters.Count) maximumPlayers++;
							}
							break;
						}
					#endregion

					#region -- ClientConnect Command --
					case CommandType.ClientConnect:
						{
							Logger.ConsoleLog("Command", $"{command.CommandType} from {clientId}.");
							var clientConnectCommand = command as ClientConnectCommand;

                            var clientConnectResponseCommand = new ClientConnectResponseCommand
                            {
                                Success = false,
                                Message = ""
                            };

                            foreach (Character character in connectedCharacters)
                            {
								if (character.Id == clientConnectCommand.Id)
                                {

									clientConnectResponseCommand.Success = true;
									clientConnectResponseCommand.Message = $"&tilda&gWelcome to &tilda&w{Constants.GAME_NAME}&tilda&g, &tilda&w{character.Name}&tilda&g! There are &tilda&w{connectedCharacters.Count}&tilda&g players online. We have had &tilda&w{totalConnections}&tilda&g total connections and a maximum of &tilda&w{maximumPlayers}&tilda&g players online this reboot. The current time is &tilda&w{DateTime.Now.ToShortTimeString()}&tilda&g.\n";

									SendCommandToClient(clientWhoSendCommand, clientConnectResponseCommand);
									break;
                                }
                            }
							if (!clientConnectResponseCommand.Success)
                            {
								SendCommandToClient(clientWhoSendCommand, clientConnectResponseCommand);
							}
							break;
						}
					#endregion

					#region -- ClientRoom Command --
					case CommandType.ClientRoom:
						{
							Logger.ConsoleLog("Command", $"{command.CommandType} from {clientId}.");
							var clientRoomCommand = command as ClientRoomCommand;
							var clientRoomResponseCommand = new ClientRoomResponseCommand();

							foreach (Room room in rooms)
                            {
								if (room.ID == clientRoomCommand.RoomID)
                                {
									clientRoomResponseCommand.Success = true;
									clientRoomResponseCommand.Room.Name = room.Name;
									clientRoomResponseCommand.Room.Description = room.Description;
									clientRoomResponseCommand.Room.Exits = room.Exits;
									clientRoomResponseCommand.Room.Type = room.Type;
									
									foreach (Character character in room.RoomCharacters) 
									{
										Character newCharacter = new();
										newCharacter.Name = character.Name;
										newCharacter.Image = character.Image;
										newCharacter.Type = character.Type;
										clientRoomResponseCommand.Room.RoomCharacters.Add(newCharacter);
									}

									foreach (Entity entity in room.RoomEntities)
									{
										Entity newEntity = new();
										newEntity.Name = entity.Name;
										newEntity.Image = entity.Image;
										newEntity.Type = entity.Type;
										clientRoomResponseCommand.Room.RoomEntities.Add(newEntity);
									}

									foreach (Item item in room.RoomItems)
                                    {
										Item newItem = new();
										newItem.Name = item.Name;
										newItem.Image = item.Image;
										newItem.Type = item.Type;
										newItem.Amount = item.Amount;
                                    }

									List<Room> roomArray = FindRooms(rooms[clientRoomCommand.RoomID]);

									var roomMapResponseCommand = new RoomMapResponseCommand
									{
										Success = true,
										RoomsHigh = 5,
										RoomsWide = 5,
										Rooms = roomArray
									};
									room.TimeStamp = DateTimeOffset.Now.ToUnixTimeSeconds() + 300;
									SendCommandToClient(clientWhoSendCommand, clientRoomResponseCommand);
									SendCommandToClient(clientWhoSendCommand, roomMapResponseCommand);
									break;
								}
							}
							if (!clientRoomResponseCommand.Success)
							{
								clientRoomResponseCommand.Success = false;
								clientRoomResponseCommand.Room = new Room();
								SendCommandToClient(clientWhoSendCommand, clientRoomResponseCommand);
							}
							break;
						}
                    #endregion

                    #region -- MessageRoom Command --
                    case CommandType.MessageRoom:
						{
							Logger.ConsoleLog("Command", $"{command.CommandType} from {clientId}.");
							var messageRoomCommand = command as MessageRoomCommand;

							foreach (ConnectedAccount connectedAccount in connectedAccounts)
                            {
								if (connectedAccount.Account.Character != null)
                                {
									if (connectedAccount.Account.Character.RoomID == accountConnected.Account.Character.RoomID)
									{
										var nMessageRoomCommand = new MessageRoomCommand
										{
											Character = accountConnected.Account.Character,
											Message = messageRoomCommand.Message
										};
										SendCommandToClient(connectedAccount.Client, nMessageRoomCommand);
									}
								}
                            }
							break;
						}
					#endregion

					#region -- Unknown Command --

					case CommandType.Unknown:
					default:
						{
							Logger.ConsoleLog("Command", $"Unknown command from {clientId}.");
							break;
						}
                    #endregion

                }

                Thread.Sleep(10);
			}
		}

        #endregion

        #region -- Tools --

        /// <summary>
        /// Returns a Client ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private ConnectedClient GetClientById(string id)
		{
			if (this.connectedClients.ContainsKey(id))
				return this.connectedClients[id];

			var connectedAccount = this.connectedAccounts.FirstOrDefault(c => c.Client.Id == id);
			return connectedAccount.Client;
		}
		
		/// <summary>
		/// Returns an Account ID.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		private ConnectedAccount GetConnectedAccount(string id)
        {
			var connectedAccount = this.connectedAccounts.FirstOrDefault(c => c.Client.Id == id);
			return connectedAccount;
        }

		/// <summary>
		/// Returns Connected Rooms for Map Draw, But Better.
		/// </summary>
		/// <param name="room"></param>
		/// <returns></returns>
		private List<Room> FindRooms (Room room)
        {
			List<Room> roomList = new();
			int[,] grid = new int[5, 5];
			grid[2, 2] = room.ID;
			for (int x = 2; x < 5; x++)
            {
				for (int y = 2; y < 5; y++)
                {
					if (rooms[grid[x, y]].CanGoNorth)
                    {
						grid[x - 1, y] = rooms[grid[x,y]].North;
						if (rooms[grid[x - 1, y]].CanGoNorth)
							grid[x - 2, y] = rooms[grid[x - 1, y]].North;
					}
					if (rooms[grid[x, y]].CanGoSouth)
                    {
						grid[x + 1, y] = rooms[grid[x, y]].South;
						if (rooms[grid[x + 1, y]].CanGoSouth)
							grid[x + 2, y] = rooms[grid[x + 1, y]].South;

					}
					if (rooms[grid[x, y]].CanGoWest)
                    {
						grid[x, y - 1] = rooms[grid[x, y]].West;
						if (rooms[grid[x, y - 1]].CanGoWest)
							grid[x, y - 2] = rooms[grid[x, y - 1]].West;

					}
					if (rooms[grid[x, y]].CanGoEast)
                    {
						grid[x, y + 1] = rooms[grid[x, y]].East;
						if (rooms[grid[x, y + 1]].CanGoEast)
							grid[x, y + 2] = rooms[grid[x, y + 1]].East;
					}
				}
            }
			for (int x = 2; x > -1; x--)
            {
				for (int y = 2; y > -1; y--)
                {
					if (rooms[grid[x, y]].CanGoNorth)
					{
						grid[x - 1, y] = rooms[grid[x, y]].North;
						if (rooms[grid[x - 1, y]].CanGoNorth)
							grid[x - 2, y] = rooms[grid[x - 1, y]].North;
					}
					if (rooms[grid[x, y]].CanGoSouth)
					{
						grid[x + 1, y] = rooms[grid[x, y]].South;
						if (rooms[grid[x + 1, y]].CanGoSouth)
							grid[x + 2, y] = rooms[grid[x + 1, y]].South;

					}
					if (rooms[grid[x, y]].CanGoWest)
					{
						grid[x, y - 1] = rooms[grid[x, y]].West;
						if (rooms[grid[x, y - 1]].CanGoWest)
							grid[x, y - 2] = rooms[grid[x, y - 1]].West;

					}
					if (rooms[grid[x, y]].CanGoEast)
					{
						grid[x, y + 1] = rooms[grid[x, y]].East;
						if (rooms[grid[x, y + 1]].CanGoEast)
							grid[x, y + 2] = rooms[grid[x, y + 1]].East;
					}
				}
            }
			foreach (int gridLoc in grid)
			{
				roomList.Add(rooms[gridLoc]);
			}

			return roomList;
		}

		/// <summary>
		/// Added Queues to the Queue Library & ConcurrentQueue.
		/// </summary>
		/// <param name="queue"></param>
		/// <param name="currentTimeStamp"></param>
		/// <param name="command"></param>
		/// <param name="uid"></param>
		public void AddToQueue(bool queue, double currentTimeStamp, Command command = null, string uid = "")
		{
			if (queue)
			{
				if (actionQueue.ContainsKey(currentTimeStamp))
				{
					foreach (ClientCommand clientCommand in actionQueue[currentTimeStamp])
					{
						queuedCommand.Enqueue(clientCommand);
					}
				}
			}
			else
			{
				if (actionQueue.ContainsKey(currentTimeStamp))
				{
					actionQueue[currentTimeStamp].Add(new ClientCommand
					{
						Id = uid,
						Command = command
					});
				}
				else
				{
					actionQueue.Add(currentTimeStamp, new List<ClientCommand>
					{
						new ClientCommand
						{
							Id = uid,
							Command = command
						}
					});
				}
			}
		}

        #endregion

        #region -- Server Update Requests --
        
		#endregion
    }
}
