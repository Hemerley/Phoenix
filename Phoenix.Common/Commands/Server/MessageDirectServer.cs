using Phoenix.Common.Commands.Factory;
using System.Collections.Generic;

namespace Phoenix.Common.Commands.Server
{
    public class MessageDirectServer : Command
    {

        public string SendingName { get; set; }
        public string ReceivingName { get; set; }
        public string Message { get; set; }

        public MessageDirectServer()
        {
            this.CommandType = CommandType.MessageDirect;
        }

        public override IEnumerable<IEnumerable<string>> GetCommandParts()
        {
            return new List<List<string>>
            {
                new List<string>
                {
                    this.SendingName,
                    this.ReceivingName,
                    this.Message
                }
            };
        }
    }
}
