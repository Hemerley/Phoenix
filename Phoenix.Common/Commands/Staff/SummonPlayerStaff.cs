using Phoenix.Common.Commands.Factory;
using System.Collections.Generic;

namespace Phoenix.Common.Commands.Staff
{
    public class SummonPlayerStaff : Command
    {

        public int Type { get; set; }
        public string Name { get; set; }

        public SummonPlayerStaff()
        {
            this.CommandType = CommandType.SummonPlayer;
        }

		public override IEnumerable<IEnumerable<string>> GetCommandParts()
		{
			return new List<List<string>>
			{
				new List<string>
				{
					this.Type.ToString(),
					this.Name
				}
			};
		}
	}
}
