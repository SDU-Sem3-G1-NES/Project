using DataAccess;
using SharedModels;

namespace Models.Services
{
    public class TableService
    {
        private readonly TableRepository tableRepository;

        public TableService()
        {
            tableRepository = new TableRepository();
        }
        
        public void AddTable(string guid, string name, string manufacturer, int api)
        {
            tableRepository.InsertTable(guid, name, manufacturer, api);
        }

        public void AddTableUser(int userId, string tableId)
        {
            tableRepository.InsertTableUser(userId, tableId);
        }

        public void UpdateTableName(string tableId, string tableName)
        {
            tableRepository.EditTableName(tableId, tableName);
        }

        public void UpdateTableManufacturer(string tableId, string tableManufacturer)
        {
            tableRepository.EditTableManufacturer(tableId, tableManufacturer);
        }

        public void UpdateTableAPI(string tableId, int tableApi)
        {
            tableRepository.EditTableAPI(tableId, tableApi);
        }

        public void RemoveTable(string id)
        {
            tableRepository.DeleteTable(id);
        }

        public List<ITable> GetTablesUser(int userId)
        {
            return tableRepository.GetTablesUser(userId);
        }

        public List<ITable> GetTablesRoom(int roomId)
        {
            return tableRepository.GetTablesRoom(roomId);
        }

        public List<ITable> GetAllTables()
        {
            return tableRepository.GetAllTables();
        }

        public List<ITable> GetUserFreeTable()
        {
            return tableRepository.GetUserFreeTable();
        }


    }
}