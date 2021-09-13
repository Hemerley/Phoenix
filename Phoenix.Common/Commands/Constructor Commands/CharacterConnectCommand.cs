using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Common
{
    public class CharacterConnectCommand : Command
    {

        public string Name { get; set; }

        public CharacterConnectCommand()
        {
            this.CommandType = CommandType.CharacterLogin;
        }

		public override IEnumerable<IEnumerable<string>> GetCommandParts()
		{
			return new List<List<string>>
			{
				new List<string>
				{
					this.Name
				}
			};
		}

	}
}
