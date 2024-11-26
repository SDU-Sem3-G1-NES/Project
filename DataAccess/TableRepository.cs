using SharedModels;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json.Nodes;

namespace DataAccess
{
    public class TableRepository
    {
        private readonly DbAccess dbAccess;

        public TableRepository()
        {
            dbAccess = new DbAccess();
        }

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

        public void InsertTableUser(int userId, string tableId)
        {
            var sql = "INSERT INTO user_tables (u_id, t_guid) VALUES (@userId, @tableId)";
            dbAccess.ExecuteNonQuery(sql, ("@userId", userId), ("@tableId", tableId));
        }

        #endregion

        #region Edit Methods

        public void EditTableName(string tableId, string tableName)
        {
            var sql = "UPDATE tables SET t_name = @tableName WHERE t_guid = @tableId";
            dbAccess.ExecuteNonQuery(sql, ("@tableName", tableName), ("@tableId", tableId));
        }

        public void EditTableManufacturer(string tableId, string tableManufacturer)
        {
            var sql = "UPDATE tables SET t_manufacturer = @tableManufacturer WHERE t_guid = @tableId";
            dbAccess.ExecuteNonQuery(sql, ("@tableManufacturer", tableManufacturer), ("@tableId", tableId));
        }

        public void EditTableAPI(string tableId, int tableApi)
        {
            var sql = "UPDATE tables SET t_api = @tableApi WHERE t_guid = @tableId";
            dbAccess.ExecuteNonQuery(sql, ("@tableApi", tableApi), ("@tableId", tableId));
        }

        #endregion

        #region Delete Methods

        public void DeleteTable(string id)
        {
            var sql = "DELETE FROM tables WHERE t_guid = @id";
            dbAccess.ExecuteNonQuery(sql, ("@id", id));
        }

        #endregion

        #region Get Methods

        public List<ITable> GetTablesUser(int userId)
        {
            var sql = $"SELECT t.t_guid, t.t_name FROM user_tables AS ut INNER JOIN tables AS t ON ut.t_guid = t.t_guid WHERE ut.u_id = {userId}";
            List<ITable> tables = new List<ITable>();

            try
            {
                using (var cmd = dbAccess.dbDataSource.CreateCommand(sql))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ITable table = new LinakTable(reader.GetString(0), reader.GetString(1));
                            tables.Add(table);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error retrieving tables for user {userId}: {ex.Message}");
            }
            return tables;
        }

        public List<ITable> GetAllTables()
        {
            var sql = "SELECT t_guid, t_name FROM tables";
            List<ITable> tables = new List<ITable>();

            try
            {
                using (var cmd = dbAccess.dbDataSource.CreateCommand(sql))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ITable table = new LinakTable(reader.GetString(0), reader.GetString(1));
                            tables.Add(table);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error retrieving all tables: {ex.Message}");
            }
            return tables;
        }

        /// <summary>
        /// Updates the height of a specific table.
        /// </summary>
        /// <param name="tableId">The ID of the table to update.</param>
        /// <param name="height">The target height in millimeters.</param>
        public void UpdateTableHeight(string tableId, int height)
        {
            var sql = "UPDATE tables SET t_height = @height WHERE t_guid = @tableId";

            try
            {
                dbAccess.ExecuteNonQuery(sql, ("@height", height), ("@tableId", tableId));
                Console.WriteLine($"Table {tableId} height updated to {height}mm.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating table height for {tableId}: {ex.Message}");
            }
        }

        #endregion
    }
}
