using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.IO;

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
                        command.CommandText = "CREATE TABLE Accounts(ID integer NOT NULL PRIMARY KEY AUTOINCREMENT, AccountName text NOT NULL, AccountPassword text NOT NULL, AccountEmail text NOT NULL, AccountCharacters text, AccountGold integer NOT NULL DEFAULT 0)";
                        command.ExecuteNonQuery();

                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created Accounts Table.");

                        command.CommandText = "CREATE TABLE Characters (ID integer NOT NULL PRIMARY KEY AUTOINCREMENT, AccountID integer NOT NULL, CharacterName text NOT NULL, CharacterGender text NOT NULL, CharacterPhilosophy text NOT NULL, CharacterLevel integer NOT NULL DEFAULT 1, CharacterExperience integer NOT NULL DEFAULT 0, CharacterStrength integer NOT NULL DEFAULT 10, CharacterAgility integer NOT NULL DEFAULT 10, CharacterIntellect integer NOT NULL DEFAULT 10, CharacterStamina integer NOT NULL DEFAULT 10, CharacterArmor integer NOT NULL DEFAULT 0, CharacterDamage integer NOT NULL DEFAULT 20, CharacterCrit integer NOT NULL DEFAULT 0, CharacterHaste integer NOT NULL DEFAULT 0, CharacterMastery integer NOT NULL DEFAULT 0, CharacterVersatility integer NOT NULL DEFAULT 0, CharacterWeight integer NOT NULL DEFAULT 0, CharacterHealth integer NOT NULL DEFAULT 20, CharacterMana integer NOT NULL DEFAULT 20, CharacterInventory text, FOREIGN KEY (AccountID) REFERENCES Accounts (ID))";
                        command.ExecuteNonQuery();

                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created Characters Table.");

                        command.CommandText = "CREATE TABLE Rooms (ID integer NOT NULL PRIMARY KEY AUTOINCREMENT, RoomName text NOT NULL, RoomArea text NOT NULL, RoomDescription text NOT NULL, RoomExits text NOT NULL, RoomNorth integer, RoomSouth integer, RoomWest integer, RoomEast integer, RoomUp integer, RoomDown Integer, RoomTile integer NOT NULL DEFAULT 1)";
                        command.ExecuteNonQuery();

                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created Rooms Table.");

                        command.CommandText = "CREATE TABLE Items (ID integer NOT NULL PRIMARY KEY AUTOINCREMENT, ItemName text NOT NULL, ItemType text NOT NULL, RoomDescription text NOT NULL, RoomExits text NOT NULL, RoomNorth integer, RoomSouth integer, RoomWest integer, RoomEast integer, RoomUp integer, RoomDown Integer, RoomTile integer NOT NULL DEFAULT 1)";
                        command.ExecuteNonQuery();

                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created Items Table.");

                        command.CommandText = "CREATE TABLE Entities (ID integer NOT NULL PRIMARY KEY AUTOINCREMENT, ItemName text NOT NULL, ItemType text NOT NULL, RoomDescription text NOT NULL, RoomExits text NOT NULL, RoomNorth integer, RoomSouth integer, RoomWest integer, RoomEast integer, RoomUp integer, RoomDown Integer, RoomTile integer NOT NULL DEFAULT 1)";
                        command.ExecuteNonQuery();

                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created Entities Table.");
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
                        command.CommandText = "CREATE TABLE Accounts(ID integer NOT NULL PRIMARY KEY AUTOINCREMENT, AccountName text NOT NULL, AccountPassword text NOT NULL, AccountEmail text NOT NULL, AccountCharacters text, AccountGold integer NOT NULL DEFAULT 0)";
                        command.ExecuteNonQuery();

                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created Accounts Table.");

                        command.CommandText = "CREATE TABLE Characters (ID integer NOT NULL PRIMARY KEY AUTOINCREMENT, AccountID integer NOT NULL, CharacterName text NOT NULL, CharacterGender text NOT NULL, CharacterPhilosophy text NOT NULL, CharacterLevel integer NOT NULL DEFAULT 1, CharacterExperience integer NOT NULL DEFAULT 0, CharacterStrength integer NOT NULL DEFAULT 10, CharacterAgility integer NOT NULL DEFAULT 10, CharacterIntellect integer NOT NULL DEFAULT 10, CharacterStamina integer NOT NULL DEFAULT 10, CharacterArmor integer NOT NULL DEFAULT 0, CharacterDamage integer NOT NULL DEFAULT 20, CharacterCrit integer NOT NULL DEFAULT 0, CharacterHaste integer NOT NULL DEFAULT 0, CharacterMastery integer NOT NULL DEFAULT 0, CharacterVersatility integer NOT NULL DEFAULT 0, CharacterWeight integer NOT NULL DEFAULT 0, CharacterHealth integer NOT NULL DEFAULT 20, CharacterMana integer NOT NULL DEFAULT 20, CharacterInventory text, FOREIGN KEY (AccountID) REFERENCES Accounts (ID))";
                        command.ExecuteNonQuery();

                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created Characters Table.");

                        command.CommandText = "CREATE TABLE Rooms (ID integer NOT NULL PRIMARY KEY AUTOINCREMENT, RoomName text NOT NULL, RoomArea text NOT NULL, RoomDescription text NOT NULL, RoomExits text NOT NULL, RoomNorth integer, RoomSouth integer, RoomWest integer, RoomEast integer, RoomUp integer, RoomDown Integer, RoomTile integer NOT NULL DEFAULT 1)";
                        command.ExecuteNonQuery();

                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created Rooms Table.");

                        command.CommandText = "CREATE TABLE Items (ID integer NOT NULL PRIMARY KEY AUTOINCREMENT, ItemName text NOT NULL, ItemType text NOT NULL, RoomDescription text NOT NULL, RoomExits text NOT NULL, RoomNorth integer, RoomSouth integer, RoomWest integer, RoomEast integer, RoomUp integer, RoomDown Integer, RoomTile integer NOT NULL DEFAULT 1)";
                        command.ExecuteNonQuery();

                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created Items Table.");

                        command.CommandText = "CREATE TABLE Entities (ID integer NOT NULL PRIMARY KEY AUTOINCREMENT, ItemName text NOT NULL, ItemType text NOT NULL, RoomDescription text NOT NULL, RoomExits text NOT NULL, RoomNorth integer, RoomSouth integer, RoomWest integer, RoomEast integer, RoomUp integer, RoomDown Integer, RoomTile integer NOT NULL DEFAULT 1)";
                        command.ExecuteNonQuery();

                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created Entities Table.");
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
