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
        public CleanerService(ITableControllerService tableControllerService, HttpClient httpClient, TableService tableService)
        {
            this.tableControllerService = tableControllerService;
            this.tableService = tableService;
            this._httpClient = httpClient;
        }

        public async Task UpdateAllTablesMaxHeight()
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

                try {
                    var tasks = new List<Task>();
                    foreach (var table in tables)
                    {
                        var _tableController = await tableControllerService.GetTableController(table.GUID, _httpClient);
                        tasks.Add(_tableController.SetTableHeight(1320, table.GUID, _progress));
                        Console.WriteLine($"Table {table.Name} has been updated.");
                    }

                    try
                    {
                        await Task.WhenAll(tasks);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Exception occurred while waiting for all tasks to complete: {ex.Message}");
                    }
                }
                catch(Exception ex) {
                    Debug.WriteLine(ex.Message);
                }

            }
            catch (Exception e)
            {
                await ReportErrorAsync(e.Message);
            
            }
        }
         public async Task RevertAllTables()
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

                var tasks = new List<Task>();
                foreach (var table in tables)
                {
                    var _tableController = await tableControllerService.GetTableController(table.GUID, _httpClient);
                    tasks.Add(_tableController.SetTableHeight(900, table.GUID, _progress));
                    Console.WriteLine($"Table {table.Name} has been updated.");
                }

                try
                {
                    await Task.WhenAll(tasks);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Exception occurred while waiting for all tasks to complete: {ex.Message}");
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