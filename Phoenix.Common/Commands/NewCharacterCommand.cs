using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Common
{
    public class NewCharacterCommand : Command
    {
        #region -- Properties --

        public string CharacterName { get; set; }

        public string Gender { get; set; }

        public int Philosophy { get; set; }

        #endregion

        public NewCharacterCommand()
        {
            this.CommandType = CommandType.NewCharacter;
        }

        public override IEnumerable<string> GetCommandParts()
        {
            return new List<string>
            {
                this.CharacterName,
                this.Gender,
                this.Philosophy.ToString()
            };
        }
    }
}
