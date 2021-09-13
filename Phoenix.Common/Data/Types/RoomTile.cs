namespace Phoenix.Common.Data.Types
{
    public class RoomTile
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public RoomTile()
        {

        }

        public RoomTile(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
     }
}
