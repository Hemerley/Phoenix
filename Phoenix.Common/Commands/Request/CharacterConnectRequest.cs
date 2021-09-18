using Phoenix.Common.Commands.Factory;
using System.Collections.Generic;

namespace Phoenix.Common.Commands.Request
{
    public class CharacterConnectRequest : Command
    {

        public string Name { get; set; }

        public CharacterConnectRequest()
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
