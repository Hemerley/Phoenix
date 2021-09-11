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
                        // Create Accounts
                        command.CommandText = "";
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created Accounts Table.");

                        // Create Characters
                        command.CommandText = "";
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created Characters Table.");

                        // Create Abilties
                        command.CommandText = "";
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created Abilites Table.");

                        // Create Items
                        command.CommandText = "";
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created Items Table.");

                        // Create Entities
                        command.CommandText = "";
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created Entities Table.");

                        // Create Rooms
                        command.CommandText = "";
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created Rooms Table.");

                        // Create CharacterItem
                        command.CommandText = "";
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created CharacterItems Table.");

                        // Create RoomEntities
                        command.CommandText = "";
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created RoomEntities Table.");

                        // Create RoomItems
                        command.CommandText = "";
                        command.ExecuteNonQuery();
                        // Log Table Creation
                        Logger.ConsoleLog("Database", "Created RoomItems Table.");

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
