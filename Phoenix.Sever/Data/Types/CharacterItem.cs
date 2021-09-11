using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Server
{
    public class CharacterItem
    {
        public int Id { get; set; }
        public int CharacterId { get; set; }
        public int ItemId { get; set; }
        public int ItemAmount { get; set; }
        public int SlotIndex { get; set; }
        public int IsEquipped { get; set; }

        public CharacterItem(int id, int characterid, int itemid, int itemamount, int slotindex, int isequipped)
        {
            this.Id = id;
            this.CharacterId = characterid;
            this.ItemId = itemid;
            this.ItemAmount = itemamount;
            this.SlotIndex = slotindex;
            this.IsEquipped = isequipped;
        }
    }
}
