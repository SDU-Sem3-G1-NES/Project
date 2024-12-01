using SharedModels;
using Models.Services;
using TableController;
using System.Diagnostics;

namespace Famicom.Models
{
    public class TableModel
    {
        private readonly TableService tableService;
        private readonly TableControllerService TableControllerService;
        private readonly IHttpClientFactory? ClientFactory;
        private readonly Progress<ITableStatusReport> progress;
        private ITable? table;

        
        public TableModel(IHttpClientFactory clientFactory)
        {
            this.tableService = new TableService();
            this.TableControllerService = new TableControllerService();
            this.ClientFactory = clientFactory;
            this.progress = new Progress<ITableStatusReport>(message =>
            {
                Debug.WriteLine(message);
            });
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
        public async Task<int> GetTableHeight(string tableGUID)
        {
            var tableController = await TableControllerService.GetTableController(tableGUID, ClientFactory!.CreateClient("default"));
            return await tableController.GetTableHeight(tableGUID);
        }
        public async Task<ITableStatusReport> SetTableHeight(int tableHeight, string tableGUID)
        {
            var tableController = await TableControllerService.GetTableController(tableGUID, ClientFactory!.CreateClient("default"));

            var tcs = new TaskCompletionSource<ITableStatusReport>();

            var progress = new Progress<ITableStatusReport>(message =>
            {
                Debug.WriteLine(message);
                tcs.TrySetResult(message);
            });

            await tableController.SetTableHeight(tableHeight, tableGUID, progress);
            
            return await tcs.Task;
        }
    }
}