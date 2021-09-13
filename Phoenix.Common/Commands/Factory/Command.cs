using System.Collections.Generic;
using System.Linq;

namespace Phoenix.Common.Commands.Factory
{
	public abstract class Command
	{
		public CommandType CommandType { get; set; }

		public virtual IEnumerable<IEnumerable<string>> GetCommandParts() { return Enumerable.Empty<IEnumerable<string>>(); }
	}
}
