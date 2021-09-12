using System.Collections.Generic;
using System.Linq;

namespace Phoenix.Common
{
	public abstract class Command
	{
		public CommandType CommandType { get; set; }

		public virtual IEnumerable<string> GetCommandParts() { return Enumerable.Empty<string>(); }
	}
}
