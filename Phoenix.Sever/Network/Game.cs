using Phoenix.Common.Commands.Factory;
using Phoenix.Common.Commands.Request;
using Phoenix.Common.Commands.Response;
using Phoenix.Common.Commands.Server;
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
			string[] commands = e.Message.Split("%", StringSplitOptions.RemoveEmptyEntries);
			foreach (string c in commands)
			{
				var command = CommandFactory.ParseCommand(c);
				double currentTimeStamp = DateTimeOffset.Now.ToUnixTimeSeconds();
				AddToQueue(false, currentTimeStamp, command, e.ConnectedClient.Id);
			}
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
										SendCommandToClient(cAccount.Client, new RoomPlayerUpdate
										{
											Mode = 2,
											Character = connectedAccount.Account.Character
										});
										SendCommandToClient(cAccount.Client, new MessageWorldServer
										{
											Message = $"&tilda&g{connectedAccount.Account.Character.Name}&tilda&w has went &tilda&roffline&tilda&w!"
										});
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

		/// <summary>
		/// Sends Command to a Client.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="command"></param>
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

			// Load Database .
            Database.InitializeDatabse();
			Logger.ConsoleLog("System", "Loading Rooms.");
			this.rooms = Database.LoadRooms(Constants.GAME_MODE);

			// Initialize Server.
			this.server = new Server();

			// Register Events.
			this.server.OnClientConnected += Server_OnClientConnected;
			this.server.OnClientDisconnected += Server_OnClientDisconnected;
			this.server.OnClientMessage += Server_OnClientMessage;
			this.server.OnServerStarted += Server_OnServerStarted;
			this.server.OnServerStopped += Server_OnServerStopped;

			// Start Listening.
			this.server.Start(IPAddress.IPv6Any, Constants.LIVE_PORT);

            #endregion

            while (!this.stopGameWorkerThread)
			{

				#region -- Queue Handling --

				// Establish Time Stamp.
				var currentTimeStamp = DateTimeOffset.Now.ToUnixTimeSeconds();

				// Check Queue Stack & Remove Items with TimeStamp. Hands off to Queue.
				AddToQueue(true, currentTimeStamp - 1);
				actionQueue.Remove(currentTimeStamp - 1);
				AddToQueue(true, currentTimeStamp);
				actionQueue.Remove(currentTimeStamp);
				
				// Check Queue for items.
				if (!this.queuedCommand.TryDequeue(out ClientCommand cmd))
				{
					Thread.Sleep(10);
					continue;
				}

				// Handle Queue items.
				var clientId = cmd.Id;
				var command = cmd.Command;
				var clientWhoSendCommand = GetClientById(clientId);
				var accountConnected = GetConnectedAccount(clientId);

				// Log Command.
				Logger.ConsoleLog("Command", $"{command.CommandType} from {clientId}.");

				// Check if player is connected.
				if (clientWhoSendCommand == null && accountConnected == null)
					continue;

                #endregion

                switch (command.CommandType)
				{

                    #region -- Authenticate Response --

                    case CommandType.Authenticate:
						{
							var authenticateCommand = command as AuthenticateRequest;

							if (authenticateCommand.Version != new Version(Constants.GAME_VERSION))
                            {
								SendCommandToClient(clientWhoSendCommand, new AuthenticateResponse
								{
									Success = false,
									Message = "Incorrect Version Number. Please use the Patcher to run the application."
								});
								break;
							}

							if (Database.GetAccountField(Constants.GAME_MODE, "Password", "Name", authenticateCommand.Username) != authenticateCommand.Password)
							{
								SendCommandToClient(clientWhoSendCommand, new AuthenticateResponse
								{
									Success = false,
									Message = "Account Name or Password doesn't match."
								});
								break;
							}

							this.connectedAccounts.Add(new ConnectedAccount
							{
								Client = clientWhoSendCommand,
								Account = new Account
								{
									Id = Int32.Parse(Database.GetAccountField(Constants.GAME_MODE, "ID", "Name", authenticateCommand.Username)),
									Gold = Int32.Parse(Database.GetAccountField(Constants.GAME_MODE, "Gold", "Name", authenticateCommand.Username))
								}
							});
							this.connectedClients.Remove(clientId);

							SendCommandToClient(clientWhoSendCommand, new AuthenticateResponse
							{
								Success = true
							});
							break;
						}
					#endregion

                    #region -- New Account Response --

                    case CommandType.NewAccount:
						{
							var newAccountCommand = command as NewAccountRequest;

							if (newAccountCommand.Version != new Version(Constants.GAME_VERSION))
							{
								var authenticateResponseCommand = new AuthenticateResponse
								{
									Success = false,
									Message = "Incorrect Version Number. Please use the Patcher to run the application."
								};
								SendCommandToClient(clientWhoSendCommand, authenticateResponseCommand);
								break;
							}

							if (Database.GetAccountField(Constants.GAME_MODE, "Name", "Name", newAccountCommand.Username) == null)
							{
								Database.InsertNewAccount(Constants.GAME_MODE, newAccountCommand.Username, newAccountCommand.Password, newAccountCommand.Email);
								Logger.ConsoleLog("System", $"{clientId} has created a new account named: {newAccountCommand.Username}.");

								this.connectedAccounts.Add(new ConnectedAccount
								{
									Client = clientWhoSendCommand,
									Account = new Account
									{
										Id = Int32.Parse(Database.GetAccountField(Constants.GAME_MODE, "ID", "Name", newAccountCommand.Username)),
										Gold = Int32.Parse(Database.GetAccountField(Constants.GAME_MODE, "Gold", "Name", newAccountCommand.Username))
									}
								});
								this.connectedClients.Remove(clientId);
								SendCommandToClient(clientWhoSendCommand, new NewAccountResponse
								{
									Success = true
								});
							}
							else
							{
								Logger.ConsoleLog("System", $"{clientId} has failed to create a new account named: {newAccountCommand.Username}. Reason: Account Exists.");
								SendCommandToClient(clientWhoSendCommand, new NewAccountResponse
								{
									Success = false,
									Message = $"Failed to create a new account named: {newAccountCommand.Username}. Reason: Account Exists."
								});
							}
							break;
						}
					#endregion

					#region -- New Character Command --

					case CommandType.NewCharacter:
						{
							var newCharacterCommand = command as NewCharacterCommand;

							if (Database.GetCharacterField(Constants.GAME_MODE, "Name", "Name", newCharacterCommand.CharacterName) == null)
							{
								Database.InsertNewCharacter(Constants.GAME_MODE, newCharacterCommand.CharacterName, newCharacterCommand.Gender, newCharacterCommand.Philosophy, newCharacterCommand.Image, accountConnected.Account.Id);
								Logger.ConsoleLog("System", $"{clientId} has created a new character named: {newCharacterCommand.CharacterName}");
								SendCommandToClient(clientWhoSendCommand, new NewCharacterResponse 
								{
									Success = true
								});
							}
							else
							{
								Logger.ConsoleLog("System", $"{clientId} has failed to create new character named: {newCharacterCommand.CharacterName}. Reason: Character Exists.");
								SendCommandToClient(clientWhoSendCommand, new NewCharacterResponse
								{
									Success = false,
									Message = $"Failed to create new character named: {newCharacterCommand.CharacterName}. Reason: Character Exists."
								});
							}
							break;
						}
					#endregion

					#region -- Character List Response --

					case CommandType.CharacterList:
						{
							var newCharacterList = command as GetCharacterListRequest;

							List<Character> characters = Database.GetCharacterList(Constants.GAME_MODE, accountConnected.Account.Id);

							SendCommandToClient(clientWhoSendCommand, new CharacterListResponse
							{
								Success = characters.Count > 0,
								Characters = characters
							});
							break;
						}
					#endregion

					#region -- Character Login Response --

					case CommandType.CharacterLogin:
						{
							var characterConnectionCommand = command as CharacterConnectRequest;

							Character loginCharacter = Database.GetCharacter(Constants.GAME_MODE, accountConnected.Account.Id, characterConnectionCommand.Name);

							foreach (Character character in connectedCharacters)
                            {
								if (character.Name == loginCharacter.Name)
                                {
									SendCommandToClient(clientWhoSendCommand, new CharacterConnectResponse
									{
										Success = false,
										Message = "Character is already online!",
										Character = null
									});
									goto exit_loop;
								}
                            }

							SendCommandToClient(clientWhoSendCommand, new CharacterConnectResponse
							{
								Success = loginCharacter != null,
								Message = loginCharacter != null ? "None": "Failed to locate character, please contact a God for further assistance!", 
								Character = loginCharacter
							});

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
													SendCommandToClient(cAccount.Client, new RoomPlayerUpdate
													{
														Mode = 1,
														Character = loginCharacter
													});
													Thread.Sleep(10);
													SendCommandToClient(cAccount.Client, new MessageWorldServer
													{
														Message = $"&tilda&g{loginCharacter.Name}&tilda&w has come &tilda&gonline&tilda&w!"
													});
												}
                                            }
                                        }
										room.RoomCharacters.Add(loginCharacter);
									}
                                }
								totalConnections++;
								if (maximumPlayers < connectedCharacters.Count) maximumPlayers++;
							}
							break;
						}
					#endregion

					#region -- Message Room Response --
					case CommandType.MessageRoom:
						{
							var messageRoomCommand = command as MessageRoomServer;

							foreach (ConnectedAccount connectedAccount in connectedAccounts)
							{
								if (connectedAccount.Account.Character != null)
								{
									if (connectedAccount.Account.Character.RoomID == accountConnected.Account.Character.RoomID)
									{
										SendCommandToClient(connectedAccount.Client, new MessageRoomServer
										{
											Character = accountConnected.Account.Character,
											Message = messageRoomCommand.Message
										});
									}
								}
							}
							break;
						}
					#endregion

					#region  -- Message Player Response --
					#endregion

					#region  -- Message Party Response --
					#endregion

					#region  -- Message Guild Response --
					#endregion

					#region -- Message World Response --
					case CommandType.MessageWorld:
						{
							var messageWorldCommand = command as MessageWorldServer;

							if (accountConnected.Account.Character.Id == messageWorldCommand.ID)
							{
								if (accountConnected.Account.Character.TypeID > 2)
								{
									foreach (ConnectedAccount connectedAccount in connectedAccounts)
									{
										if (connectedAccount.Account.Character != null)
										{
											SendCommandToClient(connectedAccount.Client, new MessageWorldServer
											{
												Message = messageWorldCommand.Message
											});
										}
									}

								}
							}
							break;
						}
					#endregion

					#region  -- Message Broadcast Response --
					#endregion

					#region -- Client Connect Response --
					case CommandType.ClientConnect:
						{
							var clientConnectCommand = command as ClientConnectRequest;

							foreach (Character character in connectedCharacters)
							{
								if (character.Id == clientConnectCommand.Id)
								{
									SendCommandToClient(clientWhoSendCommand, new ClientConnectResponse
									{
										Success = true,
										Message = $"&tilda&w&tilda&gWelcome to &tilda&w{Constants.GAME_NAME}&tilda&g, &tilda&w{character.Name}&tilda&g! There are &tilda&w{connectedCharacters.Count}&tilda&g players online. We have had &tilda&w{totalConnections}&tilda&g total connections and a maximum of &tilda&w{maximumPlayers}&tilda&g players online this reboot. The current time is &tilda&w{DateTime.Now.ToShortTimeString()}&tilda&g.\n"
									});
									goto exit_loop;
								}
							}
							SendCommandToClient(clientWhoSendCommand, new ClientConnectResponse
							{
								Success = false
							});
							break;							
						}
					#endregion

					#region -- Client Room Response --
					case CommandType.ClientRoom:
						{
							var clientRoomCommand = command as ClientRoomRequest;
							var clientRoomResponseCommand = new ClientRoomResponse();

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

									var roomMapResponseCommand = new RoomMapResponse
									{
										Success = true,
										RoomsHigh = 5,
										RoomsWide = 5,
										Rooms = FindRooms(rooms[clientRoomCommand.RoomID])
									};
									SendCommandToClient(clientWhoSendCommand, clientRoomResponseCommand);
									Thread.Sleep(10);
									SendCommandToClient(clientWhoSendCommand, roomMapResponseCommand);
									goto exit_loop;
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

					#region  -- Map Response --
					#endregion

					#region  -- Room Player Update --
					#endregion

					#region  -- Room Entity Update --
					#endregion

					#region  -- Player Move Request --
					case CommandType.PlayerMoveRequest:
						{
							var playerMoveRequest = command as PlayerMoveRequest;

							switch (playerMoveRequest.Direction)
                            {
								case "north":
									{
										if (rooms[accountConnected.Account.Character.RoomID].CanGoNorth)
										{
											var clientRoomResponseCommand = new ClientRoomResponse();
											foreach (ConnectedAccount connectedAccount in connectedAccounts)
											{
												if (connectedAccount.Account.Character == null)
													continue;
												if (connectedAccount.Account.Character.RoomID == accountConnected.Account.Character.RoomID)
												{
													SendCommandToClient(connectedAccount.Client, new RoomPlayerUpdate
													{
														Mode = 2,
														Character = accountConnected.Account.Character
													});
													if (connectedAccount.Client != accountConnected.Client)
													{
														SendCommandToClient(connectedAccount.Client, new MessageRoomServer
														{
															Message = $"&tilda&w{accountConnected.Account.Character.Name} has moved to the {playerMoveRequest.Direction}."
														});
													}
												}
												if (connectedAccount.Account.Character.RoomID == rooms[accountConnected.Account.Character.RoomID].North)
												{
													SendCommandToClient(connectedAccount.Client, new RoomPlayerUpdate
													{
														Mode = 1,
														Character = accountConnected.Account.Character
													});
													if (connectedAccount.Client != accountConnected.Client)
													{
														SendCommandToClient(connectedAccount.Client, new MessageRoomServer
														{
															Message = $"&tilda&w{accountConnected.Account.Character.Name} has arrived from the south."
														});
													}
												}
											}
											rooms[accountConnected.Account.Character.RoomID].RoomCharacters.Remove(accountConnected.Account.Character);
											rooms[rooms[accountConnected.Account.Character.RoomID].North].RoomCharacters.Add(accountConnected.Account.Character);

											accountConnected.Account.Character.RoomID = rooms[accountConnected.Account.Character.RoomID].North;

											clientRoomResponseCommand.Success = true;
											clientRoomResponseCommand.Room.Name = rooms[accountConnected.Account.Character.RoomID].Name;
											clientRoomResponseCommand.Room.Description = rooms[accountConnected.Account.Character.RoomID].Description;
											clientRoomResponseCommand.Room.Exits = rooms[accountConnected.Account.Character.RoomID].Exits;
											clientRoomResponseCommand.Room.Type = rooms[accountConnected.Account.Character.RoomID].Type;
											foreach (Character character in rooms[accountConnected.Account.Character.RoomID].RoomCharacters)
											{
												Character newCharacter = new();
												newCharacter.Name = character.Name;
												newCharacter.Image = character.Image;
												newCharacter.Type = character.Type;
												clientRoomResponseCommand.Room.RoomCharacters.Add(newCharacter);
											}

											foreach (Entity entity in rooms[accountConnected.Account.Character.RoomID].RoomEntities)
											{
												Entity newEntity = new();
												newEntity.Name = entity.Name;
												newEntity.Image = entity.Image;
												newEntity.Type = entity.Type;
												clientRoomResponseCommand.Room.RoomEntities.Add(newEntity);
											}

											foreach (Item item in rooms[accountConnected.Account.Character.RoomID].RoomItems)
											{
												Item newItem = new();
												newItem.Name = item.Name;
												newItem.Image = item.Image;
												newItem.Type = item.Type;
												newItem.Amount = item.Amount;
											}
											SendCommandToClient(clientWhoSendCommand, clientRoomResponseCommand);
											Thread.Sleep(10);
											SendCommandToClient(clientWhoSendCommand, new RoomMapResponse
											{
												Success = true,
												RoomsHigh = 5,
												RoomsWide = 5,
												Rooms = FindRooms(rooms[accountConnected.Account.Character.RoomID])
											});
										}
                                        else
                                        {
											SendCommandToClient(accountConnected.Client, new MessageRoomServer
											{
												Message = $"&tilda&rYou cannot move that direction."
											});
										}
										goto exit_loop;
									}
								case "south":
									{
										if (rooms[accountConnected.Account.Character.RoomID].CanGoSouth)
										{
											var clientRoomResponseCommand = new ClientRoomResponse();
											foreach (ConnectedAccount connectedAccount in connectedAccounts)
											{
												if (connectedAccount.Account.Character == null)
													continue;
												if (connectedAccount.Account.Character.RoomID == accountConnected.Account.Character.RoomID)
												{
													SendCommandToClient(connectedAccount.Client, new RoomPlayerUpdate
													{
														Mode = 2,
														Character = accountConnected.Account.Character
													});
													if (connectedAccount.Client != accountConnected.Client)
													{
														SendCommandToClient(connectedAccount.Client, new MessageRoomServer
														{
															Message = $"&tilda&w{accountConnected.Account.Character.Name} has moved to the {playerMoveRequest.Direction}."
														});
													}
												}
												if (connectedAccount.Account.Character.RoomID == rooms[accountConnected.Account.Character.RoomID].South)
												{
													SendCommandToClient(connectedAccount.Client, new RoomPlayerUpdate
													{
														Mode = 1,
														Character = accountConnected.Account.Character
													});
													if (connectedAccount.Client != accountConnected.Client)
													{
														SendCommandToClient(connectedAccount.Client, new MessageRoomServer
														{
															Message = $"&tilda&w{accountConnected.Account.Character.Name} has arrived from the north."
														});
													}
												}
											}
											rooms[accountConnected.Account.Character.RoomID].RoomCharacters.Remove(accountConnected.Account.Character);
											rooms[rooms[accountConnected.Account.Character.RoomID].South].RoomCharacters.Add(accountConnected.Account.Character);

											accountConnected.Account.Character.RoomID = rooms[accountConnected.Account.Character.RoomID].South;

											clientRoomResponseCommand.Success = true;
											clientRoomResponseCommand.Room.Name = rooms[accountConnected.Account.Character.RoomID].Name;
											clientRoomResponseCommand.Room.Description = rooms[accountConnected.Account.Character.RoomID].Description;
											clientRoomResponseCommand.Room.Exits = rooms[accountConnected.Account.Character.RoomID].Exits;
											clientRoomResponseCommand.Room.Type = rooms[accountConnected.Account.Character.RoomID].Type;
											foreach (Character character in rooms[accountConnected.Account.Character.RoomID].RoomCharacters)
											{
												Character newCharacter = new();
												newCharacter.Name = character.Name;
												newCharacter.Image = character.Image;
												newCharacter.Type = character.Type;
												clientRoomResponseCommand.Room.RoomCharacters.Add(newCharacter);
											}

											foreach (Entity entity in rooms[accountConnected.Account.Character.RoomID].RoomEntities)
											{
												Entity newEntity = new();
												newEntity.Name = entity.Name;
												newEntity.Image = entity.Image;
												newEntity.Type = entity.Type;
												clientRoomResponseCommand.Room.RoomEntities.Add(newEntity);
											}

											foreach (Item item in rooms[accountConnected.Account.Character.RoomID].RoomItems)
											{
												Item newItem = new();
												newItem.Name = item.Name;
												newItem.Image = item.Image;
												newItem.Type = item.Type;
												newItem.Amount = item.Amount;
											}
											SendCommandToClient(clientWhoSendCommand, clientRoomResponseCommand);
											Thread.Sleep(10);
											SendCommandToClient(clientWhoSendCommand, new RoomMapResponse
											{
												Success = true,
												RoomsHigh = 5,
												RoomsWide = 5,
												Rooms = FindRooms(rooms[accountConnected.Account.Character.RoomID])
											});
										}
										else
										{
											SendCommandToClient(accountConnected.Client, new MessageRoomServer
											{
												Message = $"&tilda&rYou cannot move that direction."
											});
										}
										goto exit_loop;
									}
								case "west":
									{
										if (rooms[accountConnected.Account.Character.RoomID].CanGoWest)
										{
											var clientRoomResponseCommand = new ClientRoomResponse();
											foreach (ConnectedAccount connectedAccount in connectedAccounts)
											{
												if (connectedAccount.Account.Character == null)
													continue;
												if (connectedAccount.Account.Character.RoomID == accountConnected.Account.Character.RoomID)
												{
													SendCommandToClient(connectedAccount.Client, new RoomPlayerUpdate
													{
														Mode = 2,
														Character = accountConnected.Account.Character
													});
													if (connectedAccount.Client != accountConnected.Client)
													{
														SendCommandToClient(connectedAccount.Client, new MessageRoomServer
														{
															Message = $"&tilda&w{accountConnected.Account.Character.Name} has moved to the {playerMoveRequest.Direction}."
														});
													}
												}
												if (connectedAccount.Account.Character.RoomID == rooms[accountConnected.Account.Character.RoomID].West)
												{
													SendCommandToClient(connectedAccount.Client, new RoomPlayerUpdate
													{
														Mode = 1,
														Character = accountConnected.Account.Character
													});
													if (connectedAccount.Client != accountConnected.Client)
													{
														SendCommandToClient(connectedAccount.Client, new MessageRoomServer
														{
															Message = $"&tilda&w{accountConnected.Account.Character.Name} has arrived from the east."
														});
													}
												}
											}
											rooms[accountConnected.Account.Character.RoomID].RoomCharacters.Remove(accountConnected.Account.Character);
											rooms[rooms[accountConnected.Account.Character.RoomID].West].RoomCharacters.Add(accountConnected.Account.Character);

											accountConnected.Account.Character.RoomID = rooms[accountConnected.Account.Character.RoomID].West;

											clientRoomResponseCommand.Success = true;
											clientRoomResponseCommand.Room.Name = rooms[accountConnected.Account.Character.RoomID].Name;
											clientRoomResponseCommand.Room.Description = rooms[accountConnected.Account.Character.RoomID].Description;
											clientRoomResponseCommand.Room.Exits = rooms[accountConnected.Account.Character.RoomID].Exits;
											clientRoomResponseCommand.Room.Type = rooms[accountConnected.Account.Character.RoomID].Type;
											foreach (Character character in rooms[accountConnected.Account.Character.RoomID].RoomCharacters)
											{
												Character newCharacter = new();
												newCharacter.Name = character.Name;
												newCharacter.Image = character.Image;
												newCharacter.Type = character.Type;
												clientRoomResponseCommand.Room.RoomCharacters.Add(newCharacter);
											}

											foreach (Entity entity in rooms[accountConnected.Account.Character.RoomID].RoomEntities)
											{
												Entity newEntity = new();
												newEntity.Name = entity.Name;
												newEntity.Image = entity.Image;
												newEntity.Type = entity.Type;
												clientRoomResponseCommand.Room.RoomEntities.Add(newEntity);
											}

											foreach (Item item in rooms[accountConnected.Account.Character.RoomID].RoomItems)
											{
												Item newItem = new();
												newItem.Name = item.Name;
												newItem.Image = item.Image;
												newItem.Type = item.Type;
												newItem.Amount = item.Amount;
											}
											SendCommandToClient(clientWhoSendCommand, clientRoomResponseCommand);
											Thread.Sleep(10);
											SendCommandToClient(clientWhoSendCommand, new RoomMapResponse
											{
												Success = true,
												RoomsHigh = 5,
												RoomsWide = 5,
												Rooms = FindRooms(rooms[accountConnected.Account.Character.RoomID])
											});
										}
										else
										{
											SendCommandToClient(accountConnected.Client, new MessageRoomServer
											{
												Message = $"&tilda&rYou cannot move that direction."
											});
										}
										goto exit_loop;
									}

								case "east":
									{
										if (rooms[accountConnected.Account.Character.RoomID].CanGoEast)
										{
											var clientRoomResponseCommand = new ClientRoomResponse();
											foreach (ConnectedAccount connectedAccount in connectedAccounts)
											{
												if (connectedAccount.Account.Character == null)
													continue;
												if (connectedAccount.Account.Character.RoomID == accountConnected.Account.Character.RoomID)
												{
													SendCommandToClient(connectedAccount.Client, new RoomPlayerUpdate
													{
														Mode = 2,
														Character = accountConnected.Account.Character
													});
													if (connectedAccount.Client != accountConnected.Client)
													{
														SendCommandToClient(connectedAccount.Client, new MessageRoomServer
														{
															Message = $"&tilda&w{accountConnected.Account.Character.Name} has moved to the {playerMoveRequest.Direction}."
														});
													}
												}
												if (connectedAccount.Account.Character.RoomID == rooms[accountConnected.Account.Character.RoomID].East)
												{
													SendCommandToClient(connectedAccount.Client, new RoomPlayerUpdate
													{
														Mode = 1,
														Character = accountConnected.Account.Character
													});
													if (connectedAccount.Client != accountConnected.Client)
													{
														SendCommandToClient(connectedAccount.Client, new MessageRoomServer
														{
															Message = $"&tilda&w{accountConnected.Account.Character.Name} has arrived from the west."
														});
													}
												}
											}
											rooms[accountConnected.Account.Character.RoomID].RoomCharacters.Remove(accountConnected.Account.Character);
											rooms[rooms[accountConnected.Account.Character.RoomID].East].RoomCharacters.Add(accountConnected.Account.Character);

											accountConnected.Account.Character.RoomID = rooms[accountConnected.Account.Character.RoomID].East;

											clientRoomResponseCommand.Success = true;
											clientRoomResponseCommand.Room.Name = rooms[accountConnected.Account.Character.RoomID].Name;
											clientRoomResponseCommand.Room.Description = rooms[accountConnected.Account.Character.RoomID].Description;
											clientRoomResponseCommand.Room.Exits = rooms[accountConnected.Account.Character.RoomID].Exits;
											clientRoomResponseCommand.Room.Type = rooms[accountConnected.Account.Character.RoomID].Type;
											foreach (Character character in rooms[accountConnected.Account.Character.RoomID].RoomCharacters)
											{
												Character newCharacter = new();
												newCharacter.Name = character.Name;
												newCharacter.Image = character.Image;
												newCharacter.Type = character.Type;
												clientRoomResponseCommand.Room.RoomCharacters.Add(newCharacter);
											}

											foreach (Entity entity in rooms[accountConnected.Account.Character.RoomID].RoomEntities)
											{
												Entity newEntity = new();
												newEntity.Name = entity.Name;
												newEntity.Image = entity.Image;
												newEntity.Type = entity.Type;
												clientRoomResponseCommand.Room.RoomEntities.Add(newEntity);
											}

											foreach (Item item in rooms[accountConnected.Account.Character.RoomID].RoomItems)
											{
												Item newItem = new();
												newItem.Name = item.Name;
												newItem.Image = item.Image;
												newItem.Type = item.Type;
												newItem.Amount = item.Amount;
											}
											SendCommandToClient(clientWhoSendCommand, clientRoomResponseCommand);
											Thread.Sleep(10);
											SendCommandToClient(clientWhoSendCommand, new RoomMapResponse
											{
												Success = true,
												RoomsHigh = 5,
												RoomsWide = 5,
												Rooms = FindRooms(rooms[accountConnected.Account.Character.RoomID])
											});
										}
										else
										{
											SendCommandToClient(accountConnected.Client, new MessageRoomServer
											{
												Message = $"&tilda&rYou cannot move that direction."
											});
										}
										goto exit_loop;
									}

								default:
									break;
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
			exit_loop:;
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
