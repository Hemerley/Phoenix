using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Common
{
    public class ConnectedCommand : Command
    {
		#region -- Properties --

		/// <summary>
		/// 0 - Fail
		/// 1 - Success
		/// </summary>
		public int Success { get; set; }

		#endregion

		public ConnectedCommand()
		{
			this.CommandType = CommandType.Connected;
		}

		public override IEnumerable<string> GetCommandParts()
		{
			return new List<string> { this.Success.ToString() };
		}
	}
}
