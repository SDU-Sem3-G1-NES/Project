using SharedModels;
using Models.Services;

namespace Famicom.Models
{
    public class TableModel
    {
        private readonly TableService tableService;
        private ITable? table;
        //For testing purposes only
        private List<ITable> tableList = new();

        public TableModel()
        {
            this.tableService = new TableService();
        }


        public ITable? GetTable(int userId)
        {
            var tables = tableService.GetTablesUser(userId);
            if (tables != null && tables.Count > 0)
            {
                this.table = tables[0];
            }
            return this.table;
        }
    }
}


    
