using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Services;
using SharedModels;
using TableController;

namespace Famicom.Models
{
    public class CleanerModel
    {
        private readonly ITableControllerService tableControllerService;
        private readonly TableService tableService;

        public CleanerModel(ITableControllerService tableControllerService, TableService tableService)
        {
            this.tableControllerService = tableControllerService;
            this.tableService = tableService;
        }

        public async Task UpdateAllTablesMaxHeight(bool isMaxHeight)
        {
            try
            {
                // Retrieve all tables from the database
                var tables = tableService.GetAllTables();

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