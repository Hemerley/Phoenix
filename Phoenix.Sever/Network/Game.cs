using Phoenix.Common.Commands.Factory;
using Phoenix.Common.Commands.Failure;
using Phoenix.Common.Commands.Request;
using Phoenix.Common.Commands.Response;
using Phoenix.Common.Commands.Server;
using Phoenix.Common.Commands.Staff;
using Phoenix.Common.Commands.Updates;
using Phoenix.Common.Data;
using Phoenix.Common.Data.Types;
using Phoenix.Server.Connections;
using Phoenix.Server.Data;
using Phoenix.Server.Functions;
using Serilog;
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

		#region -- Private --

		/// <summary>
		/// Declaration of the Server.
		/// </summary>
		private Listener server;

		/// <summary>
		/// Boolean for Game Thread.
		/// </summary>
		private readonly bool stopGameWorkerThread = false;

		/// <summary>
		/// Declaration of Game Thread that controls Game Logic.
		/// </summary>
		private Thread gameWorkerThread;

		#endregion

		#region -- Public --

		/// <summary>
		/// Contains unique ID for server.
		/// </summary>
		public Guid serverID = Guid.NewGuid();

		/// <summary>
		/// Declaration of Concurrent Queue that handles queue of commands.
		/// </summary>
		public readonly ConcurrentQueue<ClientCommand> queuedCommand = new();

		/// <summary>
		/// Queue Command Library.
		/// </summary>
		public readonly SortedDictionary<double, List<ClientCommand>> actionQueue = new();

		/// <summary>
		/// Connected clients that are NOT authenticated yet.
		/// </summary>
		public readonly Dictionary<string, ConnectedClient> connectedClients = new();

		/// <summary>
		/// Connected clients that ARE authenticated.
		/// </summary>
		public readonly List<ConnectedAccount> connectedAccounts = new();

		/// <summary>
		/// Declaration of Loaded Rooms.
		/// </summary>
		public List<Room> rooms = new();

		/// <summary>
		/// Declaration of Connected Characters.
		/// </summary>
		public readonly List<Character> connectedCharacters = new();

		/// <summary>
		/// Declaration of Current Entities Spawned.
		/// </summary>
		public readonly List<Entity> currentEntities = new();

		/// <summary>
		/// Declaration of Current Rooms.
		/// </summary>
		public readonly List<Room> currentRooms = new();

		/// <summary>
		/// Declaration of Total Connections This Reboot.
		/// </summary>
		public int totalConnections = 0;

		/// <summary>
		/// Declaration of Maximum Players.
		/// </summary>
		public int maximumPlayers = 0;

		#endregion

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
			Log.Information($"{e.Id} has connected.");
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
				ToolFunctions.AddToQueue(false, currentTimeStamp, command, e.ConnectedClient.Id);
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
			Log.Information($"{e.Id} has disconnected.");
		}

		/// <summary>
		/// Handled Server Boot.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Server_OnServerStarted(object sender, EventArgs e)
		{
			// Log Command
			Log.Information("Server Started!");
		}

		/// <summary>
		/// Handles Server Shutdown.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Server_OnServerStopped(object sender, EventArgs e)
		{
			// Log Command
			Log.Information("Server Shutdown!");
		}

        #endregion

        #region -- Send Commands --

		/// <summary>
		/// Sends Command to a Client.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="command"></param>
        public void SendCommandToClient(ConnectedClient client, Command command)
		{
			var message = CommandFactory.FormatCommand(command);
			client.Send(message);
			Log.Information($"{command.CommandType} sent to {client.Id}");
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
			Log.Information("Loading Rooms...");
			this.rooms = Database.LoadRooms(Constants.GAME_MODE);
			Log.Information("Rooms Loaded!");

			// Initialize Server.
			this.server = new Listener();

			// Register Events.
			this.server.OnClientConnected += Server_OnClientConnected;
			this.server.OnClientDisconnected += Server_OnClientDisconnected;
			this.server.OnClientMessage += Server_OnClientMessage;
			this.server.OnServerStarted += Server_OnServerStarted;
			this.server.OnServerStopped += Server_OnServerStopped;

			// Start Listening.
			this.server.Start(IPAddress.IPv6Any, Constants.LIVE_PORT);

			ToolFunctions.AddToQueue(false, DateTimeOffset.Now.ToUnixTimeSeconds() + 30, new SpawnEntityServer(), serverID.ToString());
			#endregion

			while (!this.stopGameWorkerThread)
			{

				#region -- Queue Handling --

				// Establish Time Stamp.
				var currentTimeStamp = DateTimeOffset.Now.ToUnixTimeSeconds();

				// Check Queue Stack & Remove Items with TimeStamp. Hands off to Queue.
				ToolFunctions.AddToQueue(true, currentTimeStamp - 1);
				actionQueue.Remove(currentTimeStamp - 1);
				ToolFunctions.AddToQueue(true, currentTimeStamp);
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
				var clientWhoSendCommand = ToolFunctions.GetClientById(clientId);
				var accountConnected = ToolFunctions.GetConnectedAccount(clientId);

				Log.Information($"{command.CommandType} from {clientId}.");

				// Check if player is connected.
				if (clientWhoSendCommand == null && accountConnected == null && clientId != serverID.ToString())
					continue;

                #endregion

                switch (command.CommandType)
				{

                    #region -- Authenticate Response --

                    case CommandType.Authenticate:
						{
							var parsedCommand = command as AuthenticateRequest;

							SendCommandToClient(clientWhoSendCommand, ServerFunctions.Authenticate(parsedCommand.Version, parsedCommand.Username.ToLower(), parsedCommand.Password, clientWhoSendCommand, accountConnected));

							break;
						}
					#endregion

                    #region -- New Account Response --

                    case CommandType.NewAccount:
						{
							var parsedCommand = command as NewAccountRequest;

							SendCommandToClient(clientWhoSendCommand, ServerFunctions.NewAccount(parsedCommand.Version, parsedCommand.Username.ToLower(), parsedCommand.Password, parsedCommand.Email, clientWhoSendCommand, accountConnected));

                            break;
						}
					#endregion

					#region -- New Character Command --

					case CommandType.NewCharacter:
						{
							var parsedCommand = command as NewCharacterCommand;

							SendCommandToClient(clientWhoSendCommand, ServerFunctions.NewCharacter(parsedCommand.CharacterName.ToLower(), parsedCommand.Gender, parsedCommand.Philosophy, parsedCommand.Image, clientWhoSendCommand, accountConnected));

							break;
						}
					#endregion

					#region -- Character List Response --

					case CommandType.CharacterList:
						{
							var parsedCommand = command as GetCharacterListRequest;

							SendCommandToClient(clientWhoSendCommand, ServerFunctions.CharacterList(clientWhoSendCommand, accountConnected));

							break;
						}
					#endregion

					#region -- Character Login Response --

					case CommandType.CharacterLogin:
						{
							var parsedCommand = command as CharacterConnectRequest;

							SendCommandToClient(clientWhoSendCommand, ServerFunctions.CharacterConnect(parsedCommand.Name.ToLower(), clientWhoSendCommand, accountConnected));

							break;
						}
					#endregion

					#region -- Message Room Response --
					case CommandType.MessageRoom:
						{
							var parsedCommand = command as MessageRoomServer;

							MessageFunctions.MessageRoom(true, parsedCommand.Message, accountConnected);

							break;
						}
					#endregion

					#region  -- Message Direct Response --
					case CommandType.MessageDirect:
						{
							var parsedCommand = command as MessageDirectServer;
							MessageFunctions.MessageDirect(true, parsedCommand.Message, parsedCommand.SendingName.ToLower(), parsedCommand.ReceivingName.ToLower(), accountConnected);
							break;
						}
					#endregion

					#region  -- Message Party Response --
					#endregion

					#region  -- Message Guild Response --
					#endregion

					#region -- Message World Response --
					case CommandType.MessageWorld:
						{
							var parsedCommand = command as MessageWorldServer;
							MessageFunctions.MessageWorld(true, parsedCommand.Message, parsedCommand.ID, accountConnected);
							break;
						}
					#endregion

					#region  -- Message Broadcast Response --
					#endregion

					#region -- Client Connect Response --
					case CommandType.ClientConnect:
						{
							var parsedCommand = command as ClientConnectRequest;
							SendCommandToClient(clientWhoSendCommand, ServerFunctions.ClientConnect(parsedCommand.Id, accountConnected));
							break;							
						}
					#endregion

					#region -- Client Room Response --
					case CommandType.ClientRoom:
						{
							var parseCommand = command as ClientRoomRequest;
							ServerFunctions.ClientRoom(parseCommand.RoomID, accountConnected);
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
							var parsedCommand = command as PlayerMoveRequest;

							RoomFunctions.MovePlayer(true, -1, -1, parsedCommand.Direction, accountConnected);
							
							break;
						}
					#endregion

					#region  -- Spawn Entity --
					case CommandType.SpawnEntity:
						{
							var parsedCommand = command as SpawnEntityServer;

							ServerFunctions.SpawnEntity(parsedCommand.CharacterName, parsedCommand.EntityName);
						}
						break;
					#endregion

					#region  -- Summon Player --
					case CommandType.SummonPlayer:
						{
							var parsedCommand = command as SummonPlayerStaff;

							RoomFunctions.MovePlayer(false, 1, -1, "", accountConnected, $"&tilda&mA bright white portal opens and &tilda&w{parsedCommand.Name.ToLower().FirstCharToUpper()}&tilda&m steps through.", parsedCommand.Name.ToLower(), $"&tilda&mA bright white portal opens and &tilda&w{parsedCommand.Name.ToLower().FirstCharToUpper()}&tilda&m is pulled into it!");
						}
						break;
					#endregion

					#region -- Unknown Command --

					case CommandType.Unknown:
					default:
						{
							Log.Error($"Unknown command from {clientId}.");
							break;
						}
                    #endregion

                }
                Thread.Sleep(10);
			}
		}

        #endregion

    }
}
