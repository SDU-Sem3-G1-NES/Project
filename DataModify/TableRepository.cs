using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
