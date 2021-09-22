using Phoenix.Server.Connections;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Unclassified.Net;

namespace Phoenix.Server.Network
{
    public class Listener
    {
        #region -- Fields --

        /// <summary>
        /// TCP Listener that does all of the TCP logic.
        /// </summary>
        private AsyncTcpListener listener;

        /// <summary>
        /// The list of currently connected clients.
        /// </summary>
        private readonly List<ConnectedClient> clients = new();

        #endregion

        #region -- Properties --

        #endregion

        #region -- Events --

        /// <summary>
        /// Event fires when a new client connects.
        /// </summary>
        public event EventHandler<ConnectedClient> OnClientConnected;

        /// <summary>
        /// Event fires when a client disconnects.
        /// </summary>
        public event EventHandler<ConnectedClient> OnClientDisconnected;

        /// <summary>
        /// Event fires when a connected client sends a message.
        /// </summary>
        public event EventHandler<ConnectedClientMessage> OnClientMessage;

        /// <summary>
        /// Event fires when the server starts listening.
        /// </summary>
        public event EventHandler OnServerStarted;

        /// <summary>
        /// Event fires when the server stops listening.
        /// </summary>
        public event EventHandler OnServerStopped;

        #endregion

        /// <summary>
        /// Starts the server.
        /// </summary>
        /// <param name="ip">The listening IP address.</param>
        /// <param name="port">The listening port.</param>
        public void Start(IPAddress ip, int port)
        {
            if (this.listener != null)
                Stop();

            this.listener = new AsyncTcpListener
            {
                IPAddress = ip,
                Port = port,
                ClientConnectedCallback = ClientConnected
            };

            this.listener.Message += Listener_Message;

            this.listener.RunAsync();
        }

        /// <summary>
        /// Handles the TCP library's messages.
        /// Needed to know when it starts/stops.
        /// </summary>
        /// <param name="sender">Listener object.</param>
        /// <param name="e">Contains the message of what is happening.</param>
        private void Listener_Message(object sender, AsyncTcpEventArgs e)
        {
            //DUMB way to check for this, but what the library gives us
            if (e.Message.Contains("Waiting for connections"))
                this.OnServerStarted?.Invoke(this, EventArgs.Empty);
            else if (e.Message.Contains("Shutting down"))
                this.OnServerStopped?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Stops the server.
        /// </summary>
        public void Stop()
        {
            this.listener?.Stop(true);
            this.listener = null;
        }

        /// <summary>
        /// Sends a message to all connected clients.
        /// </summary>
        /// <param name="msg">The message to send.</param>
        public void Send(string msg)
        {
            foreach (var client in this.clients)
            {
                client.Send(msg);
            }
        }

        /// <summary>
        /// Handler for when a client is connected.
        /// </summary>
        /// <param name="tcpClient">The client that connected.</param>
        /// <returns>Async Task.</returns>
        private Task ClientConnected(TcpClient tcpClient)
        {
            var client = new ConnectedClient(tcpClient, this)
            {
                Disconnected = ClientClosed
            };

            this.clients.Add(client);

            this.OnClientConnected?.Invoke(this, client);

            return client.RunAsync();
        }

        /// <summary>
        /// Handler for when a client sends a message to the server.
        /// </summary>
        /// <param name="client">The client that sent the message.</param>
        /// <param name="message">The message that was sent.</param>
        public void ClientMessage(ConnectedClient client, string message)
        {
            this.OnClientMessage?.Invoke(this, new ConnectedClientMessage
            {
                ConnectedClient = client,
                Message = message
            });
        }

        /// <summary>
        /// Handler for when a client disconnects from the server.
        /// </summary>
        /// <param name="tcpClient">The client that disconnected.</param>
        /// <param name="closedByRemote">True if it was closed remotely.</param>
        private void ClientClosed(ConnectedClient tcpClient, bool closedByRemote)
        {
            this.OnClientDisconnected?.Invoke(this, tcpClient);
            this.clients.Remove(tcpClient);
        }
    }
}
