using Phoenix.Common.Commands.Factory;
using System.Collections.Generic;


namespace Phoenix.Common.Commands.Request
{
    public class NewCharacterCommand : Command
    {
        #region -- Properties --

        public string CharacterName { get; set; }

        public string Gender { get; set; }

        public int Philosophy { get; set; }

        public string Image { get; set; }

        #endregion

        public NewCharacterCommand()
        {
            this.CommandType = CommandType.NewCharacter;
        }

        public override IEnumerable<IEnumerable<string>> GetCommandParts()
        {
            return new List<List<string>>
            {
                new List<string>
                {
                    this.CharacterName,
                    this.Gender,
                    this.Philosophy.ToString(),
                    this.Image.ToString()
                }
            };
        }
    }
}
