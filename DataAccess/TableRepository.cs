using System;
using System.Diagnostics;
using Famicom.Components.Classes;

namespace DataAccess
{
    public class TableRepository
    {
        private readonly DbAccess dbAccess;

        public TableRepository()
        {
            
            dbAccess = new DbAccess();
        }
        // Constructor for testing.
        public TableRepository(DbAccess dbAccess)
        {
            this.dbAccess = dbAccess;
        }
        #region Insert Methods

        public void InsertTable(string address, string name, string manufacturer, int api)
        {
            var sql = "INSERT INTO tables (t_addr, t_name, t_manufacturer, t_api) VALUES (@address, @name, @manufacturer, @api)";
            dbAccess.ExecuteNonQuery(sql, ("@address", address), ("@name", name), ("@manufacturer", manufacturer), ("@api", api));
        }

        public void InsertTableUser(int userId ,int tableId)
        {
            var sql = "INSERT INTO user_tables (u_id, t_id) VALUES (@userId, @tableId)";
            dbAccess.ExecuteNonQuery(sql, ("@userId", userId), ("@tableId", tableId));
        }

        #endregion

        #region Edit Methods

        public void EditTableName(int tableId, string tableName)
        {
            var sql = "UPDATE tables SET t_name = @tableName WHERE t_id = @tableId";
            dbAccess.ExecuteNonQuery(sql, ("@tableName", tableName), ("@tableId", tableId));
        }

        public void EditTableManufacturer(int tableId, string tableManufacturer)
        {
            var sql = "UPDATE tables SET t_manufacturer = @tableManufacturer WHERE t_id = @tableId";
            dbAccess.ExecuteNonQuery(sql, ("@tableManufacturer", tableManufacturer), ("@tableId", tableId));
        }

        public void EditTableAPI(int tableId, int tableApi)
        {
            var sql = "UPDATE tables SET t_api = @tableApi WHERE t_id = @tableId";
            dbAccess.ExecuteNonQuery(sql, ("@tableApi", tableApi), ("@tableId", tableId));
        }

        #endregion

        #region Delete Methods

        public void DeleteTable(int id)
        {
            var sql = "DELETE FROM tables WHERE t_id = @id";
            dbAccess.ExecuteNonQuery(sql, ("@id", id));
        }

        public List<Tables> GetTablesUser(int userId)
        {
            var sql = $"SELECT t.* FROM user_tables AS ut INNER JOIN tables AS t ON ut.t_id = t.t_id WHERE ut.u_id = {userId}";

            List<Tables> tables = new List<Tables>();

            try
            {
                using (var cmd = dbAccess.dbDataSource.CreateCommand(sql))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Tables table = new Tables()
                            {
                                TableId = reader.GetInt32(0),
                                TableAddress = reader.GetString(1),
                                TableName = reader.GetString(2),
                                TableManufacturer = reader.GetString(3),
                                TableApi = reader.GetInt32(4)
                            };
                            tables.Add(table);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
                Debug.WriteLine($"An error occurred while executing the SQL query: {ex.Message}");
                
            }

            return tables;
        }

        public List<Tables> GetTablesRoom(int roomId)
        {
            var sql = $"SELECT t.* FROM room_tables AS rm INNER JOIN tables AS t ON rm.t_id = t.t_id WHERE rm.r_id = {roomId}";

            List<Tables> roomTables = new List<Tables>();

            try
            {
                using (var cmd = dbAccess.dbDataSource.CreateCommand(sql))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Tables table = new Tables()
                            {
                                TableId = reader.GetInt32(0),
                                TableAddress = reader.GetString(1),
                                TableName = reader.GetString(2),
                                TableManufacturer = reader.GetString(3),
                                TableApi = reader.GetInt32(4)
                            };
                            roomTables.Add(table);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
                Debug.WriteLine($"An error occurred while executing the SQL query: {ex.Message}");
            }

            return roomTables;
        }

        #endregion
    }
}
