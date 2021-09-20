using Phoenix.Common.Data;
using Phoenix.Common.Data.Types;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Data.SQLite;
using System.IO;
using System;
using Serilog;

namespace Phoenix.Server.Data
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
            Log.Information($"Executed query from {file}.");
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
                Log.Error($"{database} could not be located. Created new {database}.");

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
                    RoomID = Int32.Parse(reader["RoomID"]?.ToString()),
                    RoomName = reader["RoomName"].ToString(),
                    RoomArea = reader["RoomArea"].ToString(),
                    RoomStatus = Int32.Parse(reader["RoomStatus"]?.ToString()), 
                    RoomType = Int32.Parse(reader["RoomType"]?.ToString()),
                    RoomDescription = reader["RoomDescription"].ToString(),
                    RoomExits = reader["RoomExits"].ToString(),
                    RoomTile = Int32.Parse(reader["RoomTile"]?.ToString()),
                    RoomNorth = Int32.Parse(reader["RoomNorth"]?.ToString()),
                    RoomSouth = Int32.Parse(reader["RoomSouth"]?.ToString()),
                    RoomWest = Int32.Parse(reader["RoomWest"]?.ToString()),
                    RoomEast = Int32.Parse(reader["RoomEast"]?.ToString()),
                    RoomUp = Int32.Parse(reader["RoomUp"]?.ToString()),
                    RoomDown = Int32.Parse(reader["RoomDown"]?.ToString()),
                    RoomKeyModeNorth = Int32.Parse(reader["RoomKeyModeNorth"]?.ToString()), 
                    RoomKeyModeSouth = Int32.Parse(reader["RoomKeyModeSouth"]?.ToString()),
                    RoomKeyModeWest = Int32.Parse(reader["RoomKeyModeWest"]?.ToString()),
                    RoomKeyModeEast = Int32.Parse(reader["RoomKeyModeEast"]?.ToString()),
                    RoomKeyModeUp = Int32.Parse(reader["RoomKeyModeUp"]?.ToString()),
                    RoomKeyModeDown = Int32.Parse(reader["RoomKeyModeDown"]?.ToString()),
                    RoomKeyNameNorth = reader["RoomKeyNameNorth"].ToString(),
                    RoomKeyNameSouth = reader["RoomKeyNameSouth"].ToString(),
                    RoomKeyNameWest = reader["RoomKeyNameWest"].ToString(),
                    RoomKeyNameEast = reader["RoomKeyNameEast"].ToString(),
                    RoomKeyNameUp = reader["RoomKeyNameUp"].ToString(),
                    RoomKeyNameDown = reader["RoomKeyNameDown"].ToString(),
                    RoomKeyTypeNorth = Int32.Parse(reader["RoomKeyTypeNorth"]?.ToString()),
                    RoomKeyTypeSouth = Int32.Parse(reader["RoomKeyTypeSouth"]?.ToString()),
                    RoomKeyTypeWest = Int32.Parse(reader["RoomKeyTypeWest"]?.ToString()),
                    RoomKeyTypeEast = Int32.Parse(reader["RoomKeyTypeEast"]?.ToString()),
                    RoomKeyTypeUp = Int32.Parse(reader["RoomKeyTypeUp"]?.ToString()),
                    RoomKeyTypeDown = Int32.Parse(reader["RoomKeyTypeDown"]?.ToString()),
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
                    EntityImage = reader["EntityImage"].ToString(),
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
                    group data by new { data.RoomID, data.RoomName, data.RoomArea, data.RoomStatus, data.RoomType, data.RoomDescription, data.RoomExits, data.RoomTile, data.RoomNorth, data.RoomSouth, data.RoomWest, data.RoomEast, data.RoomUp, data.RoomDown, data.RoomKeyModeNorth, data.RoomKeyModeSouth, data.RoomKeyModeWest, data.RoomKeyModeEast, data.RoomKeyModeUp, data.RoomKeyModeDown, data.RoomKeyNameNorth, data.RoomKeyNameSouth, data.RoomKeyNameWest, data.RoomKeyNameEast, data.RoomKeyNameUp, data.RoomKeyNameDown, data.RoomKeyTypeNorth, data.RoomKeyTypeSouth, data.RoomKeyTypeWest, data.RoomKeyTypeEast, data.RoomKeyTypeUp, data.RoomKeyTypeDown, data.RoomKeyPassNorth, data.RoomKeyPassSouth, data.RoomKeyPassWest, data.RoomKeyPassEast, data.RoomKeyPassUp, data.RoomKeyPassDown, data.RoomKeyFailNorth, data.RoomKeyFailSouth, data.RoomKeyFailWest, data.RoomKeyFailEast, data.RoomKeyFailUp, data.RoomKeyFailDown, data.RoomScript} into g
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
                        CanGoNorth = g.Key.RoomNorth is not -1,
                        CanGoSouth = g.Key.RoomSouth is not -1,
                        CanGoWest = g.Key.RoomWest is not -1,
                        CanGoEast = g.Key.RoomEast is not -1,
                        CanGoUp = g.Key.RoomUp is not -1,
                        CanGoDown = g.Key.RoomDown is not -1,
                        InstanceID = Guid.NewGuid(),
                        Entities = g.Where(e => e.EntityID.HasValue).ToList().Select(e => new Entity
                        {
                            ID = e.EntityID.Value,
                            Type = Helper.ReturnEntityTypeText(e.EntityType.Value),
                            TypeID = e.EntityType.Value,
                            Rarity = Helper.ReturnRarityText(e.EntityRarity.Value),
                            RarityID = e.EntityRarity.Value,
                            Name = e.EntityName,
                            Image = e.EntityImage,
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
                            InstanceID = Guid.NewGuid()
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
        public static void InsertNewCharacter(string connectionType, string CharacterName, string Gender, int Philosophy, string Image, int accountID)
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
            string query = $"INSERT INTO Characters (AccountID, Name, Image, Type, Gender, HisHer, HeShe, Experience, Title, Caste, Rank, Philosophy, Alignment, Creation, Strength, Agility, Intellect, Stamina, Damage, Health, Mana, RoomID) VALUES ('{accountID}', '{CharacterName}', '{Image}', 0, '{Gender}', '{hisHer}', '{heShe}', 0, 'Initiate', 0, 0, {Philosophy}, 0, 0, 10, 10, 10, 10, 20, 20, 20, 1);";
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
                    Name = reader[0].ToString().FirstCharToUpper(),
                    Caste = Helper.ReturnCasteText(Int32.Parse(reader[1].ToString())).FirstCharToUpper(),
                    Philosophy = Helper.ReturnPhilosophyText(Int32.Parse(reader[2].ToString())).FirstCharToUpper()
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
                    Name = reader[2].ToString().FirstCharToUpper(),
                    Type = Helper.ReturnCharacterTypeText(Int32.Parse(reader[3].ToString())),
                    TypeID = Int32.Parse(reader[3].ToString()),
                    Image = reader[4].ToString(),
                    Gender = reader[5].ToString(),
                    HisHer = reader[6].ToString(),
                    HeShe = reader[7].ToString(),
                    Experience = Int32.Parse(reader[8].ToString()),
                    Title = reader[9].ToString(),
                    Caste = Helper.ReturnCasteText(Int32.Parse(reader[10].ToString())),
                    CasteID = Int32.Parse(reader[10].ToString()),
                    Rank = Helper.ReturnCharacterRankText(Int32.Parse(reader[11].ToString())),
                    RankID = Int32.Parse(reader[11].ToString()),
                    Philosophy = Helper.ReturnPhilosophyText(Int32.Parse(reader[12].ToString())),
                    PhilosophyID = Int32.Parse(reader[12].ToString()),
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
