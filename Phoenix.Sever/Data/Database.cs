using Phoenix.Common.Data;
using Phoenix.Common.Data.Types;
using static Phoenix.Server.Program;
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
            string query = $"SELECT r.ID as RoomID, r.Name as RoomName, r.Area as RoomArea, r.Status as RoomStatus, r.Type as RoomType, r.Description as RoomDescription, r.Exits as RoomExits, r.North as RoomNorth, r.South as RoomSouth, r.West as RoomWest, r.East as RoomEast, r.Up as RoomUp, r.Down as RoomDown, r.KeyModeNorth as RoomKeyModeNorth, r.KeyModeSouth as RoomKeyModeSouth, r.KeyModeWest as RoomKeyModeWest, r.KeyModeEast as RoomKeyModeEast, r.KeyModeUp as RoomKeyModeUp, r.KeyModeDown as RoomKeyModeDown, r.KeyNameNorth as RoomKeyNameNorth, r.KeyNameSouth as RoomKeyNameSouth, r.KeyNameWest as RoomKeyNameWest, r.KeyNameEast as RoomKeyNameEast, r.KeyNameUp as RoomKeyNameUp, r.KeyNameDown as RoomKeyNameDown, r.KeyTypeNorth as RoomKeyTypeNorth, r.KeyTypeSouth as RoomKeyTypeSouth, r.KeyTypeWest as RoomKeyTypeWest, r.KeyTypeEast as RoomKeyTypeEast, r.KeyTypeUp as RoomKeyTypeUp, r.KeyTypeDown as RoomKeyTypeDown, r.KeyPassNorth as RoomKeyPassNorth, r.KeyPassSouth as RoomKeyPassSouth, r.KeyPassWest as RoomKeyPassWest, r.KeyPassEast as RoomKeyPassEast, r.KeyPassUp as RoomKeyPassUp, r.KeyPassDown as RoomKeyPassDown, r.KeyFailNorth as RoomKeyFailNorth, r.KeyFailSouth as RoomKeyFailSouth, r.KeyFailWest as RoomKeyFailWest, r.KeyFailEast as RoomKeyFailEast, r.KeyFailUp as RoomKeyFailUp, r.KeyFailDown as RoomKeyFailDown, r.Script as RoomScript, e.ID as NPCID, e.Type as NPCType, e.Rarity as NPCRarity, e.Name as NPCName, e.Image as NPCImage, e.HisHer as NPCHisHer, e.HeShe as NPCHeShe, e.BName as NPCBName, e.Level as NPCLevel, e.Gold as NPCGold, e.Strength as NPCStrength, e.Agility as NPCAgility, e.Intellect as NPCIntellect, e.Stamina as NPCStamina, e.Damage as NPCDamage, e.Haste as NPCHaste, e.Crit as NPCCrit, e.Mastery as NPCMastery, e.Versatility as NPCVersatility, e.Health as NPCHealth, e.Mana as NPCMana, e.Taunt as NPCTaunt, e.SpawnTime as NPCSpawnTime, e.SpawnDelay as NPCSpawnDelay, e.VanishTime as NPCVanishTime, e.Script as NPCScript, nd.EntityID as NPCDEntityID, nd.ItemId as NPCDItemID, nd.DropChance as NPCDDropChance, nd.ItemAmount as NPCDItemAmount, re.ID as NPCRID FROM Rooms r LEFT OUTER JOIN RoomNPC re ON re.RoomID = r.ID LEFT OUTER JOIN NPC e ON e.ID = re.NPCID LEFT OUTER JOIN NPCItems nd ON nd.EntityID = e.ID;";
            using var command = new SQLiteCommand(query, connection);
            using SQLiteDataReader reader = command.ExecuteReader();
            List<RoomNPCDTO> rawData = new();
            while (reader.Read())
            {
                var RoomNPCDTO = new RoomNPCDTO
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
                    NPCDItemID = int.TryParse(reader["NPCDItemID"]?.ToString(), out int NPCDItemID) ? NPCDItemID : (int?)null,
                    NPCRID = int.TryParse(reader["NPCRID"]?.ToString(), out int NPCRID) ? NPCRID : (int?)null
                };
                rawData.Add(RoomNPCDTO);
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
                        NPC = g.Where(e => e.NPCID.HasValue).GroupBy(z => z.NPCRID).ToList().Select(e => new NPC
                        {
                            ID = e.First().NPCID.Value,
                            Type = Helper.ReturnNPCTypeText(e.First().NPCType.Value),
                            TypeID = e.First().NPCType.Value,
                            Rarity = Helper.ReturnRarityText(e.First().NPCRarity.Value),
                            RarityID = e.First().NPCRarity.Value,
                            Name = e.First().NPCName,
                            Image = e.First().NPCImage,
                            HisHer = e.First().NPCHisHer,
                            HeShe = e.First().NPCHeShe,
                            BName = e.First().NPCBName,
                            Level = e.First().NPCLevel.Value,
                            Gold = e.First().NPCGold.Value,
                            Strength = e.First().NPCStrength.Value,
                            Agility = e.First().NPCAgility.Value,
                            Intellect = e.First().NPCIntellect.Value,
                            Stamina = e.First().NPCStamina.Value,
                            Damage = e.First().NPCDamage.Value,
                            Haste = e.First().NPCHaste.Value,
                            Crit = e.First().NPCCrit.Value,
                            Mastery = e.First().NPCMastery.Value,
                            Versatility = e.First().NPCVersatility.Value,
                            Health = e.First().NPCHealth.Value,
                            Mana = e.First().NPCMana.Value,
                            Taunt = e.First().NPCTaunt.Value,
                            SpawnTime = e.First().NPCSpawnTime.Value,
                            SpawnDelay = e.First().NPCSpawnDelay.Value,
                            VanishTime = e.First().NPCVanishTime.Value,
                            Script = e.First().NPCScript,
                            InstanceID = Guid.NewGuid(),
                            DisplayName = e.First().NPCName + " (Level: " + e.First().NPCLevel + ")",
                            CurrentAgility = e.First().NPCAgility.Value,
                            CurrentStrength = e.First().NPCStrength.Value,
                            CurrentIntellect = e.First().NPCIntellect.Value,
                            CurrentStamina = e.First().NPCStamina.Value,
                            AttackSpeed = 1,
                            CurrentHealth = e.First().NPCHealth.Value,
                            CurrentMana = e.First().NPCMana.Value,
                            IsAttacking = false,
                            RoomID = g.Key.RoomID,
                            TargetID = "",
                            Threat = 0,
                            TargetIsPlayer = false,
                            Drops = e.Where(x => x.NPCDEntityID.HasValue).ToList().Select(x => new NPCItems
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
            string query = $"INSERT INTO Characters (AccountID, Name, Image, Type, Gender, HisHer, HeShe, Experience, Title, Caste, Rank, Philosophy, Alignment, Creation, Strength, Agility, Intellect, Stamina, Damage, Health, Mana, RoomID, CurrentHealth, CurrentMana, Recall, AutoAttack, AutoLoot) VALUES ('{accountID}', '{CharacterName}', '{Image}', 0, '{Gender}', '{hisHer}', '{heShe}', 0, 'Initiate', 0, 1, {Philosophy}, 0, 0, 10, 10, 10, 10, 20, 20, 20, 1, 20, 20, 1, false, false, false, false, false);";
            using var command = new SQLiteCommand(query, connection);
            command.ExecuteNonQuery();
        }

        public static void SetCharacter(string connectionType, Character character)
        {
            using SQLiteConnection connection = new(LoadConnectionString(connectionType));
            connection.Open();
            string query = $"UPDATE Characters SET Experience = '{character.Experience}',  Title = '{character.Title}', Caste = '{character.CasteID}', Rank = '{character.RankID}', Alignment = '{character.Alignment}', Strength = '{character.Strength}',  Agility = '{character.Agility}', Intellect = '{character.Intellect}',  Stamina = '{character.Stamina}',  Damage = '{character.Damage}',  Health = '{character.Health}',  Mana = '{character.Mana}', RoomID = '{character.RoomID}', CurrentHealth = '{character.CurrentHealth}', CurrentMana = '{character.CurrentMana}', Recall = '{character.Recall}', AutoAttack = '{character.AutoAttack}', AutoLoot = '{character.AutoLoot}', HealthRegen = '{character.HealthRegen}', IsDead = '{character.IsDead}', IsGhosted = '{character.IsGhosted}' WHERE Name = '{character.Name.ToLower()}';";
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
            string query = $"SELECT c.ID as CharID, c.AccountID as CharAccountID, c.Name as CharName, c.Type as CharTypeID, c.Image as CharImage, c.Gender as CharGender, c.HisHer as CharHisHer, c.HeShe as CharHeShe, c.Experience as CharExperience, c.Title as CharTitle, c.Caste as CharCasteID, c.Rank as CharRankID, c.Philosophy as CharPhilosophyID, c.Alignment as CharAlignment, c.Creation as CharCreation, c.Strength as CharStrength, c.Agility as CharAgility, c.Intellect as CharIntellect, c.Stamina as CharStamina, c.Damage as CharDamage, c.Health as CharHealth, c.Mana as CharMana, c.RoomID as CharRoomID, c.CurrentHealth as CharCurrentHealth, c.CurrentMana as CharCurrentMana, c.AutoLoot as CharAutoLoot, c.AutoAttack as CharAutoAttack, c.Recall as CharRecall, c.HealthRegen as CharHealthRegen, c.IsDead as CharIsDead, c.IsGhosted as CharIsGhosted, ci.ID as CIID, ci.CharacterID as CICharacterID, ci.ItemID as CIItemID, ci.ItemAmount as CIItemAmount, ci.SlotIndex as CISlotIndex, ci.IsEquipped as CIIsEquipped, i.ID as ItemID, i.Name as ItemName, i.Image as ItemImage, i.Type as ItemTypeID, i.Slot as ItemSlotID, i.Value as ItemValue, i.Rarity as ItemRarityID, i.Weight as ItemWeight, i.Damage as ItemDamage, i.Strength as ItemStrength, i.Agility as ItemAgility, i.Intellect as ItemIntellect, i.Stamina as ItemStamina, i.Crit as ItemCrit, i.Haste as ItemHaste, i.Mastery as ItemMastery, i.Versatility as ItemVersatility, i.PhilosophyReq as ItemPhilosophyReq, i.StrengthReq as ItemStrengthReq, i.AgilityReq as ItemAgilityReq, i.IntellectReq as ItemIntellectReq, i.StaminaReq as ItemStaminaReq, i.AlignmentReq as ItemAlignmentReq, i.RankReq as ItemRankReq, i.Script as ItemScript FROM Characters c LEFT OUTER JOIN CharacterItems ci ON ci.CharacterID = c.ID LEFT OUTER JOIN Items i ON i.ID = ci.ItemID;";
            using var command = new SQLiteCommand(query, connection);
            using SQLiteDataReader reader = command.ExecuteReader();
            List<CharacterItemDTO> rawData = new();
            while (reader.Read())
            {
                var characterItemDTO = new CharacterItemDTO
                {
                    CharId = Int32.Parse(reader["CharID"]?.ToString()),
                    CharAccountId = Int32.Parse(reader["CharAccountID"]?.ToString()),
                    CharName = reader["CharName"].ToString(),
                    CharTypeID = Int32.Parse(reader["CharTypeID"]?.ToString()),
                    CharImage = reader["CharImage"].ToString(),
                    CharGender = reader["CharGender"].ToString(),
                    CharAgility = Int32.Parse(reader["CharAgility"]?.ToString()),
                    CharAlignment = Int32.Parse(reader["CharAlignment"]?.ToString()),
                    CharAutoAttack = bool.Parse(reader["CharAutoAttack"]?.ToString()),
                    CharAutoLoot = bool.Parse(reader["CharAutoLoot"]?.ToString()),
                    CharCasteID = Int32.Parse(reader["CharCasteID"]?.ToString()),
                    CharCreation = Int32.Parse(reader["CharCreation"]?.ToString()),
                    CharCurrentHealth = Int32.Parse(reader["CharCurrentHealth"]?.ToString()),
                    CharCurrentMana = Int32.Parse(reader["CharCurrentMana"]?.ToString()),
                    CharDamage = Int32.Parse(reader["CharDamage"]?.ToString()),
                    CharExperience = Int32.Parse(reader["CharExperience"]?.ToString()),
                    CharHealth = Int32.Parse(reader["CharHealth"]?.ToString()),
                    CharHealthRegen = bool.Parse(reader["CharHealthRegen"]?.ToString()),
                    CharHeShe = reader["CharHeShe"].ToString(),
                    CharHisHer = reader["CharHisHer"].ToString(),
                    CharIntellect = Int32.Parse(reader["CharIntellect"]?.ToString()),
                    CharIsDead = bool.Parse(reader["CharIsDead"]?.ToString()),
                    CharIsGhosted = bool.Parse(reader["CharIsGhosted"]?.ToString()),
                    CharMana = Int32.Parse(reader["CharMana"]?.ToString()),
                    CharPhilosophyID = Int32.Parse(reader["CharPhilosophyID"]?.ToString()),
                    CharRankID = Int32.Parse(reader["CharRankID"]?.ToString()),
                    CharRecall = Int32.Parse(reader["CharRecall"]?.ToString()),
                    CharRoomID = Int32.Parse(reader["CharRoomID"]?.ToString()),
                    CharStamina = Int32.Parse(reader["CharStamina"]?.ToString()),
                    CharStrength = Int32.Parse(reader["CharStrength"]?.ToString()),
                    CharTitle = reader["CharTitle"].ToString(),
                    CICharacterID = int.TryParse(reader["CICharacterID"]?.ToString(), out int CICharacterID) ? CICharacterID : (int?)null,
                    CIID = int.TryParse(reader["CIID"]?.ToString(), out int CIID) ? CIID: (int?)null,
                    CIIsEquipped = bool.TryParse(reader["CIIsEquipped"]?.ToString(), out bool CIIsEquipped) ? CIIsEquipped : (bool?)null,
                    CIItemAmount = int.TryParse(reader["CIItemAmount"]?.ToString(), out int CIItemAmount) ? CIItemAmount : (int?)null,
                    CIItemID = int.TryParse(reader["CIItemID"]?.ToString(), out int CIItemID) ? CIItemID : (int?)null,
                    CISlotIndex = int.TryParse(reader["CISlotIndex"]?.ToString(), out int CISlotIndex) ? CISlotIndex : (int?)null,
                    ItemAgility = int.TryParse(reader["ItemAgility"]?.ToString(), out int ItemAgility) ? ItemAgility : (int?)null,
                    ItemAgilityReq = int.TryParse(reader["ItemAgilityReq"]?.ToString(), out int ItemAgilityReq) ? ItemAgilityReq : (int?)null,
                    ItemAlignmentReq = int.TryParse(reader["ItemAlignmentReq"]?.ToString(), out int ItemAlignmentReq) ? ItemAlignmentReq : (int?)null,
                    ItemCrit = double.TryParse(reader["ItemCrit"]?.ToString(), out double ItemCrit) ? ItemCrit : (double?)null,
                    ItemDamage = int.TryParse(reader["ItemDamage"]?.ToString(), out int ItemDamage) ? ItemDamage : (int?)null,
                    ItemHaste = double.TryParse(reader["ItemHaste"]?.ToString(), out double ItemHaste) ? ItemHaste : (double?)null,
                    ItemId = int.TryParse(reader["ItemID"]?.ToString(), out int ItemId) ? ItemId : (int?)null,
                    ItemImage = reader["ItemImage"].ToString(),
                    ItemIntellect = int.TryParse(reader["ItemIntellect"]?.ToString(), out int ItemIntellect) ? ItemIntellect : (int?)null,
                    ItemIntellectReq = int.TryParse(reader["ItemIntellectReq"]?.ToString(), out int ItemIntellectReq) ? ItemIntellectReq : (int?)null,
                    ItemMastery = double.TryParse(reader["ItemMastery"]?.ToString(), out double ItemMastery) ? ItemMastery : (double?)null,
                    ItemName = reader["ItemName"].ToString(),
                    ItemPhilosophyReq = int.TryParse(reader["ItemPhilosophyReq"]?.ToString(), out int ItemPhilosophyReq) ? ItemPhilosophyReq : (int?)null,
                    ItemRankReq = int.TryParse(reader["ItemRankReq"]?.ToString(), out int ItemRankReq) ? ItemRankReq : (int?)null,
                    ItemRarityID = int.TryParse(reader["ItemRarityID"]?.ToString(), out int ItemRarityID) ? ItemRarityID : (int?)null,
                    ItemScript = reader["ItemScript"].ToString(),
                    ItemSlotID = int.TryParse(reader["ItemSlotID"]?.ToString(), out int ItemSlotID) ? ItemSlotID : (int?)null,
                    ItemStamina = int.TryParse(reader["ItemStamina"]?.ToString(), out int ItemStamina) ? ItemStamina : (int?)null,
                    ItemStaminaReq = int.TryParse(reader["ItemStaminaReq"]?.ToString(), out int ItemStaminaReq) ? ItemStaminaReq : (int?)null,
                    ItemStrength = int.TryParse(reader["ItemStrength"]?.ToString(), out int ItemStrength) ? ItemStrength : (int?)null,
                    ItemStrengthReq = int.TryParse(reader["ItemStrengthReq"]?.ToString(), out int ItemStrengthReq) ? ItemStrengthReq : (int?)null,
                    ItemTypeID = int.TryParse(reader["ItemTypeID"]?.ToString(), out int ItemTypeID) ? ItemTypeID : (int?)null,
                    ItemValue = int.TryParse(reader["ItemValue"]?.ToString(), out int ItemValue) ? ItemValue : (int?)null,
                    ItemVersatility = double.TryParse(reader["ItemVersatility"]?.ToString(), out double ItemVersatility) ? ItemVersatility : (double?)null,
                    ItemWeight = int.TryParse(reader["ItemWeight"]?.ToString(), out int ItemWeight) ? ItemWeight : (int?)null,
                };
                rawData.Add(characterItemDTO);
            }

            return (from data in rawData
                    group data by new { data.CharAccountId, data.CharAgility, data.CharAlignment, data.CharAutoAttack, data.CharAutoLoot, data.CharCasteID, data.CharCreation, data.CharCurrentHealth, data.CharCurrentMana, data.CharDamage, data.CharExperience, data.CharGender, data.CharHealth, data.CharHealthRegen, data.CharHeShe, data.CharHisHer, data.CharId, data.CharImage, data.CharIntellect, data.CharIsDead, data.CharIsGhosted, data.CharMana, data.CharName, data.CharPhilosophyID, data.CharRankID, data.CharRecall, data.CharRoomID, data.CharStamina, data.CharStrength, data.CharTitle, data.CharTypeID } into g
                    select new Character
                    {
                        AccountId = g.Key.CharAccountId,
                        Agility = g.Key.CharAgility,
                        Alignment = g.Key.CharAlignment,
                        AttackSpeed = 1,
                        AutoAttack = g.Key.CharAutoAttack,
                        AutoLoot = g.Key.CharAutoLoot,
                        Caste = Helper.ReturnCasteText(g.Key.CharCasteID),
                        CasteID = g.Key.CharCasteID,
                        Creation = g.Key.CharCreation,
                        Crit = 0,
                        CurrentAgility = g.Key.CharAgility,
                        CurrentArmor = 0,
                        CurrentDamage = g.Key.CharDamage,
                        CurrentHealth = g.Key.CharCurrentHealth,
                        CurrentIntellect = g.Key.CharIntellect,
                        CurrentMana = g.Key.CharCurrentMana,
                        CurrentStamina = g.Key.CharStamina,
                        CurrentStrength = g.Key.CharStrength,
                        Damage = g.Key.CharDamage,
                        Experience = g.Key.CharExperience,
                        Gender = g.Key.CharGender,
                        Gold = game.connectedAccounts.Values.Where(x => x.Account.Id == g.Key.CharAccountId).Select(x => x.Account.Gold).DefaultIfEmpty(0).First(),
                        Haste = 0,
                        Health = g.Key.CharHealth,
                        HealthRegen = g.Key.CharHealthRegen,
                        HeShe = g.Key.CharHeShe,
                        HisHer = g.Key.CharHisHer,
                        Id = g.Key.CharId,
                        Image = g.Key.CharImage,
                        Intellect = g.Key.CharIntellect,
                        IsAttacking = false,
                        IsDead = g.Key.CharIsDead,
                        IsGhosted = g.Key.CharIsGhosted,
                        Mana = g.Key.CharMana,
                        Mastery = 0,
                        MaxExperience = g.Key.CharExperience * 1500,
                        Name = g.Key.CharName,
                        Philosophy = Helper.ReturnPhilosophyText(g.Key.CharPhilosophyID),
                        PhilosophyID = g.Key.CharPhilosophyID,
                        Rank = Helper.ReturnCharacterRankText(g.Key.CharRankID),
                        RankID = g.Key.CharRankID,
                        Recall = g.Key.CharRecall,
                        RoomID = g.Key.CharRoomID,
                        Stamina = g.Key.CharStamina,
                        Strength = g.Key.CharStrength,
                        TargetID = "",
                        TargetIsPlayer = false,
                        Title = g.Key.CharTitle,
                        Type = Helper.ReturnCharacterTypeText(g.Key.CharTypeID),
                        TypeID = g.Key.CharTypeID,
                        Versatility = 0,
                        Items = g.Where(e => e.CIItemID.HasValue).GroupBy(z => z.CIID).ToList().Select(e => new Item
                        {
                           Id = e.First().ItemId.Value,
                           Image = e.First().ItemImage,
                           Agility = e.First().ItemAgility.Value,
                           AgilityReq = e.First().ItemAgilityReq.Value,
                           AlignmentReq = e.First().ItemAlignmentReq.Value,
                           Amount = e.First().CIItemAmount.Value,
                           Crit = e.First().ItemCrit.Value,
                           Damage = e.First().ItemDamage.Value,
                           Haste = e.First().ItemHaste.Value,
                           Intellect = e.First().ItemIntellect.Value,
                           IntellectReq = e.First().ItemIntellectReq.Value,
                           Mastery = e.First().ItemMastery.Value,
                           Name = e.First().ItemName,
                           PhilosophyReq = e.First().ItemPhilosophyReq.Value,
                           RankReq = e.First().ItemRankReq.Value,
                           Rarity = Helper.ReturnRarityText(e.First().ItemRarityID.Value),
                           RarityID = e.First().ItemRarityID.Value,
                           Script = e.First().ItemScript,
                           Slot = Helper.ReturnSlotText(e.First().ItemSlotID.Value),
                           SlotID = e.First().ItemSlotID.Value,
                           Stamina = e.First().ItemStamina.Value,
                           StaminaReq = e.First().ItemStamina.Value,
                           Strength = e.First().ItemStrength.Value,
                           StrengthReq = e.First().ItemStrengthReq.Value,
                           Type = Helper.ReturnTypeText(e.First().ItemTypeID.Value),
                           TypeID = e.First().ItemTypeID.Value,
                           Value = e.First().ItemValue.Value,
                           Versatility = e.First().ItemVersatility.Value,
                           Weight = e.First().ItemWeight.Value,
                           SlotIndex = e.First().CISlotIndex.Value,
                           IsEquipped = e.First().CIIsEquipped.Value
                        }).ToList(),
                        InventoryIndex = g.Where(e => e.CIItemID.HasValue).GroupBy(z => z.CIID).ToList().Count
                    }).ToList().Where(x => x.Name.ToLower() == name.ToLower() && x.AccountId == accountID).FirstOrDefault();
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
