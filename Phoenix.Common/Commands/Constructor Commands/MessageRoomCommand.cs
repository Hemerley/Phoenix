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

		public override IEnumerable<IEnumerable<string>> GetCommandParts()
		{
			return new List<List<string>>
			{
				new List<string>
				{
					this.Message
				}
			};
		}
	}
}
