using Phoenix.Common.Data;
using Phoenix.Common.Data.Types;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.IO;
using System.Linq;

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
            CheckDatabase("TestDB.db", "Test");
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
            string query = $"SELECT r.ID as RoomID, r.Name as RoomName, r.Area as RoomArea, r.Status as RoomStatus, r.Type as RoomType, r.Description as RoomDescription, r.Exits as RoomExits, r.North as RoomNorth, r.South as RoomSouth, r.West as RoomWest, r.East as RoomEast, r.Up as RoomUp, r.Down as RoomDown, r.KeyModeNorth as RoomKeyModeNorth, r.KeyModeSouth as RoomKeyModeSouth, r.KeyModeWest as RoomKeyModeWest, r.KeyModeEast as RoomKeyModeEast, r.KeyModeUp as RoomKeyModeUp, r.KeyModeDown as RoomKeyModeDown, r.KeyNameNorth as RoomKeyNameNorth, r.KeyNameSouth as RoomKeyNameSouth, r.KeyNameWest as RoomKeyNameWest, r.KeyNameEast as RoomKeyNameEast, r.KeyNameUp as RoomKeyNameUp, r.KeyNameDown as RoomKeyNameDown, r.KeyTypeNorth as RoomKeyTypeNorth, r.KeyTypeSouth as RoomKeyTypeSouth, r.KeyTypeWest as RoomKeyTypeWest, r.KeyTypeEast as RoomKeyTypeEast, r.KeyTypeUp as RoomKeyTypeUp, r.KeyTypeDown as RoomKeyTypeDown, r.KeyPassNorth as RoomKeyPassNorth, r.KeyPassSouth as RoomKeyPassSouth, r.KeyPassWest as RoomKeyPassWest, r.KeyPassEast as RoomKeyPassEast, r.KeyPassUp as RoomKeyPassUp, r.KeyPassDown as RoomKeyPassDown, r.KeyFailNorth as RoomKeyFailNorth, r.KeyFailSouth as RoomKeyFailSouth, r.KeyFailWest as RoomKeyFailWest, r.KeyFailEast as RoomKeyFailEast, r.KeyFailUp as RoomKeyFailUp, r.KeyFailDown as RoomKeyFailDown, r.Script as RoomScript, e.ID as NPCID, e.Type as NPCType, e.Rarity as NPCRarity, e.Name as NPCName, e.Image as NPCImage, e.HisHer as NPCHisHer, e.HeShe as NPCHeShe, e.BName as NPCBName, e.Level as NPCLevel, e.Gold as NPCGold, e.Strength as NPCStrength, e.Agility as NPCAgility, e.Intellect as NPCIntellect, e.Stamina as NPCStamina, e.Damage as NPCDamage, e.Haste as NPCHaste, e.Crit as NPCCrit, e.Mastery as NPCMastery, e.Versatility as NPCVersatility, e.Health as NPCHealth, e.Mana as NPCMana, e.Taunt as NPCTaunt, e.SpawnTime as NPCSpawnTime, e.SpawnDelay as NPCSpawnDelay, e.VanishTime as NPCVanishTime, e.Script as NPCScript, nd.EntityID as NPCDEntityID, nd.ItemId as NPCDItemID, nd.DropChance as NPCDDropChance, nd.ItemAmount as NPCDItemAmount FROM Rooms r LEFT OUTER JOIN RoomNPC re ON re.RoomID = r.ID LEFT OUTER JOIN NPC e ON e.ID = re.NPCID LEFT OUTER JOIN NPCItems nd ON nd.EntityID = e.ID; ";
            using var command = new SQLiteCommand(query, connection);
            using SQLiteDataReader reader = command.ExecuteReader();
            List<RoomNPCDto> rawData = new();
            while (reader.Read())
            {
                var roomNPCDto = new RoomNPCDto
                {
                    RoomID = Int32.Parse(reader["RoomID"]?.ToString()),
                    RoomName = reader["RoomName"].ToString(),
                    RoomArea = reader["RoomArea"].ToString(),
                    RoomStatus = Int32.Parse(reader["RoomStatus"]?.ToString()),
                    RoomType = Int32.Parse(reader["RoomType"]?.ToString()),
                    RoomDescription = reader["RoomDescription"].ToString(),
                    RoomExits = reader["RoomExits"].ToString(),
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
                    NPCID = int.TryParse(reader["NPCID"]?.ToString(), out int NPCID) ? NPCID : (int?)null,
                    NPCType = int.TryParse(reader["NPCType"]?.ToString(), out int NPCType) ? NPCType : (int?)null,
                    NPCRarity = int.TryParse(reader["NPCRarity"]?.ToString(), out int NPCRarity) ? NPCRarity : (int?)null,
                    NPCName = reader["NPCName"].ToString(),
                    NPCImage = reader["NPCImage"].ToString(),
                    NPCHisHer = reader["NPCHisHer"].ToString(),
                    NPCHeShe = reader["NPCHeShe"].ToString(),
                    NPCBName = reader["NPCBName"].ToString(),
                    NPCLevel = int.TryParse(reader["NPCLevel"]?.ToString(), out int NPCLevel) ? NPCLevel : (int?)null,
                    NPCGold = int.TryParse(reader["NPCGold"]?.ToString(), out int NPCGold) ? NPCGold : (int?)null,
                    NPCStrength = int.TryParse(reader["NPCStrength"]?.ToString(), out int NPCStrength) ? NPCStrength : (int?)null,
                    NPCAgility = int.TryParse(reader["NPCAgility"]?.ToString(), out int NPCAgility) ? NPCAgility : (int?)null,
                    NPCIntellect = int.TryParse(reader["NPCIntellect"]?.ToString(), out int NPCIntellect) ? NPCIntellect : (int?)null,
                    NPCStamina = int.TryParse(reader["NPCStamina"]?.ToString(), out int NPCStamina) ? NPCStamina : (int?)null,
                    NPCDamage = int.TryParse(reader["NPCDamage"]?.ToString(), out int NPCDamage) ? NPCDamage : (int?)null,
                    NPCHaste = double.TryParse(reader["NPCDamage"]?.ToString(), out double NPCHaste) ? NPCHaste : (double?)null,
                    NPCCrit = double.TryParse(reader["NPCDamage"]?.ToString(), out double NPCCrit) ? NPCCrit : (double?)null,
                    NPCMastery = double.TryParse(reader["NPCDamage"]?.ToString(), out double NPCMastery) ? NPCMastery : (double?)null,
                    NPCVersatility = double.TryParse(reader["NPCDamage"]?.ToString(), out double NPCVersatility) ? NPCVersatility : (double?)null,
                    NPCHealth = int.TryParse(reader["NPCHealth"]?.ToString(), out int NPCHealth) ? NPCHealth : (int?)null,
                    NPCMana = int.TryParse(reader["NPCMana"]?.ToString(), out int NPCMana) ? NPCMana : (int?)null,
                    NPCTaunt = int.TryParse(reader["NPCTaunt"]?.ToString(), out int NPCTaunt) ? NPCTaunt : (int?)null,
                    NPCSpawnTime = int.TryParse(reader["NPCSpawnTime"]?.ToString(), out int NPCSpawnTime) ? NPCSpawnTime : (int?)null,
                    NPCSpawnDelay = int.TryParse(reader["NPCSpawnDelay"]?.ToString(), out int NPCSpawnDelay) ? NPCSpawnDelay : (int?)null,
                    NPCVanishTime = int.TryParse(reader["NPCVanishTime"]?.ToString(), out int NPCVanishTime) ? NPCVanishTime : (int?)null,
                    NPCScript = reader["NPCScript"].ToString(),
                    NPCDEntityID = int.TryParse(reader["NPCDEntityID"]?.ToString(), out int NPCDEntityID) ? NPCDEntityID : (int?)null,
                    NPCDDropChance = double.TryParse(reader["NPCDDropChance"]?.ToString(), out double NPCDDropChance) ? NPCDDropChance : (double?)null,
                    NPCDItemAmount = int.TryParse(reader["NPCDItemAmount"]?.ToString(), out int NPCDItemAmount) ? NPCDItemAmount : (int?)null,
                    NPCDItemID = int.TryParse(reader["NPCDItemID"]?.ToString(), out int NPCDItemID) ? NPCDItemID : (int?)null
                };
                rawData.Add(roomNPCDto);
            }

            return (from data in rawData
                    group data by new { data.RoomID, data.RoomName, data.RoomArea, data.RoomStatus, data.RoomType, data.RoomDescription, data.RoomExits, data.RoomNorth, data.RoomSouth, data.RoomWest, data.RoomEast, data.RoomUp, data.RoomDown, data.RoomKeyModeNorth, data.RoomKeyModeSouth, data.RoomKeyModeWest, data.RoomKeyModeEast, data.RoomKeyModeUp, data.RoomKeyModeDown, data.RoomKeyNameNorth, data.RoomKeyNameSouth, data.RoomKeyNameWest, data.RoomKeyNameEast, data.RoomKeyNameUp, data.RoomKeyNameDown, data.RoomKeyTypeNorth, data.RoomKeyTypeSouth, data.RoomKeyTypeWest, data.RoomKeyTypeEast, data.RoomKeyTypeUp, data.RoomKeyTypeDown, data.RoomKeyPassNorth, data.RoomKeyPassSouth, data.RoomKeyPassWest, data.RoomKeyPassEast, data.RoomKeyPassUp, data.RoomKeyPassDown, data.RoomKeyFailNorth, data.RoomKeyFailSouth, data.RoomKeyFailWest, data.RoomKeyFailEast, data.RoomKeyFailUp, data.RoomKeyFailDown, data.RoomScript } into g
                    select new Room
                    {
                        ID = g.Key.RoomID,
                        Name = g.Key.RoomName,
                        Area = g.Key.RoomArea,
                        Status = g.Key.RoomStatus,
                        Type = g.Key.RoomType,
                        Description = g.Key.RoomDescription,
                        Exits = Helper.RemoveTilda(g.Key.RoomExits),
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
                        NPC = g.Where(e => e.NPCID.HasValue).ToList().Select(e => new NPC
                        {
                            ID = e.NPCID.Value,
                            Type = Helper.ReturnNPCTypeText(e.NPCType.Value),
                            TypeID = e.NPCType.Value,
                            Rarity = Helper.ReturnRarityText(e.NPCRarity.Value),
                            RarityID = e.NPCRarity.Value,
                            Name = e.NPCName,
                            Image = e.NPCImage,
                            HisHer = e.NPCHisHer,
                            HeShe = e.NPCHeShe,
                            BName = e.NPCBName,
                            Level = e.NPCLevel.Value,
                            Gold = e.NPCGold.Value,
                            Strength = e.NPCStrength.Value,
                            Agility = e.NPCAgility.Value,
                            Intellect = e.NPCIntellect.Value,
                            Stamina = e.NPCStamina.Value,
                            Damage = e.NPCDamage.Value,
                            Haste = e.NPCHaste.Value,
                            Crit = e.NPCCrit.Value,
                            Mastery = e.NPCMastery.Value,
                            Versatility = e.NPCVersatility.Value,
                            Health = e.NPCHealth.Value,
                            Mana = e.NPCMana.Value,
                            Taunt = e.NPCTaunt.Value,
                            SpawnTime = e.NPCSpawnTime.Value,
                            SpawnDelay = e.NPCSpawnDelay.Value,
                            VanishTime = e.NPCVanishTime.Value,
                            Script = e.NPCScript,
                            InstanceID = Guid.NewGuid(),
                            DisplayName = e.NPCName + " (Level: " + e.NPCLevel + ")",
                            CurrentAgility = e.NPCAgility.Value,
                            CurrentStrength = e.NPCStrength.Value,
                            CurrentIntellect = e.NPCIntellect.Value,
                            CurrentStamina = e.NPCStamina.Value,
                            AttackSpeed = 1,
                            CurrentHealth = e.NPCHealth.Value,
                            CurrentMana = e.NPCMana.Value,
                            IsAttacking = false,
                            RoomID = g.Key.RoomID,
                            TargetID = "",
                            Threat = 0,
                            TargetIsPlayer = false,
                            Drops = g.Where(x => x.NPCDEntityID.HasValue).ToList().Select(x => new NPCItems
                            {
                                EntityID = x.NPCDEntityID.Value,
                                DropChance = x.NPCDDropChance.Value,
                                ItemID = x.NPCDItemID.Value,
                                ItemAmount = x.NPCDItemAmount.Value
                            }).ToList()
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
        /// Updates An Account Field in Database.
        /// </summary>
        /// <param name="connectionType"></param>
        /// <param name="column"></param>
        /// <param name="columnValue"></param>
        /// <param name="field"></param>
        /// <param name="fieldValue"></param>
        public static void SetAccountField(string connectionType, string column, string columnValue, string field, string fieldValue)
        {
            using SQLiteConnection connection = new(LoadConnectionString(connectionType));
            connection.Open();
            string query = $"UPDATE Accounts SET {field} = '{fieldValue}' WHERE {column} = '{columnValue}';";
            using var command = new SQLiteCommand(query, connection);
            command.ExecuteNonQuery();
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

        public static void SetCharacterField(string connectionType, string column, string columnValue, string field, string fieldValue)
        {
            using SQLiteConnection connection = new(LoadConnectionString(connectionType));
            connection.Open();
            string query = $"UPDATE Characters SET {field} = '{fieldValue}' WHERE {column} = '{columnValue}';";
            using var command = new SQLiteCommand(query, connection);
            command.ExecuteNonQuery();
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
            string query = $"INSERT INTO Characters (AccountID, Name, Image, Type, Gender, HisHer, HeShe, Experience, Title, Caste, Rank, Philosophy, Alignment, Creation, Strength, Agility, Intellect, Stamina, Damage, Health, Mana, RoomID, CurrentHealth, CurrentMana, Recall, AutoAttack, AutoLoot) VALUES ('{accountID}', '{CharacterName}', '{Image}', 0, '{Gender}', '{hisHer}', '{heShe}', 0, 'Initiate', 0, 1, {Philosophy}, 0, 0, 10, 10, 10, 10, 20, 20, 20, 1, 20, 20, 1, false, false);";
            using var command = new SQLiteCommand(query, connection);
            command.ExecuteNonQuery();
        }

        public static void SetCharacter(string connectionType, Character character)
        {
            using SQLiteConnection connection = new(LoadConnectionString(connectionType));
            connection.Open();
            string query = $"UPDATE Characters SET Experience = '{character.Experience}',  Title = '{character.Title}', Caste = '{character.CasteID}', Rank = '{character.RankID}', Alignment = '{character.Alignment}', Strength = '{character.Strength}',  Agility = '{character.Agility}', Intellect = '{character.Intellect}',  Stamina = '{character.Stamina}',  Damage = '{character.Damage}',  Health = '{character.Health}',  Mana = '{character.Mana}', RoomID = '{character.RoomID}', CurrentHealth = '{character.CurrentHealth}', CurrentMana = '{character.CurrentMana}', Recall = '{character.Recall}', AutoAttack = '{character.AutoAttack}', AutoLoot = '{character.AutoLoot}' WHERE Name = '{character.Name.ToLower()}';";
            using var command = new SQLiteCommand(query, connection);
            command.ExecuteNonQuery();
        }

        public static List<Character> GetCharacterList(string connectionType, int accountID)
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

        public static Character GetCharacter(string connectionType, int accountID, string name)
        {
            using var connection = new SQLiteConnection(LoadConnectionString(connectionType));
            connection.Open();
            string query = $"SELECT ID, AccountID, Name, Type, Image, Gender, HisHer, HeShe, Experience, Title, Caste, Rank, Philosophy, Alignment, Creation, Strength, Agility, Intellect, Stamina, Damage, Health, Mana, RoomID, CurrentHealth, CurrentMana, Recall, AutoAttack, AutoLoot FROM Characters WHERE AccountID = '{accountID}' AND Name = '{name}';";
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
                    Versatility = 0,
                    CurrentStrength = Int32.Parse(reader[15].ToString()),
                    CurrentAgility = Int32.Parse(reader[16].ToString()),
                    CurrentIntellect = Int32.Parse(reader[17].ToString()),
                    CurrentStamina = Int32.Parse(reader[18].ToString()),
                    CurrentDamage = Int32.Parse(reader[19].ToString()),
                    CurrentHealth = Int32.Parse(reader[23].ToString()),
                    CurrentMana = Int32.Parse(reader[24].ToString()),
                    CurrentArmor = 0,
                    AttackSpeed = 1,
                    IsAttacking = false,
                    IsDead = false,
                    MaxExperience = Int32.Parse(reader[11].ToString()) * 1500,
                    TargetID = "",
                    Recall = Int32.Parse(reader[25].ToString()),
                    AutoAttack = bool.Parse(reader[26].ToString()),
                    AutoLoot = bool.Parse(reader[27].ToString()),
                    TargetIsPlayer = false,
                    HealthRegen = true
                };
                return character;
            }
            return null;
        }

        #endregion

        #region -- Items --

        public static string GetItemField(string connectionType, string column, string field, string value)
        {
            using var connection = new SQLiteConnection(LoadConnectionString(connectionType));
            connection.Open();
            string query = $"SELECT {column} FROM Items WHERE {field} = '{value}';";
            using var command = new SQLiteCommand(query, connection);
            using SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                return reader[0].ToString();
            }
            return null;
        }

        public static List<Item> LoadItems(string connectionType)
        {
            using var connection = new SQLiteConnection(LoadConnectionString(connectionType));
            connection.Open();
            string query = $"SELECT ID, Name, Image, Type, Slot, Value, Rarity, Weight, Damage, Strength, Agility, Intellect, Stamina, Crit, Haste, Mastery, Versatility, PhilosophyReq, StrengthReq, AgilityReq, IntellectReq, StaminaReq, AlignmentReq, RankReq, Script FROM Items";
            using var command = new SQLiteCommand(query, connection);
            using SQLiteDataReader reader = command.ExecuteReader();
            List<Item> items = new();
            while (reader.Read())
            {
                Item item = new()
                {
                    Id = Convert.ToInt32(reader[0].ToString()),
                    Name = reader[1].ToString(),
                    Image = reader[2].ToString(),
                    TypeID = Convert.ToInt32(reader[3].ToString()),
                    SlotID = Convert.ToInt32(reader[4].ToString()),
                    Value = Convert.ToInt32(reader[5].ToString()),
                    RarityID = Convert.ToInt32(reader[6].ToString()),
                    Weight = Convert.ToInt32(reader[7].ToString()),
                    Damage = Convert.ToInt32(reader[8].ToString()),
                    Strength = Convert.ToInt32(reader[9].ToString()),
                    Agility = Convert.ToInt32(reader[10].ToString()),
                    Intellect = Convert.ToInt32(reader[11].ToString()),
                    Stamina = Convert.ToInt32(reader[12].ToString()),
                    Crit = Convert.ToDouble(reader[13].ToString()),
                    Haste = Convert.ToDouble(reader[14].ToString()),
                    Mastery = Convert.ToDouble(reader[15].ToString()),
                    Versatility = Convert.ToDouble(reader[16].ToString()),
                    PhilosophyReq = Convert.ToInt32(reader[17].ToString()),
                    StrengthReq = Convert.ToInt32(reader[18].ToString()),
                    AgilityReq = Convert.ToInt32(reader[19].ToString()),
                    IntellectReq = Convert.ToInt32(reader[20].ToString()),
                    StaminaReq = Convert.ToInt32(reader[21].ToString()),
                    AlignmentReq = Convert.ToInt32(reader[22].ToString()),
                    RankReq = Convert.ToInt32(reader[23].ToString()),
                    Script = reader[24].ToString(),
                    Amount = 1,
                    Rarity = Helper.ReturnRarityText(Convert.ToInt32(reader[6].ToString())),
                    Slot = Helper.ReturnSlotText(Convert.ToInt32(reader[4].ToString())),
                    Type = Helper.ReturnTypeText(Convert.ToInt32(reader[3].ToString()))
                };
                items.Add(item);
            }
            return items;
        }

        #endregion

    }
}
