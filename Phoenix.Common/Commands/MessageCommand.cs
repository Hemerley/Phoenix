using System.Collections.Generic;

namespace Phoenix.Common
{
	public class MessageCommand : Command
	{
		#region -- Properties --

		public string Message { get; set; }

		#endregion

		public MessageCommand()
		{
			this.CommandType = CommandType.Message;
		}

		public override IEnumerable<string> GetCommandParts()
		{
			return new List<string>
			{
				this.Message
			};
		}
	}
}
