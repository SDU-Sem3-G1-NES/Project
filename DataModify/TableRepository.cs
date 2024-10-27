using System;
using Famicom.Components.Classes;

namespace DataModify
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

        public void InsertTable(string name, string manufacturer, int api)
        {
            var sql = "INSERT INTO tables (t_name, t_manufacturer, t_api) VALUES (@name, @manufacturer, @api)";
            dbAccess.ExecuteNonQuery(sql, ("@name", name), ("@manufacturer", manufacturer), ("@api", api));
        }

        public void InsertTableUser(int tableId, int userId)
        {
            var sql = "INSERT INTO user_tables (t_id, u_id) VALUES (@tableId, @userId)";
            dbAccess.ExecuteNonQuery(sql, ("@tableId", tableId), ("@userId", userId));
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

        #endregion

        #region Get Methods

        public List<Tables> GetTablesUser(int userId)
        {
            var sql = $"SELECT * FROM user_tables ut INNER JOIN tables t ON ut.t_id = t.t_id WHERE ut.u_id = {userId}";

            List<Tables> tables = new List<Tables>();

            using (var cmd = dbAccess.dbDataSource.CreateCommand(sql))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Tables table = new Tables()
                        {
                            TableId = reader.GetInt32(0),
                            TableName = reader.GetString(1),
                            TableManufacturer = reader.GetString(2),
                            TableApi = reader.GetInt32(3)
                        };
                    }
                }
            }

            return tables;
        }

        public List<Tables> GetTablesRoom(int roomId)
        {
            var sql = $"SELECT t.* FROM room_tables rm INNER JOIN tables t ON rm.t_id = t.t_id WHERE r_id = {roomId}";

            List<Tables> tables = new List<Tables>();

            using (var cmd = dbAccess.dbDataSource.CreateCommand(sql))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Tables table = new Tables()
                        {
                            TableId = reader.GetInt32(0),
                            TableName = reader.GetString(1),
                            TableManufacturer = reader.GetString(2),
                            TableApi = reader.GetInt32(3)
                        };
                    }
                }
            }

            return tables;
        }

        #endregion
    }
}
