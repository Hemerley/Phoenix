using Phoenix.Common.Commands.Factory;
using System.Collections.Generic;

namespace Phoenix.Common.Commands.Response
{
    public class ClientConnectResponse : Command
    {
		#region -- Properties --

		public bool Success { get; set; }

		public string Message { get; set; } = "None";
		#endregion

		public ClientConnectResponse()
		{
			this.CommandType = CommandType.ClientConnectResponse;
		}

		public override IEnumerable<IEnumerable<string>> GetCommandParts()
		{

			return new List<List<string>>
			{
				new List<string>
				{
					this.Success.ToString(),
					this.Message
				}
			};
		}
	}
}
