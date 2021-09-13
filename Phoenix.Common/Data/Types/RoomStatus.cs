namespace Phoenix.Common.Data.Types
{
    public class RoomStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public RoomStatus()
        {

        }

        public RoomStatus(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
