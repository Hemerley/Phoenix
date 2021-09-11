using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Server
{
    public class RoomKeyMode
    {
        public int Id
        {
            get; set;
        }
        public string Name
        {
            get; set;
        }
        public RoomKeyMode(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
     }
}
