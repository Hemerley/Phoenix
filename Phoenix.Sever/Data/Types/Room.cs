using System.Collections.Generic;

namespace Phoenix.Server
{
    public class Room
    {
        public int? ID { get; set; }
        public string Name { get; set; }
        public string Area { get; set; }
        public int? Status { get; set; }
        public int? Type { get; set; }
        public string Description { get; set; }
        public string Exits { get; set; }
        public int? Tile { get; set; }
        public int? North { get; set; }
        public int? South { get; set; }
        public int? West { get; set; }
        public int? East { get; set; }
        public int? Up { get; set; }
        public int? Down { get; set; }
        public int? KeyModeNorth { get; set; }
        public int? KeyModeSouth { get; set; }
        public int? KeyModeWest { get; set; }
        public int? KeyModeEast { get; set; }
        public int? KeyModeUp { get; set; }
        public int? KeyModeDown { get; set; }
        public string KeyNameNorth { get; set; }
        public string KeyNameSouth { get; set; }
        public string KeyNameWest { get; set; }
        public string KeyNameEast { get; set; }
        public string KeyNameUp { get; set; }
        public string KeyNameDown { get; set; }
        public int? KeyTypeNorth { get; set; }
        public int? KeyTypeSouth { get; set; }
        public int? KeyTypeWest { get; set; }
        public int? KeyTypeEast { get; set; }
        public int? KeyTypeUp { get; set; }
        public int? KeyTypeDown { get; set; }
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

        public Room()
        {

        }

        public Room(int id, string name, string area, int status, int type, string description, string exits, int tile, int north, int south, int west, int east, int up, int down, int keymodenorth, int keymodesouth, int keymodewest, int keymodeeast, int keymodeup, int keymodedown, string keynamenorth, string keynamesouth, string keynamewest, string keynameeast, string keynameup, string keynamedown, int keytypenorth, int keytypesouth, int keytypewest, int keytypeeast, int keytypeup, int keytypedown, string keypassnorth, string keypasssouth, string keypasswest, string keypasseast, string keypassup, string keypassdown, string keyfailnorth, string keyfailsouth, string keyfailwest, string keyfaileast, string keyfailup, string keyfaildown, string script)
        {
            this.ID = id;
            this.Name = name;
            this.Area = area;
            this.Status = status;
            this.Type = type;
            this.Description = description;
            this.Exits = exits;
            this.Tile = tile;
            this.North = north;
            this.South = south;
            this.West = west;
            this.East = east;
            this.Down = down;
            this.Up = up;
            this.KeyModeNorth = keymodenorth;
            this.KeyModeSouth = keymodesouth;
            this.KeyModeWest = keymodewest;
            this.KeyModeEast = keymodeeast;
            this.KeyModeDown = keymodedown;
            this.KeyModeUp = keymodeup;
            this.KeyNameNorth = keynamenorth;
            this.KeyNameSouth = keynamesouth;
            this.KeyNameWest = keynamewest;
            this.KeyNameEast = keynameeast;
            this.KeyNameDown = keynamedown;
            this.KeyNameUp = keynameup;
            this.KeyTypeNorth = keytypenorth;
            this.KeyTypeSouth = keytypesouth;
            this.KeyTypeWest = keytypewest;
            this.KeyTypeEast = keytypeeast;
            this.KeyTypeDown = keytypedown;
            this.KeyTypeUp = keytypeup;
            this.KeyPassNorth = keypassnorth;
            this.KeyPassSouth = keypasssouth;
            this.KeyPassWest = keypasswest;
            this.KeyPassEast = keypasseast;
            this.KeyPassDown = keypassdown;
            this.KeyPassUp = keypassup;
            this.KeyFailNorth = keyfailnorth;
            this.KeyFailSouth = keyfailsouth;
            this.KeyFailWest = keyfailwest;
            this.KeyFailEast = keyfaileast;
            this.KeyFailDown = keyfaildown;
            this.KeyFailUp = keyfailup;
            this.Script = script;
        }
    }
}
