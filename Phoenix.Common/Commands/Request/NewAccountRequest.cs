using Phoenix.Common.Commands.Factory;
using System;
using System.Collections.Generic;

namespace Phoenix.Common.Commands.Request
{
    public class NewAccountRequest : Command
    {
        #region -- Properties --
        public Version Version { get; set; }
        
        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        #endregion

        public NewAccountRequest()
        {
            this.CommandType = CommandType.NewAccount;
        }

        public override IEnumerable<IEnumerable<string>> GetCommandParts()
        {
            return new List<List<string>>
            {
                new List<string>
                {
                    this.Version.ToString(),
                    this.Username,
                    this.Password,
                    this.Email
                }
            };
        }
    }
}
