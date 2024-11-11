using SharedModels;
using System.Diagnostics;

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
            var sql = $"SELECT t.t_guid,t.t_name FROM user_tables AS ut INNER JOIN tables AS t ON ut.t_guid = t.t_guid WHERE ut.u_id = {userId}";
            List<ITable> tables = new List<ITable>();

            try
            {
                using (var cmd = dbAccess.dbDataSource.CreateCommand(sql))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ITable table = new LinakTable(
                                reader.GetString(0),
                                reader.GetString(1)
                                );
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

        public List<ITable> GetTablesRoom(int roomId)
        {
            var sql = $"SELECT t.t_guid,t.t_name FROM room_tables AS rm INNER JOIN tables AS t ON rm.t_guid = t.t_guid WHERE rm.r_id = {roomId}";
            List<ITable> roomTables = new List<ITable>();

            try
            {
                using (var cmd = dbAccess.dbDataSource.CreateCommand(sql))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ITable table = new LinakTable(
                                reader.GetString(0),
                                reader.GetString(1)
                                );
                              
                            
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
                            ITable table = new LinakTable(
                                reader.GetString(0),
                                reader.GetString(1)
                                );
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

        #endregion

    }
}
