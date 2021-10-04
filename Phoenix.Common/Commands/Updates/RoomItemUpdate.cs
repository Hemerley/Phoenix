using Phoenix.Common.Commands.Factory;
using Phoenix.Common.Data.Types;
using System.Collections.Generic;

namespace Phoenix.Common.Commands.Updates
{
    public class RoomItemUpdate : Command
    {
        #region -- Properties --
        public int Mode { get; set; }
        public Item Item { get; set; }
        #endregion

        public RoomItemUpdate()
        {
            this.CommandType = CommandType.RoomItemUpdate;
        }

        public override IEnumerable<IEnumerable<string>> GetCommandParts()
        {
            return new List<List<string>>
            {
                new List<string>
                {
                    this.Mode.ToString(),
                    this.Item.Name,
                    this.Item.Image,
                    this.Item.Rarity
                }
            };
        }
    }
}
