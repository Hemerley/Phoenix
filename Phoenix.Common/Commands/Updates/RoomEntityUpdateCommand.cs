using Phoenix.Common.Commands.Factory;
using Phoenix.Common.Data.Types;
using System.Collections.Generic;

namespace Phoenix.Common.Commands.Updates
{
    public class RoomEntityUpdateCommand :Command
    {
		#region -- Properties --
		public int Mode { get; set; }
		public Entity Entity { get; set; }
		#endregion

		public RoomEntityUpdateCommand()
		{
			this.CommandType = CommandType.RoomEntityUpdate;
		}

		public override IEnumerable<IEnumerable<string>> GetCommandParts()
		{
			return new List<List<string>>
			{
				new List<string>
				{
					this.Mode.ToString(),
					this.Entity.Name,
					this.Entity.Image,
					this.Entity.Type
				}
			};
		}
	}
}
