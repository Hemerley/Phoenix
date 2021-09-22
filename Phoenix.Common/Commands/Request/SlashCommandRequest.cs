using Phoenix.Common.Commands.Factory;
using System.Collections.Generic;


namespace Phoenix.Common.Commands.Request
{
    public class SlashCommandRequest : Command
    {
        #region -- Properties --

        public string Message { get; set; }

        #endregion

        public SlashCommandRequest()
        {
            this.CommandType = CommandType.SlashCommand;
        }

        public override IEnumerable<IEnumerable<string>> GetCommandParts()
        {
            return new List<List<string>>
            {
                new List<string>
                {
                    this.Message
                }
            };
        }
    }
}
