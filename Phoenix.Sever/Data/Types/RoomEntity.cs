using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Server
{
    public class RoomEntity
    {
        public int Id { get; set; }
        public int EntityId { get; set; }
        public int RoomId { get; set; }

        public RoomEntity(int id, int entityid, int roomid)
        {
            this.Id = id;
            this.EntityId = entityid;
            this.RoomId = roomid;
        }
    }
}
