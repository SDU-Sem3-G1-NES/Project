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

        public List<ITable> GetTableList()
        {
            // Logic to get shit from the backend WOULD BE here, for now, you just mock. -N
            var linakTable = new LinakTable(
                "cd:fb:1a:53:fb:e6",
                "DESK 4486"
            );
            tableList.Add(linakTable);

            var linakTable2 = new LinakTable(
                "cd:ft:1r:23:fb:e6",
                "Ironman"
                );
            tableList.Add(linakTable2);

            var linakTable3 = new LinakTable(
                "ct:ht:1r:23:fb:t2",
                "Hulk"
                );
            tableList.Add(linakTable3);



            return this.tableList;
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


    
