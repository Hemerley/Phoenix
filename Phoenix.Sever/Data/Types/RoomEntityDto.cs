using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Server
{
    public class RoomEntityDto
    {
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public int? EntityId { get; set; }
        public string EntityName { get; set; }
    }
}
