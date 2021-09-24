using Phoenix.Common.Commands.Factory;
using System.Collections.Generic;

namespace Phoenix.Common.Commands.Server
{
    public class RespawnCharacterServer : Command
    {
        #region -- Properties --

        public string RoomID { get; set; }
        public string EntityID { get; set; }
        public string ArrivalMessage { get; set; }
        public string DepartureMessage { get; set; }

        #endregion

        public RespawnCharacterServer()
        {
            this.CommandType = CommandType.RespawnCharacter;
        }

        public override IEnumerable<IEnumerable<string>> GetCommandParts()
        {
            return new List<List<string>>
            {
                new List<string>
                {
                    this.RoomID,
                    this.EntityID,
                    this.ArrivalMessage,
                    this.DepartureMessage
                }
            };
        }
    }
}

