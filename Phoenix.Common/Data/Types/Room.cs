using System.Collections.Generic;

namespace Phoenix.Common.Data.Types
{
    public class Room
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Area { get; set; }
        public int Status { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }
        public string Exits { get; set; }
        public int Tile { get; set; }
        public int North { get; set; }
        public int South { get; set; }
        public int West { get; set; }
        public int East { get; set; }
        public int Up { get; set; }
        public int Down { get; set; }
        public int KeyModeNorth { get; set; }
        public int KeyModeSouth { get; set; }
        public int KeyModeWest { get; set; }
        public int KeyModeEast { get; set; }
        public int KeyModeUp { get; set; }
        public int KeyModeDown { get; set; }
        public string KeyNameNorth { get; set; }
        public string KeyNameSouth { get; set; }
        public string KeyNameWest { get; set; }
        public string KeyNameEast { get; set; }
        public string KeyNameUp { get; set; }
        public string KeyNameDown { get; set; }
        public int KeyTypeNorth { get; set; }
        public int KeyTypeSouth { get; set; }
        public int KeyTypeWest { get; set; }
        public int KeyTypeEast { get; set; }
        public int KeyTypeUp { get; set; }
        public int KeyTypeDown { get; set; }
        public string KeyPassNorth { get; set; }
        public string KeyPassSouth { get; set; }
        public string KeyPassWest { get; set; }
        public string KeyPassEast { get; set; }
        public string KeyPassUp { get; set; }
        public string KeyPassDown { get; set; }
        public string KeyFailNorth { get; set; }
        public string KeyFailSouth { get; set; }
        public string KeyFailWest { get; set; }
        public string KeyFailEast { get; set; }
        public string KeyFailUp { get; set; }
        public string KeyFailDown { get; set; }
        public string Script { get; set; }
        public List<Entity> Entities { get; set; } = new();
        public List<Character> RoomCharacters { get; set; } = new();
        public List<Entity> RoomEntities { get; set; } = new();
        public List<Item> RoomItems { get; set; } = new();
        public bool CanGoNorth { get; set; }
        public bool CanGoSouth { get; set; }
        public bool CanGoWest { get; set; }
        public bool CanGoEast { get; set; }
        public bool CanGoUp { get; set; }
        public bool CanGoDown { get; set; }
        public double TimeStamp { get; set; }
    }
}
