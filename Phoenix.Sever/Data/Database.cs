using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Data.SQLite;
using System.IO;
using System;

namespace Phoenix.Server
{
    public class Database
    {

        /// <summary>
        /// Declare Game Objects.
        /// </summary>

        #region -- Abilities --
  
        #endregion

        #region -- Accounts --
        #endregion

        #region -- Helpers --

        /// <summary>
        /// Returns Connection String
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ConnectionString</returns>
        public static string LoadConnectionString(string id = "Live")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

        /// <summary>
        /// Intialize Database
        /// </summary>
        public static void InitializeDatabse()
        {
            CheckDatabase("LiveDB.db", "Live");
            CheckDatabase("TestDB.db","Test");   
        }
        
        /// <summary>
        /// Run Query
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="file"></param>
        private static void ExecuteQuery(SQLiteConnection connection, string file)
        {
            using var command = new SQLiteCommand(connection);
            command.CommandText = File.ReadAllText(file);
            command.ExecuteNonQuery();
            Logger.ConsoleLog("Database", $"Executed query from {file}.");
        }
        
        /// <summary>
       /// Checks Database, Creates if not there.
       /// </summary>
        private static void CheckDatabase(string database, string connectionType)
        {
            // Create TestDB if it doesn't exist.
            if (!File.Exists($"./{database}"))
            {
                SQLiteConnection.CreateFile(database);

                // Log Database Creation
                Logger.ConsoleLog("Error", $"{database} could not be located. Created new {database}.");

                using var connection = new SQLiteConnection(LoadConnectionString(connectionType));
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    foreach (var file in Directory.EnumerateFiles("./SQL/", "*.sql"))
                    {
                        ExecuteQuery(connection, file);
                    }
                }
                connection.Close();
            }
        }

        #endregion
        #region -- Dynamic Loaders --

        #endregion

        #region -- Rooms --

