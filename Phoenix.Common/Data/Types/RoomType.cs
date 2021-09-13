namespace Phoenix.Common.Data.Types
{
    public class RoomType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public RoomType()
        {

        }

        public RoomType(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
