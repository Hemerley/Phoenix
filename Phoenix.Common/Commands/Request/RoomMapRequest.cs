using Phoenix.Common.Commands.Factory;
using System.Collections.Generic;

namespace Phoenix.Common.Commands.Request
{
    class RoomMapRequest : Command
    {
		public int RoomID { get; set; }

		public RoomMapRequest()
		{
			this.CommandType = CommandType.MapRequest;
		}

		public override IEnumerable<IEnumerable<string>> GetCommandParts()
		{
			return new List<List<string>>
			{
				new List<string>
				{
					this.RoomID.ToString()
				}
			};
		}
	}
}
