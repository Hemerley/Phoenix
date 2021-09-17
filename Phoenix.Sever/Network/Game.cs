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
									clientConnectResponseCommand.Message = $"Welcome to {Constants.GAME_NAME}, {character.Name}! There are {connectedCharacters.Count} players online. We have had {totalConnections} total connections and a maximum of {maximumPlayers} players online this reboot. The current time is {DateTime.Now.ToShortTimeString()}.";

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
		/// Returns Connect Rooms for Map Draw.
		/// </summary>
		/// <param name="room"></param>
		/// <returns></returns>
		private List<Room> FindRooms(Room room)
        {
			List<Room> roomsList = new ();
			int[,] grid = new int[5, 5];
			grid[2, 2] = room.ID;

            #region -- Basic --
            // North
            if (room.CanGoNorth)
				grid[1, 2] = room.North;
			if (rooms[grid[1, 2]].CanGoNorth)
				grid[0, 2] = room.North;

			// South
			if (room.CanGoSouth)
				grid[3, 2] = room.South;
			if (rooms[grid[3, 2]].CanGoSouth)
				grid[4, 2] = room.South;

			// West
			if (room.CanGoWest)
				grid[2, 1] = room.West;
			if (rooms[grid[2, 1]].CanGoWest)
				grid[2, 0] = room.West;
		
			// east
			if (room.CanGoEast)
				grid[2, 3] = room.East;
			if (rooms[grid[2, 3]].CanGoEast)
				grid[2, 4] = room.East;
			#endregion

			#region -- North Path --
			// Northest-West
			if (rooms[grid[0, 2]].CanGoWest)
				grid[0, 1] = rooms[grid[0, 2]].West;
			if (rooms[grid[0, 1]].CanGoWest)
				grid[0, 0] = rooms[grid[0, 1]].West;

			// Northless-West
			if (rooms[grid[1, 2]].CanGoWest)
				grid[1, 1] = rooms[grid[1, 2]].West;
			if (rooms[grid[1, 1]].CanGoWest)
				grid[1, 0] = rooms[grid[1, 1]].West;

			// Northest-East
			if (rooms[grid[0, 2]].CanGoEast)
				grid[0, 3] = rooms[grid[0, 2]].East;
			if (rooms[grid[0, 3]].CanGoWest)
				grid[0, 4] = rooms[grid[0, 3]].East;

			// Northless-East
			if (rooms[grid[1, 2]].CanGoEast)
				grid[1, 3] = rooms[grid[1, 2]].East;
			if (rooms[grid[1, 3]].CanGoEast)
				grid[1, 4] = rooms[grid[1, 3]].East;
            #endregion

            #region -- South Path --
            // Southest-West
            if (rooms[grid[4, 2]].CanGoWest)
				grid[4, 1] = rooms[grid[4, 2]].West;
			if (rooms[grid[4, 1]].CanGoWest)
				grid[4, 0] = rooms[grid[4, 1]].West;

			// Southest-West
			if (rooms[grid[3, 2]].CanGoWest)
				grid[3, 1] = rooms[grid[3, 2]].West;
			if (rooms[grid[3, 1]].CanGoWest)
				grid[3, 0] = rooms[grid[3, 1]].West;

			// Southest-East
			if (rooms[grid[4, 2]].CanGoEast)
				grid[4, 3] = rooms[grid[4, 2]].East;
			if (rooms[grid[4, 3]].CanGoWest)
				grid[4, 4] = rooms[grid[4, 3]].East;

			// Southess-East
			if (rooms[grid[3, 2]].CanGoEast)
				grid[3, 3] = rooms[grid[3, 2]].East;
			if (rooms[grid[3, 3]].CanGoEast)
				grid[3, 4] = rooms[grid[3, 3]].East;
			#endregion

			#region -- West Path --
			// Westest-North
			if (rooms[grid[2, 0]].CanGoNorth)
				grid[1, 0] = rooms[grid[2, 0]].North;
			if (rooms[grid[1, 0]].CanGoNorth)
				grid[0, 0] = rooms[grid[1, 0]].North;

			// Westest-North
			if (rooms[grid[2, 1]].CanGoNorth)
				grid[1, 1] = rooms[grid[2, 1]].North;
			if (rooms[grid[1, 1]].CanGoNorth)
				grid[0, 1] = rooms[grid[1, 1]].North;

			// Westest-South
			if (rooms[grid[2, 0]].CanGoSouth)
				grid[3, 0] = rooms[grid[2, 0]].South;
			if (rooms[grid[3, 0]].CanGoSouth)
				grid[4, 0] = rooms[grid[3, 0]].South;

			// Westess-West
			if (rooms[grid[2, 1]].CanGoSouth)
				grid[3, 1] = rooms[grid[2, 1]].South;
			if (rooms[grid[3, 1]].CanGoSouth)
				grid[4, 1] = rooms[grid[3, 1]].South;
			#endregion

			#region -- East Path --
			// Eastest-North
			if (rooms[grid[2, 4]].CanGoNorth)
				grid[1, 4] = rooms[grid[2, 4]].North;
			if (rooms[grid[1, 4]].CanGoNorth)
				grid[0, 4] = rooms[grid[1, 4]].North;

			// Eastest-North
			if (rooms[grid[2, 3]].CanGoNorth)
				grid[1, 3] = rooms[grid[2, 3]].North;
			if (rooms[grid[1, 3]].CanGoNorth)
				grid[0, 3] = rooms[grid[1, 3]].North;

			// Eastest-South
			if (rooms[grid[2, 4]].CanGoSouth)
				grid[3, 4] = rooms[grid[2, 4]].South;
			if (rooms[grid[3, 4]].CanGoSouth)
				grid[4, 4] = rooms[grid[3, 4]].South;

			// Eastess-West
			if (rooms[grid[2, 3]].CanGoSouth)
				grid[3, 3] = rooms[grid[2, 3]].South;
			if (rooms[grid[3, 3]].CanGoSouth)
				grid[3, 4] = rooms[grid[3, 3]].South;
			#endregion

			foreach (int gridLoc in grid)
            {
				roomsList.Add(rooms[gridLoc]);
            }

			return roomsList;

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
