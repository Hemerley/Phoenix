using Phoenix.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;

namespace Phoenix.Server
{
	public class Game
	{
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

		// Templates


		// Game as Lives
		private List<Room> rooms = new();

		public void Start()
		{

			//Setup your Game thread
			this.gameWorkerThread = new Thread(new ThreadStart(GameWorkerThread));
			this.gameWorkerThread.Start();
		}

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

		private void SendMessageToClient(ConnectedClient client)
		{
			var messageCmd = new MessageRoomCommand
			{
				Message = $"Hey Client! You are known to me as {client.Id}."
			};

			SendCommandToClient(client, messageCmd);
		}

		private void SendCommandToClient(ConnectedClient client, Command command)
		{
			var message = CommandFactory.FormatCommand(command);
			client.Send(message);
			Logger.ConsoleLog("Command", $"{command.CommandType} sent to {client.Id}");
		}

		/// <summary>
		/// Game Loop.
		/// </summary>
		private void GameWorkerThread()
		{

			Database.InitializeDatabse();
			this.rooms = Database.LoadRooms("Live");

			foreach(var room in this.rooms)
            {
				Logger.ConsoleLog("System", $"{room.Name}");
            }

			/*
			 * Load Rooms
			 * Rooms Request Entity Creation
			 * Rooms 
			 * 
			 */

			this.server = new Server();

			this.server.OnClientConnected += Server_OnClientConnected;
			this.server.OnClientDisconnected += Server_OnClientDisconnected;
			this.server.OnClientMessage += Server_OnClientMessage;
			this.server.OnServerStarted += Server_OnServerStarted;
			this.server.OnServerStopped += Server_OnServerStopped;

			this.server.Start(IPAddress.IPv6Any, 4444);

			while (!this.stopGameWorkerThread)
			{
				if (!this.queuedCommand.TryDequeue(out ClientCommand cmd))
				{
					Thread.Sleep(10);
					continue;
				}

				var clientId = cmd.Id;
				var command = cmd.Command;
				var clientWhoSendCommand = GetClientById(clientId);

				switch (command.CommandType)
				{
					case CommandType.Authenticate:
						// Log Command
						Logger.ConsoleLog("Command", $"{command.CommandType} from {clientId}.");

						var authCommand = command as AuthenticateCommand;
						//authCommand.Username

						//Do authenticate logic....
						var authenticated = true;

						//tell them if they connected 
						var authResponseCmd = new AuthenticateResponseCommand
						{
							Success = authenticated ? 1 : 0
						};

						SendCommandToClient(clientWhoSendCommand, authResponseCmd);

						if (authenticated)
						{
							var connectedAccount = new ConnectedAccount
							{
								Client = clientWhoSendCommand
							};
							//connectAccount.Account = this.database.LoadAcccount(authCommand.Username);
							this.connectedAccounts.Add(connectedAccount);
							this.connectedClients.Remove(clientId);
						}

						break;
					case CommandType.NewAccount:
						//TODO: Create New Account.
						break;
					case CommandType.MessageRoom:
						//TODO: Send the message to everyone in the same room as e.ConnectedClient.Id
						// Log Command
						Logger.ConsoleLog("Command", $"{command.CommandType} from {clientId}.");
						break;
					case CommandType.Unknown:
					default:
						// Log Command
						Logger.ConsoleLog("Command", $"Unknown command from {clientId}.");
						break;
				}

				Thread.Sleep(10);
			}
		}

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
	}
}
