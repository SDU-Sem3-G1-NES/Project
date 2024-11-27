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

        private readonly Progress<ITableStatusReport> _progress = new Progress<ITableStatusReport>(message =>
        {
            Debug.WriteLine(message);
        });
        public CleanerService(ITableControllerService tableControllerService, TableService tableService)
        {
            this.tableControllerService = tableControllerService;
            this.tableService = tableService;
        }

        public async Task UpdateAllTablesMaxHeight()
        {
            try
            {
                // Retrieve all tables from the database
                var tables = tableService.GetAllTables();
                foreach (var table in tables)
                {
                    var _tableController = await tableControllerService.GetTableController(table.GUID);
                    await _tableController.SetTableHeight(table.Height ?? default(int), table.GUID, _progress);
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