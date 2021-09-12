using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Data.SQLite;
using System.IO;

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
            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = File.ReadAllText(file);
                command.ExecuteNonQuery();
                Logger.ConsoleLog("Database", $"Executed query from {file}.");
            }
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

                using (var connection = new SQLiteConnection(LoadConnectionString(connectionType)))
                {
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
        }
        private static void LoadDatabaseTable<T>(List<T> list, string table, string connectionType)
        {
            using (var connection = new SQLiteConnection(LoadConnectionString(connectionType)))
            {
                connection.Open();
                string query = $"SELECT * FROM {table}";
                using (var command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            for(int i = 0; i < reader.FieldCount; i++)
                            {
                                Logger.ConsoleLog("System", $"New Data: {reader[i]}");
                            }
                        }
                    }
                }
                connection.Close();
            }
        }

        public static List<Room> LoadRooms(string connectionType)
        {
            using (var connection = new SQLiteConnection(LoadConnectionString(connectionType)))
            {
                connection.Open();
                string query = $"SELECT  r.Id as RoomId, r.Name as RoomName, e.Id as EntityId, e.Name as EntityName FROM Rooms r LEFT OUTER JOIN RoomEntities re ON re.RoomId = r.Id LEFT OUTER JOIN Entities e ON e.Id = re.EntityId; ";
                using (var command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        List<RoomEntityDto> rawData = new();
                        while (reader.Read())
                        {
                            var roomEntityDto = new RoomEntityDto
                            {
                                RoomId = int.Parse(reader["RoomId"].ToString()),
                                RoomName = reader["RoomName"].ToString(),
                                EntityId = int.TryParse(reader["EntityId"]?.ToString(), out int entityId) ? entityId : (int?)null,
                                EntityName = reader["EntityName"]?.ToString()
                            };
                            rawData.Add(roomEntityDto);
                        }
                        return (from data in rawData
                                group data by new { data.RoomId, data.RoomName } into g
                                select new Room
                                {
                                    Id = g.Key.RoomId,
                                    Name = g.Key.RoomName,
                                    Entities = g.Where(e => e.EntityId.HasValue).ToList().Select(e => new Entity
                                    {
                                        Id = e.EntityId.Value,
                                        Name = e.EntityName
                                    }).ToList()
                                }).ToList();
                    }
                }
            }
        }

        #endregion

        /*
        Open Server
        Init Database
        Load Rooms From Database
        Load Rooms Enti


         */
    }
}
