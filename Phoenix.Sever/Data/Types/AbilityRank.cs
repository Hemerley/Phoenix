using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Server
{
    public class AbilityRank
    {
        public int Id
        {
            get; set;
        }
        public string Name
        {
            get; set;
        }
        public AbilityRank(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
