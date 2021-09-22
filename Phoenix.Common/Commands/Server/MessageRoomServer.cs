using Phoenix.Common.Commands.Factory;
using Phoenix.Common.Data.Types;
using System.Collections.Generic;

namespace Phoenix.Common.Commands.Server
{
    public class MessageRoomServer : Command
    {
        #region -- Properties --

        public Character Character { get; set; } = null;
        public string Message { get; set; }

        #endregion

        public MessageRoomServer()
        {
            this.CommandType = CommandType.MessageRoom;
        }

        public override IEnumerable<IEnumerable<string>> GetCommandParts()
        {
            if (this.Character != null)
                return new List<List<string>>
                {
                    new List<string>
                    {
                        this.Message,
                        this.Character.Name
                    }
                };
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
