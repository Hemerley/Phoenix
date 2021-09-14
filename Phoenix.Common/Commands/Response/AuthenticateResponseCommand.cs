using Phoenix.Common.Commands.Factory;
using System.Collections.Generic;

namespace Phoenix.Common.Commands.Response
{
	public class AuthenticateResponseCommand : Command
	{

		// Setup To Bool
		#region -- Properties --

		//<CommandID>^<Payload>
		//<CommandID>^<Status><Payload>

		/// <summary>
		/// 0 - Fail
		/// 1 - Success
		/// </summary>
		public bool Success { get; set; }

		#endregion

		public AuthenticateResponseCommand()
		{
			this.CommandType = CommandType.AuthenticateResponse;
		}

		public override IEnumerable<IEnumerable<string>> GetCommandParts()
		{
			return new List<List<string>>
			{
				new List<string>
				{
					this.Success.ToString()
				}
			};
		}
	}
}
