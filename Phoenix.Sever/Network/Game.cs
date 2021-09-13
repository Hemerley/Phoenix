using Phoenix.Common;
using Phoenix.Common.Data.Types;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;

namespace Phoenix.Server
{
	public class Game
	{
        #region -- Game Initialization --

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

		private List<Common.Data.Types.Room> rooms = new();

		public void Start()
		{

			//Setup your Game thread
			this.gameWorkerThread = new Thread(new ThreadStart(GameWorkerThread));
			this.gameWorkerThread.Start();
		}

        #endregion

        #region -- Events --

        private void Server_OnClientConnected(object sender, ConnectedClient e)
		{
			//store the connected client
			this.connectedClients.Add(e.Id, e);

			// Log Command
			Logger.ConsoleLog("Connection", $"{e.Id} has connected.");
		}

		private void Server_OnClientMessage(object sender, ConnectedClientMessage e)
		{
			var command = CommandFactory.ParseCommand(e.Message);
			this.queuedCommand.Enqueue(new ClientCommand
			{
				Id = e.ConnectedClient.Id,
				Command = command
			});
		}

		private void Server_OnClientDisconnected(object sender, ConnectedClient e)
		{
			//Remove from EITHER connected clients OR connected accounts (it could be in either)
			if (this.connectedClients.ContainsKey(e.Id))
				this.connectedClients.Remove(e.Id);

			//coding it this way to be safe, just in case we have a different instance of the same connection
			//I DOUBT we do though
			var connectedAccount = this.connectedAccounts.FirstOrDefault(c => c.Client.Id == e.Id);
			if (connectedAccount != null)
				this.connectedAccounts.Remove(connectedAccount);

			// Log Command
			Logger.ConsoleLog("Connection", $"{e.Id} has disconnected.");
		}

		private void Server_OnServerStarted(object sender, EventArgs e)
		{
			// Log Command
			Logger.ConsoleLog("System", "Server Started.");
		}

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
        /// Game Loop.
        /// </summary>
        private void GameWorkerThread()
		{
            #region --  Initialize Thread --
            Database.InitializeDatabse();
			Logger.ConsoleLog("System", "Loading Rooms.");
			this.rooms = Database.LoadRooms(Constants.GAME_MODE);

			foreach (var room in this.rooms)
			{
				Logger.ConsoleLog("System", $"{room.Name}");
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

                #region -- Queue Handling

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
							Success = authenticated ? 1 : 0
						};

                        SendCommandToClient(clientWhoSendCommand, authResponseCmd);

						if (authenticated)
						{
							var connectedAccount = new ConnectedAccount
							{
								Client = clientWhoSendCommand,
								Account = new Common.Data.Types.Account
								{
									Id = id,
									Gold = gold
								}
							};
							this.connectedAccounts.Add(connectedAccount);
							this.connectedClients.Remove(clientId);
						}

						break;

					#endregion

					#region -- GetCharacterList Command --
					case CommandType.CharacterList:
						Logger.ConsoleLog("Command", $"{command.CommandType} from {clientId}.");
						var newCharacterList = command as GetCharacterListCommand;

						List<Common.Data.Types.Character> characters = Database.GetCharacterList(Constants.GAME_MODE, accountConnected.Account.Id);

							var newCharacterListResponseCmd = new CharacterListResponseCommand
							{
								Characters = characters
							};
                            SendCommandToClient(clientWhoSendCommand, newCharacterListResponseCmd);

						break;
					#endregion

					#region -- CharacterConnectCommand --
					case CommandType.CharacterLogin:
						Logger.ConsoleLog("Command", $"{command.CommandType} from {clientId}.");
						var newCharacterLogin = command as CharacterConnectCommand;

						Character loginCharacter = Database.GetCharacter(Constants.GAME_MODE, accountConnected.Account.Id, newCharacterLogin.Name);

						var newCharacterConnectResponseCommand = new CharacterConnectResponseCommand
						{
							Success = loginCharacter == null ? 0:1 ,
							Character = loginCharacter
						};

						SendCommandToClient(clientWhoSendCommand, newCharacterConnectResponseCommand);
						break;
                    #endregion

                    #region -- NewAccount Command --

                    case CommandType.NewAccount:
						Logger.ConsoleLog("Command", $"{command.CommandType} from {clientId}.");
						var newAccountCommand = command as NewAccountCommand;
						var newAccountResponseCmd = new NewAccountResponseCommand();

						if (Database.GetAccountField(Constants.GAME_MODE, "Name", "Name", newAccountCommand.Username) == null)
                        {
							Database.InsertNewAccount(Constants.GAME_MODE, newAccountCommand.Username, newAccountCommand.Password, newAccountCommand.Email);
							Logger.ConsoleLog("System", $"{clientId} has created a new account named: {newAccountCommand.Username}.");
							newAccountResponseCmd.Success = 1;
                        }
                        else
                        {
							Logger.ConsoleLog("System", $"{clientId} has failed to create a new account named: {newAccountCommand.Username}. Reason: Account Exists.");
							newAccountResponseCmd.Success = 0;
                        }
                        SendCommandToClient(clientWhoSendCommand, newAccountResponseCmd);
						break;

					#endregion

					#region -- NewCharacter Command --

					case CommandType.NewCharacter:
						Logger.ConsoleLog("Command", $"{command.CommandType} from {clientId}.");
						var newCharacterCommand = command as NewCharacterCommand;

						var newCharacterResponseCmd = new NewCharacterResponseCommand();

						if (Database.GetCharacterField(Constants.GAME_MODE, "Name", "Name", newCharacterCommand.CharacterName) == null)
                        {
							Database.InsertNewCharacter(Constants.GAME_MODE, newCharacterCommand.CharacterName, newCharacterCommand.Gender, newCharacterCommand.Philosophy, newCharacterCommand.Image, accountConnected.Account.Id);
							Logger.ConsoleLog("System", $"{clientId} has created a new character named: {newCharacterCommand.CharacterName}");
							newCharacterResponseCmd.Success = 1;
						}
                        else
                        {
							Logger.ConsoleLog("System", $"{clientId} has failed to create new character named: {newCharacterCommand.CharacterName}. Reason: Character Exists.");
							newCharacterResponseCmd.Success = 1;
						}
                        SendCommandToClient(clientWhoSendCommand, newCharacterResponseCmd);
						break;

					#endregion

					#region -- MessageRoom Command --

					case CommandType.MessageRoom:
						Logger.ConsoleLog("Command", $"{command.CommandType} from {clientId}.");
						break;

					#endregion

					#region -- Unknown Command --

					case CommandType.Unknown:
					default:
						Logger.ConsoleLog("Command", $"Unknown command from {clientId}.");
						break;

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

		private ConnectedAccount GetConnectedAccount(string id)
        {
			var connectedAccount = this.connectedAccounts.FirstOrDefault(c => c.Client.Id == id);
			return connectedAccount;
        }

        #endregion

    }
}
