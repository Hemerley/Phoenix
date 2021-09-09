using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Unclassified.Net;

namespace Phoenix.Client
{
    class Client
    {
		#region -- Fields --

		/// <summary>TCP connection to the server.</summary>
		private AsyncTcpClient tcpClient;

		#endregion

		#region -- Properties --

		#endregion

		#region -- Events --

		/// <summary>Sample event to demo when the server receives something from the server.</summary>
		public event EventHandler<string> OnActivity;

		#endregion

		public void Start(IPAddress ip, int port)
		{
			this.tcpClient = new AsyncTcpClient
			{
				IPAddress = ip,
				Port = port,
				ReceivedCallback = ReceivedCallback
			};

			//this.tcpClient.Message += TcpClient_Message;
			this.tcpClient.RunAsync();
		}

		private Task ReceivedCallback(AsyncTcpClient client, int count)
		{
			byte[] bytes = client.ByteBuffer.Dequeue(count);
			string message = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
			Console.WriteLine("Client: received: " + message);
			this.OnActivity?.Invoke(this, "Server Message");
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
