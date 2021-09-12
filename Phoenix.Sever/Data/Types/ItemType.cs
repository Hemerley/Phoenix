namespace Phoenix.Server
{
    public class ItemType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ItemType()
        {

        }

        public ItemType(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
