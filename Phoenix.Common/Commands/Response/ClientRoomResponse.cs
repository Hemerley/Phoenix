using Phoenix.Common.Commands.Factory;
using Phoenix.Common.Data.Types;
using System.Collections.Generic;

namespace Phoenix.Common.Commands.Response
{
    public class ClientRoomResponse : Command
    {

        #region -- Properties --

        public bool Success { get; set; }
        public Room Room { get; set; } = new();

        public ClientRoomResponse()
        {
            this.CommandType = CommandType.ClientRoomResponse;
        }
        public override IEnumerable<IEnumerable<string>> GetCommandParts()
        {

            var response = new List<List<string>>
            {
                new List<string>
            {
                this.Success.ToString()
            },

                new List<string>
            {
                this.Room.Name,
                this.Room.Description,
                this.Room.Exits,
                this.Room.Type.ToString()

            }
            };

            foreach (Character character in Room.RoomCharacters)
            {
                response.Add(new List<string>
                {
                    "Character",
                    character.Name,
                    character.Image,
                    character.Type,
                });
            }

            foreach (NPC NPC in Room.RoomNPC)
            {
                response.Add(new List<string>
                {
                    "NPC",
                    NPC.Name,
                    NPC.Image,
                    NPC.Type
                });
            }

            foreach (Item item in Room.RoomItems)
            {
                response.Add(new List<string>
                {
                    "Item",
                    item.Name,
                    item.Image,
                    item.Rarity,
                    item.Amount.ToString()
                });
            }

            return response;
        }

        #endregion

    }
}
