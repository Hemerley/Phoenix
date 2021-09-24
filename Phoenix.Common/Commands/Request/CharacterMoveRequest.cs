using Phoenix.Common.Commands.Factory;
using System.Collections.Generic;

namespace Phoenix.Common.Commands.Request
{
    public class CharacterMoveRequest : Command
    {
        public string Direction { get; set; }

        public CharacterMoveRequest()
        {
            this.CommandType = CommandType.CharacterMoveRequest;
        }

        public override IEnumerable<IEnumerable<string>> GetCommandParts()
        {
            return new List<List<string>>()
            {
                new List<string>()
                {
                    this.Direction
                }
            };
        }

    }
}
