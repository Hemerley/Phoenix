using Phoenix.Common.Commands.Factory;
using System.Collections.Generic;

namespace Phoenix.Common.Commands.Response
{
	public class AuthenticateResponse : Command
	{

		// Setup To Bool
		#region -- Properties --

		public string Message { get; set; } = "\0";
        public bool Success { get; set; }

		#endregion

		public AuthenticateResponse()
		{
			this.CommandType = CommandType.AuthenticateResponse;
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
