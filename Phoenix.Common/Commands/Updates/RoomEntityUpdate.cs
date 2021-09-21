using Phoenix.Common.Commands.Factory;
using Phoenix.Common.Data.Types;
using System.Collections.Generic;

namespace Phoenix.Common.Commands.Updates
{
    public class RoomNPCUpdate :Command
    {
		#region -- Properties --
		public int Mode { get; set; }
		public NPC NPC { get; set; }
		#endregion

		public RoomNPCUpdate()
		{
			this.CommandType = CommandType.RoomNPCUpdate;
		}

		public override IEnumerable<IEnumerable<string>> GetCommandParts()
		{
			return new List<List<string>>
			{
				new List<string>
				{
					this.Mode.ToString(),
					this.NPC.DisplayName,
					this.NPC.Image,
					this.NPC.Type
				}
			};
		}
	}
}