        /// <summary>
        /// Travis' Magic LINQ.
        /// </summary>
        /// <param name="connectionType"></param>
        /// <returns></returns>
        public static List<Room> LoadRooms(string connectionType)
        {
            using var connection = new SQLiteConnection(LoadConnectionString(connectionType));
            connection.Open();
            string query = $"SELECT r.ID as RoomID, r.Name as RoomName, r.Area as RoomArea, r.Status as RoomStatus, r.Type as RoomType, r.Description as RoomDescription, r.Exits as RoomExits, r.Tile as RoomTile, r.North as RoomNorth, r.South as RoomSouth, r.West as RoomWest, r.East as RoomEast, r.Up as RoomUp, r.Down as RoomDown, r.KeyModeNorth as RoomKeyModeNorth, r.KeyModeSouth as RoomKeyModeSouth, r.KeyModeWest as RoomKeyModeWest, r.KeyModeEast as RoomKeyModeEast, r.KeyModeUp as RoomKeyModeUp, r.KeyModeDown as RoomKeyModeDown, r.KeyNameNorth as RoomKeyNameNorth, r.KeyNameSouth as RoomKeyNameSouth, r.KeyNameWest as RoomKeyNameWest, r.KeyNameEast as RoomKeyNameEast, r.KeyNameUp as RoomKeyNameUp, r.KeyNameDown as RoomKeyNameDown, r.KeyTypeNorth as RoomKeyTypeNorth, r.KeyTypeSouth as RoomKeyTypeSouth, r.KeyTypeWest as RoomKeyTypeWest, r.KeyTypeEast as RoomKeyTypeEast, r.KeyTypeUp as RoomKeyTypeUp, r.KeyTypeDown as RoomKeyTypeDown, r.KeyPassNorth as RoomKeyPassNorth, r.KeyPassSouth as RoomKeyPassSouth, r.KeyPassWest as RoomKeyPassWest, r.KeyPassEast as RoomKeyPassEast, r.KeyPassUp as RoomKeyPassUp, r.KeyPassDown as RoomKeyPassDown, r.KeyFailNorth as RoomKeyFailNorth, r.KeyFailSouth as RoomKeyFailSouth, r.KeyFailWest as RoomKeyFailWest, r.KeyFailEast as RoomKeyFailEast, r.KeyFailUp as RoomKeyFailUp, r.KeyFailDown as RoomKeyFailDown, r.Script as RoomScript, e.ID as EntityID, e.Type as EntityType, e.Rarity as EntityRarity, e.Name as EntityName, e.Image as EntityImage, e.HisHer as EntityHisHer, e.HeShe as EntityHeShe, e.BName as EntityBName, e.Level as EntityLevel, e.Gold as EntityGold, e.Strength as EntityStrength, e.Agility as EntityAgility, e.Intellect as EntityIntellect, e.Stamina as EntityStamina, e.Damage as EntityDamage, e.Haste as EntityHaste, e.Crit as EntityCrit, e.Mastery as EntityMastery, e.Versatility as EntityVersatility, e.Health as EntityHealth, e.Mana as EntityMana, e.Taunt as EntityTaunt, e.SpawnTime as EntitySpawnTime, e.SpawnDelay as EntitySpawnDelay, e.VanishTime as EntityVanishTime, e.Script as EntityScript FROM Rooms r LEFT OUTER JOIN RoomEntities re ON re.RoomID = r.ID LEFT OUTER JOIN Entities e ON e.ID = re.EntityID; ";
            using var command = new SQLiteCommand(query, connection);
            using SQLiteDataReader reader = command.ExecuteReader();
            List<RoomEntityDto> rawData = new();
            while (reader.Read())
            {
                var roomEntityDto = new RoomEntityDto
                {
                    RoomID = int.TryParse(reader["RoomID"]?.ToString(), out int roomID) ? roomID : (int?)null,
                    RoomName = reader["RoomName"].ToString(),
                    RoomArea = reader["RoomArea"].ToString(),
                    RoomStatus = int.TryParse(reader["RoomStatus"]?.ToString(), out int roomStatus) ? roomStatus : (int?)null,
                    RoomType = int.TryParse(reader["RoomType"]?.ToString(), out int roomType) ? roomType : (int?)null,
                    RoomDescription = reader["RoomDescription"].ToString(),
                    RoomExits = reader["RoomExits"].ToString(),
                    RoomTile = int.TryParse(reader["RoomTile"]?.ToString(), out int roomTile) ? roomTile : (int?)null,
                    RoomNorth = int.TryParse(reader["RoomNorth"]?.ToString(), out int roomNorth) ? roomNorth : (int?)null,
                    RoomSouth = int.TryParse(reader["RoomSouth"]?.ToString(), out int roomSouth) ? roomSouth : (int?)null,
                    RoomWest = int.TryParse(reader["RoomWest"]?.ToString(), out int roomWest) ? roomWest : (int?)null,
                    RoomEast = int.TryParse(reader["RoomEast"]?.ToString(), out int roomEast) ? roomEast : (int?)null,
                    RoomUp = int.TryParse(reader["RoomUp"]?.ToString(), out int roomUp) ? roomUp : (int?)null,
                    RoomDown = int.TryParse(reader["RoomDown"]?.ToString(), out int roomDown) ? roomDown : (int?)null,
                    RoomKeyModeNorth = int.TryParse(reader["RoomKeyModeNorth"]?.ToString(), out int roomKeyModeNorth) ? roomKeyModeNorth : (int?)null,
                    RoomKeyModeSouth = int.TryParse(reader["RoomKeyModeSouth"]?.ToString(), out int roomKeyModeSouth) ? roomKeyModeSouth : (int?)null,
                    RoomKeyModeWest = int.TryParse(reader["RoomKeyModeWest"]?.ToString(), out int roomKeyModeWest) ? roomKeyModeWest : (int?)null,
                    RoomKeyModeEast = int.TryParse(reader["RoomKeyModeEast"]?.ToString(), out int roomKeyModeEast) ? roomKeyModeEast : (int?)null,
                    RoomKeyModeUp = int.TryParse(reader["RoomKeyModeUp"]?.ToString(), out int roomKeyModeUp) ? roomKeyModeUp : (int?)null,
                    RoomKeyModeDown = int.TryParse(reader["RoomKeyModeDown"]?.ToString(), out int roomKeyModeDown) ? roomKeyModeDown : (int?)null,
                    RoomKeyNameNorth = reader["RoomKeyNameNorth"].ToString(),
                    RoomKeyNameSouth = reader["RoomKeyNameSouth"].ToString(),
                    RoomKeyNameWest = reader["RoomKeyNameWest"].ToString(),
                    RoomKeyNameEast = reader["RoomKeyNameEast"].ToString(),
                    RoomKeyNameUp = reader["RoomKeyNameUp"].ToString(),
                    RoomKeyNameDown = reader["RoomKeyNameDown"].ToString(),
                    RoomKeyTypeNorth = int.TryParse(reader["RoomKeyTypeNorth"]?.ToString(), out int roomKeyTypeNorth) ? roomKeyTypeNorth : (int?)null,
                    RoomKeyTypeSouth = int.TryParse(reader["RoomKeyTypeSouth"]?.ToString(), out int roomKeyTypeSouth) ? roomKeyTypeSouth : (int?)null,
                    RoomKeyTypeWest = int.TryParse(reader["RoomKeyTypeWest"]?.ToString(), out int roomKeyTypeWest) ? roomKeyTypeWest : (int?)null,
                    RoomKeyTypeEast = int.TryParse(reader["RoomKeyTypeEast"]?.ToString(), out int roomKeyTypeEast) ? roomKeyTypeEast : (int?)null,
                    RoomKeyTypeUp = int.TryParse(reader["RoomKeyTypeUp"]?.ToString(), out int roomKeyTypeUp) ? roomKeyTypeUp : (int?)null,
                    RoomKeyTypeDown = int.TryParse(reader["RoomKeyTypeDown"]?.ToString(), out int roomKeyTypeDown) ? roomKeyTypeDown : (int?)null,
                    RoomKeyPassNorth = reader["RoomKeyPassNorth"].ToString(),
                    RoomKeyPassSouth = reader["RoomKeyPassSouth"].ToString(),
                    RoomKeyPassWest = reader["RoomKeyPassWest"].ToString(),
                    RoomKeyPassEast = reader["RoomKeyPassEast"].ToString(),
                    RoomKeyPassUp = reader["RoomKeyPassUp"].ToString(),
                    RoomKeyPassDown = reader["RoomKeyPassDown"].ToString(),
                    RoomKeyFailNorth = reader["RoomKeyFailNorth"].ToString(),
                    RoomKeyFailSouth = reader["RoomKeyFailSouth"].ToString(),
                    RoomKeyFailWest = reader["RoomKeyFailWest"].ToString(),
                    RoomKeyFailEast = reader["RoomKeyFailEast"].ToString(),
                    RoomKeyFailUp = reader["RoomKeyFailUp"].ToString(),
                    RoomKeyFailDown = reader["RoomKeyFailDown"].ToString(),
                    RoomScript = reader["RoomScript"].ToString(),
                    EntityID = int.TryParse(reader["EntityID"]?.ToString(), out int entityID) ? entityID : (int?)null,
                    EntityType = int.TryParse(reader["EntityType"]?.ToString(), out int entityType) ? entityType : (int?)null,
                    EntityRarity = int.TryParse(reader["EntityRarity"]?.ToString(), out int entityRarity) ? entityRarity : (int?)null,
                    EntityName = reader["EntityName"].ToString(),
                    EntityImage = int.TryParse(reader["EntityImage"]?.ToString(), out int entityImage) ? entityImage : (int?)null,
                    EntityHisHer = reader["EntityHisHer"].ToString(),
                    EntityHeShe = reader["EntityHeShe"].ToString(),
                    EntityBName = reader["EntityBName"].ToString(),
                    EntityLevel = int.TryParse(reader["EntityLevel"]?.ToString(), out int entityLevel) ? entityLevel : (int?)null,
                    EntityGold = int.TryParse(reader["EntityGold"]?.ToString(), out int entityGold) ? entityGold : (int?)null,
                    EntityStrength = int.TryParse(reader["EntityStrength"]?.ToString(), out int entityStrength) ? entityStrength : (int?)null,
                    EntityAgility = int.TryParse(reader["EntityAgility"]?.ToString(), out int entityAgility) ? entityAgility : (int?)null,
                    EntityIntellect = int.TryParse(reader["EntityIntellect"]?.ToString(), out int entityIntellect) ? entityIntellect : (int?)null,
                    EntityStamina = int.TryParse(reader["EntityStamina"]?.ToString(), out int entityStamina) ? entityStamina : (int?)null,
                    EntityDamage = int.TryParse(reader["EntityDamage"]?.ToString(), out int entityDamage) ? entityDamage : (int?)null,
                    EntityHaste = double.TryParse(reader["EntityDamage"]?.ToString(), out double entityHaste) ? entityHaste : (double?)null,
                    EntityCrit = double.TryParse(reader["EntityDamage"]?.ToString(), out double entityCrit) ? entityCrit : (double?)null,
                    EntityMastery = double.TryParse(reader["EntityDamage"]?.ToString(), out double entityMastery) ? entityMastery : (double?)null,
                    EntityVersatility = double.TryParse(reader["EntityDamage"]?.ToString(), out double entityVersatility) ? entityVersatility : (double?)null,
                    EntityHealth = int.TryParse(reader["EntityHealth"]?.ToString(), out int entityHealth) ? entityHealth : (int?)null,
                    EntityMana = int.TryParse(reader["EntityMana"]?.ToString(), out int entityMana) ? entityMana : (int?)null,
                    EntityTaunt = int.TryParse(reader["EntityTaunt"]?.ToString(), out int entityTaunt) ? entityTaunt : (int?)null,
                    EntitySpawnTime = int.TryParse(reader["EntitySpawnTime"]?.ToString(), out int entitySpawnTime) ? entitySpawnTime : (int?)null,
                    EntitySpawnDelay = int.TryParse(reader["EntitySpawnDelay"]?.ToString(), out int entitySpawnDelay) ? entitySpawnDelay : (int?)null,
                    EntityVanishTime = int.TryParse(reader["EntityVanishTime"]?.ToString(), out int entityVanishTime) ? entityVanishTime : (int?)null,
                    EntityScript = reader["EntityScript"].ToString()
                };
                rawData.Add(roomEntityDto);
            }
            return (from data in rawData
                    group data by new { data.RoomID, data.RoomName, data.RoomArea, data.RoomStatus, data.RoomType, data.RoomDescription, data.RoomExits, data.RoomTile, data.RoomNorth, data.RoomSouth, data.RoomWest, data.RoomEast, data.RoomUp, data.RoomDown, data.RoomKeyModeNorth, data.RoomKeyModeSouth, data.RoomKeyModeWest, data.RoomKeyModeEast, data.RoomKeyModeUp, data.RoomKeyModeDown, data.RoomKeyNameNorth, data.RoomKeyNameSouth, data.RoomKeyNameWest, data.RoomKeyNameEast, data.RoomKeyNameUp, data.RoomKeyNameDown, data.RoomKeyTypeNorth, data.RoomKeyTypeSouth, data.RoomKeyTypeWest, data.RoomKeyTypeEast, data.RoomKeyTypeUp, data.RoomKeyTypeDown, data.RoomKeyPassNorth, data.RoomKeyPassSouth, data.RoomKeyPassWest, data.RoomKeyPassEast, data.RoomKeyPassUp, data.RoomKeyPassDown, data.RoomKeyFailNorth, data.RoomKeyFailSouth, data.RoomKeyFailWest, data.RoomKeyFailEast, data.RoomKeyFailUp, data.RoomKeyFailDown, data.RoomScript } into g
                    select new Room
                    {
                        ID = g.Key.RoomID,
                        Name = g.Key.RoomName,
                        Area = g.Key.RoomArea,
                        Status = g.Key.RoomStatus,
                        Type = g.Key.RoomType,
                        Description = g.Key.RoomDescription,
                        Exits = g.Key.RoomExits,
                        Tile = g.Key.RoomTile,
                        North = g.Key.RoomNorth,
                        South = g.Key.RoomSouth,
                        West = g.Key.RoomWest,
                        East = g.Key.RoomEast,
                        Up = g.Key.RoomUp,
                        Down = g.Key.RoomDown,
                        KeyModeNorth = g.Key.RoomKeyModeNorth,
                        KeyModeSouth = g.Key.RoomKeyModeSouth,
                        KeyModeWest = g.Key.RoomKeyModeWest,
                        KeyModeEast = g.Key.RoomKeyModeEast,
                        KeyModeUp = g.Key.RoomKeyModeUp,
                        KeyModeDown = g.Key.RoomKeyModeDown,
                        KeyNameNorth = g.Key.RoomKeyNameNorth,
                        KeyNameSouth = g.Key.RoomKeyNameSouth,
                        KeyNameWest = g.Key.RoomKeyNameWest,
                        KeyNameEast = g.Key.RoomKeyNameEast,
                        KeyNameUp = g.Key.RoomKeyNameUp,
                        KeyNameDown = g.Key.RoomKeyNameDown,
                        KeyTypeNorth = g.Key.RoomKeyTypeNorth,
                        KeyTypeSouth = g.Key.RoomKeyTypeSouth,
                        KeyTypeWest = g.Key.RoomKeyTypeWest,
                        KeyTypeEast = g.Key.RoomKeyTypeEast,
                        KeyTypeUp = g.Key.RoomKeyTypeUp,
                        KeyTypeDown = g.Key.RoomKeyTypeDown,
                        KeyPassNorth = g.Key.RoomKeyPassNorth,
                        KeyPassSouth = g.Key.RoomKeyPassSouth,
                        KeyPassWest = g.Key.RoomKeyPassWest,
                        KeyPassEast = g.Key.RoomKeyPassEast,
                        KeyPassUp = g.Key.RoomKeyPassUp,
                        KeyPassDown = g.Key.RoomKeyPassDown,
                        KeyFailNorth = g.Key.RoomKeyFailNorth,
                        KeyFailSouth = g.Key.RoomKeyFailSouth,
                        KeyFailWest = g.Key.RoomKeyFailWest,
                        KeyFailEast = g.Key.RoomKeyFailEast,
                        KeyFailUp = g.Key.RoomKeyFailUp,
                        KeyFailDown = g.Key.RoomKeyFailDown,
                        Script = g.Key.RoomScript,
                        Entities = g.Where(e => e.EntityID.HasValue).ToList().Select(e => new Entity
                        {
                            ID = e.EntityID.Value,
                            Type = e.EntityType.Value,
                            Rarity = e.EntityRarity.Value,
                            Name = e.EntityName,
                            Image = e.EntityImage.Value,
                            HisHer = e.EntityHisHer,
                            HeShe = e.EntityHeShe,
                            BName = e.EntityBName,
                            Level = e.EntityLevel.Value,
                            Gold = e.EntityGold.Value,
                            Strength = e.EntityStrength.Value,
                            Agility = e.EntityAgility.Value,
                            Intellect = e.EntityIntellect.Value,
                            Stamina = e.EntityStamina.Value,
                            Damage = e.EntityDamage.Value,
                            Haste = e.EntityHaste.Value,
                            Crit = e.EntityCrit.Value,
                            Mastery = e.EntityMastery.Value,
                            Versatility = e.EntityVersatility.Value,
                            Health = e.EntityHealth.Value,
                            Mana = e.EntityMana.Value,
                            Taunt = e.EntityTaunt.Value,
                            SpawnTime = e.EntitySpawnTime.Value,
                            SpawnDelay = e.EntitySpawnDelay.Value,
                            VanishTime = e.EntityVanishTime.Value,
                            Script = e.EntityScript,
                        }).ToList()
                    }).ToList();
        }

