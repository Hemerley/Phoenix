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

		/// <summary>Connected clients that are NOT authenticated yet.</summary>
		private Dictionary<string, ConnectedClient> connectedClients = new Dictionary<string, ConnectedClient>();

		/// <summary>Connected clients that ARE authenticated.</summary>
		private List<ConnectedAccount> connectedAccounts = new List<ConnectedAccount>();

		private bool stopGameWorkerThread = false;

		private Thread gameWorkerThread;

		private ConcurrentQueue<ClientCommand> queuedCommand = new ConcurrentQueue<ClientCommand>();

		public void Start()
		{
			//Setup your Game thread
			this.gameWorkerThread = new Thread(new ThreadStart(GameWorkerThread));
			this.gameWorkerThread.Start();

			this.server = new Server();

			this.server.OnClientConnected += Server_OnClientConnected;
			this.server.OnClientDisconnected += Server_OnClientDisconnected;
			this.server.OnClientMessage += Server_OnClientMessage;
			this.server.OnServerStarted += Server_OnServerStarted;
			this.server.OnServerStopped += Server_OnServerStopped;

			this.server.Start(IPAddress.IPv6Any, 4444);
		}

		private void Server_OnClientConnected(object sender, ConnectedClient e)
		{
			//store the connected client
			this.connectedClients.Add(e.Id, e);

			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine($"[{DateTime.Now}][Connection]: {e.Id}  has connected.");
			Console.ResetColor();
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

			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine($"[{DateTime.Now}][Connection]: {e.Id} has disconnected.");
			Console.ResetColor();
		}

		private void Server_OnServerStarted(object sender, EventArgs e)
		{
			//TODO: Log this
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine($"[{DateTime.Now}][System]: Server Started");
			Console.ResetColor();
		}

		private void Server_OnServerStopped(object sender, EventArgs e)
		{
			//TODO: Log this
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine($"[{DateTime.Now}][System]: Server Stopped");
			Console.ResetColor();
		}

		private void SendMessageToClient(ConnectedClient client)
		{
			var messageCmd = new MessageCommand
			{
				Message = $"Hey Client! You are known to me as {client.Id}."
			};

			SendCommandToClient(client, messageCmd);
		}

		private void SendCommandToClient(ConnectedClient client, Command command)
		{
			var message = CommandFactory.FormatCommand(command);
			client.Send(message);
		}

		/// <summary>
		/// Game Loop.
		/// </summary>
		private void GameWorkerThread()
		{
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
							this.connectedAccounts.Add(new ConnectedAccount
							{
								Client = clientWhoSendCommand
							});

							this.connectedClients.Remove(clientId);
						}

						break;
					case CommandType.Message:
						Console.ForegroundColor = ConsoleColor.Yellow;
						Console.WriteLine($"[{DateTime.Now}][Command]: {command.CommandType} from {clientId}.");
						Console.ResetColor();
						//TODO: Send the message to everyone in the same room as e.ConnectedClient.Id
						break;
					case CommandType.Unknown:
					default:
						Console.ForegroundColor = ConsoleColor.Yellow;
						Console.WriteLine($"[{DateTime.Now}][Command]: Unknown command from {clientId}.");
						Console.ResetColor();
						break;
				}

				Thread.Sleep(10);
			}
		}

		private ConnectedClient GetClientById(string id)
		{
			if (this.connectedClients.ContainsKey(id))
				return this.connectedClients[id];

			var connectedAccount = this.connectedAccounts.FirstOrDefault(c => c.Client.Id == id);
			return connectedAccount.Client;
		}
	}
}
