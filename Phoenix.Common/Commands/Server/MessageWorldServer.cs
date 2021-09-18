using Phoenix.Common.Commands.Factory;
using System.Collections.Generic;

namespace Phoenix.Common.Commands.Server
{
    public class MessageWorldServer : Command
	{
		#region -- Properties --

		public string Message { get; set; }
		public int ID { get; set; } = -1;

        #endregion

        public MessageWorldServer()
		{
			this.CommandType = CommandType.MessageWorld;
		}

		public override IEnumerable<IEnumerable<string>> GetCommandParts()
		{
			return new List<List<string>>
			{
				new List<string>
				{
					this.Message,
					this.ID.ToString()
				}
			};
		}
	}
}