        #endregion

        #region -- Accounts --

        /// <summary>
        /// Returns An Account Field From Database.
        /// </summary>
        /// <param name="connectionType"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetAccountField(string connectionType, string column, string field, string value)
        {
            using SQLiteConnection connection = new(LoadConnectionString(connectionType));
            connection.Open();
            string query = $"SELECT {column} FROM Accounts WHERE {field} = '{value}';";
            using var command = new SQLiteCommand(query, connection);
            using SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                return reader[0].ToString();
            }
            return null;
        }
        
        /// <summary>
        /// Inserts A New Account
        /// </summary>
        /// <param name="connectionType"></param>
        /// <param name="AccountName"></param>
        /// <param name="Password"></param>
        /// <param name="Email"></param>
        public static void InsertNewAccount(string connectionType, string AccountName, string Password, string Email)
        {
            using var connection = new SQLiteConnection(LoadConnectionString(connectionType));
            connection.Open();
            string query = $"INSERT INTO Accounts (Name, Password, Email, Gold) VALUES ('{AccountName}', '{Password}', '{Email}', 0); ";
            using var command = new SQLiteCommand(query, connection);
            command.ExecuteNonQuery();
        }

        #endregion

        #region -- Characters --

        /// <summary>
        /// Returns An Account Field From Database.
        /// </summary>
        /// <param name="connectionType"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetCharacterField(string connectionType, string column, string field, string value)
        {
            using var connection = new SQLiteConnection(LoadConnectionString(connectionType));
            connection.Open();
            string query = $"SELECT {column} FROM Characters WHERE {field} = '{value}';";
            using var command = new SQLiteCommand(query, connection);
            using SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                return reader[0].ToString();
            }
            return null;
        }

        /// <summary>
        /// Inserts A New Account
        /// </summary>
        /// <param name="connectionType"></param>
        /// <param name="AccountName"></param>
        /// <param name="Password"></param>
        /// <param name="Email"></param>
        public static void InsertNewCharacter(string connectionType, string CharacterName, string Gender, int Philosophy, int Image, int accountID)
        {
            using var connection = new SQLiteConnection(LoadConnectionString(connectionType));
            connection.Open();
            var heShe = "";
            var hisHer = "";
            if (Gender == "Male")
            {
                heShe = "He";
                hisHer = "His";
            }
            string query = $"INSERT INTO Characters (AccountID, Name, Image, Type, Gender, HisHer, HeShe, Experience, Caste, Rank, Philosophy, Alignment, Creation, Strength, Agility, Intellect, Stamina, Damage, Health, Mana, RoomID) VALUES ('{accountID}', '{CharacterName}', {Image}, 0, '{Gender}', '{hisHer}', '{heShe}', 0, 0, 0, {Philosophy}, 0, 0, 10, 10, 10, 10, 20, 20, 20, 0);";
            using var command = new SQLiteCommand(query, connection);
            command.ExecuteNonQuery();
        }

        public static List<Character> GetCharacterList (string connectionType, int accountID)
        {
            using var connection = new SQLiteConnection(LoadConnectionString(connectionType));
            connection.Open();
            string query = $"SELECT Name, Caste, Philosophy FROM Characters WHERE AccountID = '{accountID}';";
            using var command = new SQLiteCommand(query, connection);
            using SQLiteDataReader reader = command.ExecuteReader();
            List<Character> characters = new();

            while (reader.Read())
            {
                Character character = new()
                {
                    Name = reader[0].ToString(),
                    Caste = Int32.Parse(reader[1].ToString()),
                    Philosophy = Int32.Parse(reader[2].ToString()),
                };

                characters.Add(character);
            }
            return characters;
        }

        public static Character GetCharacter (string connectionType, int accountID, string name)
        {
            using var connection = new SQLiteConnection(LoadConnectionString(connectionType));
            connection.Open();
            string query = $"SELECT ID, AccountID, Name, Type, Image, Gender, HisHer, HeShe, Experience, Title, Caste, Rank, Philosophy, Alignment, Creation, Strength, Agility, Intellect, Stamina, Damage, Health, Mana, RoomID FROM Characters WHERE AccountID = '{accountID}' AND Name = '{name}';";
            using var command = new SQLiteCommand(query, connection);
            using SQLiteDataReader reader = command.ExecuteReader();

            Character character;

            while (reader.Read())
            {
                character = new()
                {
                    Id = Int32.Parse(reader[0].ToString()),
                    AccountId = Int32.Parse(reader[1].ToString()),
                    Name = reader[2].ToString(),
                    Type = Int32.Parse(reader[3].ToString()),
                    Image = Int32.Parse(reader[4].ToString()),
                    Gender = reader[5].ToString(),
                    HisHer = reader[6].ToString(),
                    HeShe = reader[7].ToString(),
                    Experience = Int32.Parse(reader[8].ToString()),
                    Title = reader[9].ToString(),
                    Caste = Int32.Parse(reader[10].ToString()),
                    Rank = Int32.Parse(reader[11].ToString()),
                    Philosophy = Int32.Parse(reader[12].ToString()),
                    Alignment = Int32.Parse(reader[13].ToString()),
                    Creation = Int32.Parse(reader[14].ToString()),
                    Strength = Int32.Parse(reader[15].ToString()),
                    Agility = Int32.Parse(reader[16].ToString()),
                    Intellect = Int32.Parse(reader[17].ToString()),
                    Stamina = Int32.Parse(reader[18].ToString()),
                    Damage = Int32.Parse(reader[19].ToString()),
                    Health = Int32.Parse(reader[20].ToString()),
                    Mana = Int32.Parse(reader[21].ToString()),
                    RoomID = Int32.Parse(reader[22].ToString()),
                    Crit = 0,
                    Mastery = 0,
                    Haste = 0,
                    Versatility = 0
                };
                return character;
            }
            return null;
        }

        #endregion
    }
}
