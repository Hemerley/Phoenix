using Phoenix.Common.Commands.Factory;
using System.Collections.Generic;

namespace Phoenix.Common.Commands.Request
{
    public class NewAccountCommand : Command
    {
        #region -- Properties --

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        #endregion

        public NewAccountCommand()
        {
            this.CommandType = CommandType.NewAccount;
        }

        public override IEnumerable<IEnumerable<string>> GetCommandParts()
        {
            return new List<List<string>>
            {
                new List<string>
                {
                    this.Username,
                    this.Password,
                    this.Email
                }
            };
        }
    }
}
