using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Common
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

        public override IEnumerable<string> GetCommandParts()
        {
            return new List<string>
            {
                this.Username,
                this.Password,
                this.Email
            };
        }
    }
}
