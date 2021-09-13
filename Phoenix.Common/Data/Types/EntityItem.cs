namespace Phoenix.Common.Data.Types
{
    public class EntityItem
    {
        public int Id { get; set; }
        public int EntityId { get; set; }
        public int ItemId { get; set; }
        public double DropChance { get; set; }
        public int ItemAmount { get; set; }

        public EntityItem()
        {

        }

        public EntityItem(int id, int entityid, int itemid, double dropchance, int itemamount)
        {
            this.Id = id;
            this.EntityId = entityid;
            this.ItemId = itemid;
            this.DropChance = dropchance;
            this.ItemAmount = itemamount;
        }
    }
}
