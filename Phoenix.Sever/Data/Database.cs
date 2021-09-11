using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Reflection;

namespace Phoenix.Server
{
    public class Database
    {

        #region -- Helpers --

        /// <summary>
        /// Returns Connection String
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ConnectionString</returns>
        private static string LoadConnectionString(string id = "Live")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

        public void InitializeDatabse()
        {
            // Create LiveDB if it doesn't exist.
            if (!File.Exists("./LiveDB.db"))
            {
                SQLiteConnection.CreateFile("LiveDB.db");

                // Log Database Creation
                Logger.ConsoleLog("Error", "LiveDB.db could not be located. Created new LiveDB.db.");

                using (var connection = new SQLiteConnection(LoadConnectionString("Live")))
                {
                    connection.Open();
                    using (var command = new SQLiteCommand(connection))
                    {
                        // Create Accounts
                        command.CommandText = File.ReadAllText("./SQL/CreateAccount.sql");
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created Accounts Table.");

                        // Create RoomStatus
                        command.CommandText = File.ReadAllText("./SQL/CreateRoomStatus.sql");
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created RoomStatuses Table.");

                        // Create RoomType
                        command.CommandText = File.ReadAllText("./SQL/CreateRoomType.sql");
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created RoomTypes Table.");

                        // Create RoomTiles
                        command.CommandText = File.ReadAllText("./SQL/CreateRoomTile.sql");
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created RoomTiles Table.");

                        // Create RoomKeyTypes
                        command.CommandText = File.ReadAllText("./SQL/CreateRoomKeyType.sql");
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created RoomKeyTypes Table.");

                        // Create RoomKeyModes
                        command.CommandText = File.ReadAllText("./SQL/CreateRoomKeyMode.sql");
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created RoomKeyModes Table.");

                        // Create CharacterTypes
                        command.CommandText = File.ReadAllText("./SQL/CreateCharacterTypes.sql");
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created CharacterTypes Table.");

                        // Create Rooms
                        command.CommandText = File.ReadAllText("./SQL/CreateRoom.sql");
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created Rooms Table.");

                        // Create Characters
                        command.CommandText = File.ReadAllText("./SQL/CreateCharacter.sql");
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created Characters Table.");

                        // Create ItemTypes
                        command.CommandText = File.ReadAllText("./SQL/CreateItemType.sql");
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created ItemTypes Table.");

                        // Create ItemSlots
                        command.CommandText = File.ReadAllText("./SQL/CreateItemSlot.sql");
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created ItemSlots Table.");

                        // Create Rarity
                        command.CommandText = File.ReadAllText("./SQL/CreateRarity.sql");
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created Rarity Table.");

                        // Create Philosophies
                        command.CommandText = File.ReadAllText("./SQL/CreatePhilosophies.sql");
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created Philosophies Table.");

                        // Create AbilityTypes
                        command.CommandText = File.ReadAllText("./SQL/CreateAbilityType.sql");
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created AbilityTypes Table.");

                        // Create AbilityRanks
                        command.CommandText = File.ReadAllText("./SQL/CreateAbilityRank.sql");
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created AbilityRanks Table.");

                        // Create Abilities
                        command.CommandText = File.ReadAllText("./SQL/CreateAbility.sql");
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created Abilities Table.");

                        // Create Items
                        command.CommandText = File.ReadAllText("./SQL/CreateItem.sql");
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created Items Table.");

                        // Create EntityTypes
                        command.CommandText = File.ReadAllText("./SQL/CreateEntityTypes.sql");
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created EntityTypes Table.");

                        // Create Entities
                        command.CommandText = File.ReadAllText("./SQL/CreateEntity.sql");
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created Entities Table.");

                        // Create RoomEntities
                        command.CommandText = File.ReadAllText("./SQL/CreateRoomEntities.sql");
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created RoomEntities Table.");

                        // Create RoomItems
                        command.CommandText = File.ReadAllText("./SQL/CreateRoomItems.sql");
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created RoomItems Table.");

                        // Create CharacterItems
                        command.CommandText = File.ReadAllText("./SQL/CreateCharacterItems.sql");
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created CharacterItems Table.");

                        // Create EntityItems
                        command.CommandText = File.ReadAllText("./SQL/CreateEntityItems.sql");
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created EntityItems Table.");
                    }
                    connection.Close();
                }
            }

            // Create TestDB if it doesn't exist.
            if (!File.Exists("./TestDB.db"))
            {
                SQLiteConnection.CreateFile("TestDB.db");

                // Log Database Creation
                Logger.ConsoleLog("Error", "TestDB.db could not be located. Created new TestDB.db.");

                using (var connection = new SQLiteConnection(LoadConnectionString("Test")))
                {
                    connection.Open();
                    using (var command = new SQLiteCommand(connection))
                    {
                        // Create Accounts
                        command.CommandText = File.ReadAllText("./SQL/CreateAccount.sql");
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created Accounts Table.");

                        // Create RoomStatus
                        command.CommandText = File.ReadAllText("./SQL/CreateRoomStatus.sql");
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created RoomStatuses Table.");

                        // Create RoomType
                        command.CommandText = File.ReadAllText("./SQL/CreateRoomType.sql");
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created RoomTypes Table.");

                        // Create RoomTiles
                        command.CommandText = File.ReadAllText("./SQL/CreateRoomTile.sql");
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created RoomTiles Table.");

                        // Create RoomKeyTypes
                        command.CommandText = File.ReadAllText("./SQL/CreateRoomKeyType.sql");
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created RoomKeyTypes Table.");

                        // Create RoomKeyModes
                        command.CommandText = File.ReadAllText("./SQL/CreateRoomKeyMode.sql");
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created RoomKeyModes Table.");

                        // Create CharacterTypes
                        command.CommandText = File.ReadAllText("./SQL/CreateCharacterTypes.sql");
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created CharacterTypes Table.");

                        // Create Rooms
                        command.CommandText = File.ReadAllText("./SQL/CreateRoom.sql");
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created Rooms Table.");

                        // Create Characters
                        command.CommandText = File.ReadAllText("./SQL/CreateCharacter.sql");
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created Characters Table.");

                        // Create ItemTypes
                        command.CommandText = File.ReadAllText("./SQL/CreateItemType.sql");
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created ItemTypes Table.");

                        // Create ItemSlots
                        command.CommandText = File.ReadAllText("./SQL/CreateItemSlot.sql");
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created ItemSlots Table.");

                        // Create Rarity
                        command.CommandText = File.ReadAllText("./SQL/CreateRarity.sql");
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created Rarity Table.");

                        // Create Philosophies
                        command.CommandText = File.ReadAllText("./SQL/CreatePhilosophies.sql");
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created Philosophies Table.");

                        // Create AbilityTypes
                        command.CommandText = File.ReadAllText("./SQL/CreateAbilityType.sql");
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created AbilityTypes Table.");

                        // Create AbilityRanks
                        command.CommandText = File.ReadAllText("./SQL/CreateAbilityRank.sql");
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created AbilityRanks Table.");

                        // Create Abilities
                        command.CommandText = File.ReadAllText("./SQL/CreateAbility.sql");
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created Abilities Table.");

                        // Create Items
                        command.CommandText = File.ReadAllText("./SQL/CreateItem.sql");
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created Items Table.");

                        // Create EntityTypes
                        command.CommandText = File.ReadAllText("./SQL/CreateEntityTypes.sql");
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created EntityTypes Table.");

                        // Create Entities
                        command.CommandText = File.ReadAllText("./SQL/CreateEntity.sql");
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created Entities Table.");

                        // Create RoomEntities
                        command.CommandText = File.ReadAllText("./SQL/CreateRoomEntities.sql");
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created RoomEntities Table.");

                        // Create RoomItems
                        command.CommandText = File.ReadAllText("./SQL/CreateRoomItems.sql");
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created RoomItems Table.");

                        // Create CharacterItems
                        command.CommandText = File.ReadAllText("./SQL/CreateCharacterItems.sql");
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created CharacterItems Table.");

                        // Create EntityItems
                        command.CommandText = File.ReadAllText("./SQL/CreateEntityItems.sql");
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created EntityItems Table.");
                    }
                    connection.Close();
                }
            }
        }

        #endregion

        #region -- Account ---

        public void SaveAccount(string name, string password, string email)
        {
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                return;
            }
        }

        #endregion

        #region -- Items--

        public void LoadItems()
        {

        }

        #endregion

        #region -- Debug --

        public void TestLogger()
        {
            Logger.ConsoleLog("System", "Testing System Logger.");
        }

        #endregion
    }
}
