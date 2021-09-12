namespace Phoenix.Server
{
    public class RoomEntity
    {
        public int ID { get; set; }
        public int EntityID { get; set; }
        public int RoomID { get; set; }

        public RoomEntity()
        {

        }

        public RoomEntity(int id, int entityid, int roomid)
        {
            this.ID = id;
            this.EntityID = entityid;
            this.RoomID = roomid;
        }
    }
}
