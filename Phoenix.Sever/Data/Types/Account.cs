using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Server
{
    public class Account
    {
		public int id { get; set; }
		public int gold { get; set; }
		public Character character { get; set; }
		public Account(int id, int gold)
		{
			this.id = id;
			this.gold = gold;
		}
	}
}
