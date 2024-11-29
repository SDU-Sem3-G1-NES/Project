using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Models.Services;
using SharedModels;
using TableController;

namespace Famicom.Models
{
    public class CleanerService
    {
        private readonly ITableControllerService tableControllerService;
        private readonly TableService tableService;

        private readonly HttpClient _httpClient;

        private readonly Progress<ITableStatusReport> _progress = new Progress<ITableStatusReport>(message =>
        {
            Debug.WriteLine(message);
        });
        public CleanerService(ITableControllerService tableControllerService, HttpClient httpClient)
        {
            this.tableControllerService = tableControllerService;
            this.tableService = new TableService();
            this._httpClient = httpClient;
        }

        public async Task UpdateAllTablesMaxHeight()
        {
            try
            {
                // Retrieve all tables from the database
                var tables = tableService.GetAllTables();
                foreach (var table in tables)
                {
                    var _tableController = await tableControllerService.GetTableController(table.GUID, _httpClient);
                    await _tableController.SetTableHeight(9999, table.GUID, _progress);
                }

                if (tables.Count == 0)
                {
                    await ReportErrorAsync("No tables found in the database.");
                    return;
                }
            }
            catch (Exception e)
            {
                await ReportErrorAsync(e.Message);
            }
        }

        private async Task ReportErrorAsync(string message)
        {
            //error reporting logic
            await Task.CompletedTask;
        }
    }
}