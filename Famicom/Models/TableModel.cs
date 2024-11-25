using SharedModels;
using Models.Services;

namespace Famicom.Models
{
    public class TableModel
    {
        private readonly TableService tableService;
        private ITable? table;

        public TableModel()
        {
            this.tableService = new TableService();
        }

        // Method to retrieve a table based on a user ID
        public ITable? GetTable(int userId)
        {
            var tables = tableService.GetTablesUser(userId);
            if (tables != null && tables.Count > 0)
            {
                this.table = tables[0];
            }
            return this.table;
        }

        public void UpdateAllTablesMaxHeight(bool isMaxHeight)
        {
            string state = isMaxHeight ? "maximum height" : "normal height";

            Console.WriteLine($"Updating all tables to {state} in the database.");
        }
    }
}
