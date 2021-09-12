namespace Phoenix.Server
{
    public class RoomItem
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int ItemId { get; set; }

        public RoomItem()
        {

        }

        public RoomItem(int id, int roomid, int itemid)
        {
            this.Id = id;
            this.RoomId = roomid;
            this.ItemId = itemid;
        }
    }
}
