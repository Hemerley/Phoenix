using Phoenix.Common.Commands.Factory;
using Phoenix.Common.Data.Types;
using System.Collections.Generic;

namespace Phoenix.Common.Commands.Updates
{
    public class RoomPlayerUpdate : Command
    {
        #region -- Properties --
        public int Mode { get; set; }
        public Character Character { get; set; }
        #endregion

		public RoomPlayerUpdate()
        {
			this.CommandType = CommandType.RoomPlayerUpdate;
        }

		public override IEnumerable<IEnumerable<string>> GetCommandParts()
		{
			return new List<List<string>>
			{
				new List<string>
				{
					this.Mode.ToString(),
					this.Character.Name,
					this.Character.Image,
					this.Character.Type
				}
			};
		}
	}
}
