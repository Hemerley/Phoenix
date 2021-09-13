namespace Phoenix.Common.Data.Types
{
    public class ItemSlot
    {
        public int Id { get; set; }
        public string Name { get; set;  }

        public ItemSlot()
        {

        }

        public ItemSlot(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
