using System.Collections.Generic;

namespace Phoenix.Common
{
	public class AuthenticateCommand : Command
	{
		#region -- Properties --

		public string Username { get; set; }

		public string Password { get; set; }

		#endregion

		public AuthenticateCommand()
		{
			this.CommandType = CommandType.Authenticate;
		}

		public override IEnumerable<IEnumerable<string>> GetCommandParts()
		{
			return new List<List<string>>
			{
				new List<string>
				{
					this.Username,
					this.Password
				}
			};
		}
	}
}
