using System.Collections.Generic;

namespace Phoenix.Common
{
	public class MessageRoomCommand : Command
	{
		#region -- Properties --

		public string Message { get; set; }

		#endregion

		public MessageRoomCommand()
		{
			this.CommandType = CommandType.MessageRoom;
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
