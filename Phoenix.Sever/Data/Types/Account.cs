using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Server
{
    public class Account
    {
		public int Id { get; set; }
		public int Gold { get; set; }
		public Character Character { get; set; }
		public Account(int id, int gold)
		{
			this.Id = id;
			this.Gold = gold;
		}
	}
}
