using Phoenix.Common.Commands.Factory;
using System.Collections.Generic;

namespace Phoenix.Common.Commands.Request
{
    public class ClientRoomRequest : Command
    {

        #region -- Properties --

        public int RoomID { get; set; }

        #endregion

        public ClientRoomRequest()
        {
            this.CommandType = CommandType.ClientRoom;
        }
        public override IEnumerable<IEnumerable<string>> GetCommandParts()
        {

            return new List<List<string>>
            {
                new List<string>
                {
                    this.RoomID.ToString()
                }
            };
        }
    }
}
