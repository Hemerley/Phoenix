using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Unclassified.Net;

namespace Phoenix.Client.Classes.Network
{
    public class Client
    {
		#region -- Fields --

		/// <summary>TCP connection to the server.</summary>
		private AsyncTcpClient tcpClient;

		#endregion

		#region -- Properties --

		#endregion

		#region -- Events --

		/// <summary>
		/// Sample event to demo when the server receives something from the server.
		/// </summary>
		public event EventHandler<string> OnActivity;
		public event EventHandler<bool> IsConnected;
		public event EventHandler<bool> IsClosed;

		#endregion

		public void Start(IPAddress ip, int port)
		{
			this.tcpClient = new AsyncTcpClient
			{
				IPAddress = ip,
				Port = port,
				ReceivedCallback = ReceivedCallback,
				ConnectedCallback = ConnectedCallback,
				ClosedCallback = ClosedCallback
			};

			//this.tcpClient.Message += TcpClient_Message;
			this.tcpClient.RunAsync();
		}

		private Task ConnectedCallback(AsyncTcpClient client, bool isReconnected)
        {
			this.IsConnected?.Invoke(this, isReconnected);
			return Task.CompletedTask;
        }

		protected virtual void ClosedCallback(AsyncTcpClient client, bool remote)
        {
			this.IsClosed?.Invoke(this, remote);
		}

		private Task ReceivedCallback(AsyncTcpClient client, int count)
		{
			byte[] bytes = client.ByteBuffer.Dequeue(count);
			string message = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
			this.OnActivity?.Invoke(this, message);
			return Task.CompletedTask;
		}

		public void Send(string msg)
		{
			var bytes = Encoding.UTF8.GetBytes(msg);
			this.tcpClient.Send(new ArraySegment<byte>(bytes));
		}

		public void Stop()
		{
			this.tcpClient.Disconnect();
		}
	}
}
