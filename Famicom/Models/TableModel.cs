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
        private readonly HttpClient client;
        private readonly Progress<ITableStatusReport> progress;
        private ITable? table;

        
        public TableModel(IHttpClientFactory clientFactory, TableControllerService tableControllerService)
        {
            this.tableService = new TableService();
            this.TableControllerService = tableControllerService;
            this.ClientFactory = clientFactory;
            client = ClientFactory!.CreateClient("default");
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
            var tableController = await TableControllerService.GetTableController(tableGUID, client);
            return await tableController.GetTableHeight(tableGUID);
        }
        public async Task SetTableHeight(int tableHeight, string tableGUID, IProgress<ITableStatusReport> progress)
        {
            var tableController = await TableControllerService.GetTableController(tableGUID, client);

            await tableController.SetTableHeight(tableHeight, tableGUID, progress);
        }
    }
}