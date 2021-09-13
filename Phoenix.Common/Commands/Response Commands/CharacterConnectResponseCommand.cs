using Phoenix.Common;
using System.Collections.Generic;


namespace Phoenix.Common
{
    public class CharacterConnectResponseCommand : Command
    {
        public int Success { get; set; }
        public Data.Types.Character Character { get; set; }

        public CharacterConnectResponseCommand()
        {
            this.CommandType = CommandType.CharacterLoginResponse;
        }

        public override IEnumerable<string> GetCommandParts()
        {
            return new List<string> {
                this.Success.ToString(),
                this.Character.ToString()
            };
        }
    }
}
