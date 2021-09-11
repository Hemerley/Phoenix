using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Server
{
    public class Account
    {

		/// <summary>
		/// Stores Account ID.
		/// </summary>
		public int id { get; set; }

		/// <summary>
		/// Stores Character Strings.
		/// </summary>
		public string[] characters { get; set; }

		/// <summary>
		/// Stores Account Gold.
		/// </summary>
		public int gold { get; set; }

		/// <summary>
		/// Stores Account Vault.
		/// </summary>
		public string[] vault { get; set; }

		/// <summary>
		/// Stores Current Character.
		/// </summary>
		public Character character { get; set; }

		public Account(int id, string characters, int gold, string vault)
		{
			this.id = id;
			this.characters = characters.Split("|");
			this.gold = gold;
			this.vault = vault.Split("|");
		}
	}
}
