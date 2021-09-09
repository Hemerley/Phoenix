using System.Collections.Generic;

namespace Phoenix.Common
{
	public class AuthenticateResponseCommand : Command
	{
		#region -- Properties --

		/// <summary>
		/// 0 - Fail
		/// 1 - Success
		/// </summary>
		public int Success { get; set; }

		#endregion

		public AuthenticateResponseCommand()
		{
			this.CommandType = CommandType.AuthenticateResponse;
		}

		public override IEnumerable<string> GetCommandParts()
		{
			return new List<string> { this.Success.ToString() };
		}
	}
}
