using SharedModels;
using System.Data;
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
        // Constructor for testing.
        public TableRepository(DbAccess dbAccess)
        {
            this.dbAccess = dbAccess;
        }

        #region Insert Methods

        public void InsertTable(string guid, string name, string manufacturer, int api)
        {
           string sql = "INSERT INTO tables (t_guid, t_name, t_manufacturer, t_api) VALUES (@guid ,@name, @manufacturer, @api)";
            dbAccess.ExecuteNonQuery(sql, ("@guid", guid), ("@name", name), ("@manufacturer", manufacturer), ("@api", api));
            
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

        public void DeleteTableUser(string tableId)
        {
            var sql = "DELETE FROM user_tables WHERE t_guid = @tableId";
            dbAccess.ExecuteNonQuery(sql, ("@tableId", tableId));
        }
        #endregion

        #region Get Methods
        public List<ITable> GetTablesUser(int userId)
        {
            var sql = $"SELECT t.t_guid,t.t_name, t_manufacturer FROM user_tables AS ut INNER JOIN tables AS t ON ut.t_guid = t.t_guid WHERE ut.u_id = {userId}";
            List<ITable> tables = new List<ITable>();

            try
            {
                using(var connection = dbAccess.dbDataSource.CreateConnection())
                {
                    connection.Open();
                    using (var cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = sql;
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ITable table = new LinakTable(
                                    reader.GetString(0),
                                    reader.GetString(1),
                                    reader.IsDBNull(2) ? null : reader.GetString(2)
                                    );
                                tables.Add(table);
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {

                Debug.WriteLine($"An error occurred while executing the SQL query: {ex.Message}");
            }
            return tables;
        }

        public List<ITable> GetUserFreeTable()
        {
            var sql = $"SELECT t.t_guid, t.t_name, t_manufacturer FROM tables AS t LEFT JOIN user_tables AS ut ON t.t_guid = ut.t_guid WHERE ut.t_guid IS NULL; ";
            List<ITable> tables = new List<ITable>();

            try
            {
                using(var connection = dbAccess.dbDataSource.CreateConnection())
                {
                    connection.Open();
                    using (var cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = sql;
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ITable table = new LinakTable(
                                    reader.GetString(0),
                                    reader.GetString(1),
                                    reader.IsDBNull(2) ? null : reader.GetString(2)
                                    );
                                tables.Add(table);
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {

                Debug.WriteLine($"An error occurred while executing the SQL query: {ex.Message}");
            }
            return tables;
        }

        public List<ITable> GetTablesRoom(int roomId)
        {
            var sql = $"SELECT t.t_guid,t.t_name,t_manufacturer FROM room_tables AS rm INNER JOIN tables AS t ON rm.t_guid = t.t_guid WHERE rm.r_id = {roomId}";
            List<ITable> roomTables = new List<ITable>();

            try
            {
                using(var connection = dbAccess.dbDataSource.CreateConnection())
                {
                    connection.Open();
                    using (var cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = sql;
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ITable table = new LinakTable(
                                    reader.GetString(0),
                                    reader.GetString(1),
                                    reader.IsDBNull(2) ? null : reader.GetString(2)
                                    );
                                
                                roomTables.Add(table);
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {

                Debug.WriteLine($"An error occurred while executing the SQL query: {ex.Message}");
            }

            return roomTables;
        }

        public List<ITable> GetAllTables()
        {
            var sql = "SELECT t_guid, t_name, t_manufacturer FROM tables";
            List<ITable> tables = new List<ITable>();

            try
            {
                using (var connection = dbAccess.dbDataSource.CreateConnection())
                {
                    connection.Open();
                    using (var cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = sql;
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ITable table = new LinakTable(
                                    reader.GetString(0),
                                    reader.GetString(1),
                                    reader.IsDBNull(2) ? null : reader.GetString(2)
                                );
                                tables.Add(table);
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred while executing the SQL query: {ex.Message}");
            }
            return tables;
        }

        public string? GetTableAPI(string tableId)
        {
            var sql = $"SELECT distinct a_config FROM apis INNER JOIN tables ON apis.a_id = tables.t_api WHERE tables.t_guid = '{tableId}'";
            try
            {
                using(var connection = dbAccess.dbDataSource.CreateConnection())
                {
                    connection.Open();
                    using (var cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = sql;
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var node =  JsonNode.Parse(reader.GetString(0));
                                if(node == null || !(node is JsonObject jsonObject) || !jsonObject.TryGetPropertyValue("controller", out JsonNode? controllerNode)) 
                                {
                                    connection.Close();
                                    return null;
                                }
                                else
                                {
                                    connection.Close();
                                    return controllerNode?.ToString();
                                } 
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred while executing the SQL query: {ex.Message}");
            }
            return null;
        }

        #endregion

    }
}
