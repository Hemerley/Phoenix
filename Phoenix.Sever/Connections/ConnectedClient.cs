using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Unclassified.Net;

namespace Phoenix.Server.Connections
{
	public class ConnectedClient
	{
		public Action<ConnectedClient, bool> Disconnected;

		private readonly AsyncTcpClient tcpClient;

		private readonly Network.Server server;

		public string Id { get; private set; }

		public ConnectedClient(TcpClient tcpClient, Network.Server server)
		{
			this.Id = Guid.NewGuid().ToString();

			this.tcpClient = new AsyncTcpClient
			{
				ServerTcpClient = tcpClient,
				ConnectedCallback = ClientConnected,
				ReceivedCallback = MessageRecieved,
				ClosedCallback = ClientClosed
			};

			this.server = server;
		}

		private Task ClientConnected(AsyncTcpClient client, bool isReconnected)
		{
			return Task.CompletedTask;
		}

		private Task MessageRecieved(AsyncTcpClient client, int count)
		{
			byte[] bytes = client.ByteBuffer.Dequeue(count);
			string message = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

			this.server.ClientMessage(this, message);
			return Task.CompletedTask;
		}

		private void ClientClosed(AsyncTcpClient client, bool remote)
		{
			Disconnected?.Invoke(this, remote);
		}

		public Task RunAsync()
		{
			return tcpClient.RunAsync();
		}

		public void Send(string msg)
		{
			var bytes = Encoding.UTF8.GetBytes(msg);
			this.tcpClient.Send(new ArraySegment<byte>(bytes));
		}
	}
}
