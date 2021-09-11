using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Server
{
    class Ability
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Philosophy { get; set; }

        public int Type { get; set; }

        public string Script { get; set; }

        public Ability(int id, string name, string philosophy, int type, string script)
        {
            this.Id = id;
            this.Name = name;
            this.Philosophy = philosophy;
            this.Type = type;
            this.Script = script;
        }

    }
}
