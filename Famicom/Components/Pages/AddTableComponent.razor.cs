﻿using DataAccess;
using Microsoft.AspNetCore.Components;
using Models.Services;
using MudBlazor;
using SharedModels;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TableController;
using TableControllerApi.Controllers;

namespace Famicom.Components.Pages
{
    public partial class AddTableComponent : ComponentBase
    {
        ApiService apiService = new ApiService();
        [Inject] IHttpClientFactory? ClientFactory { get; set; }

        [Inject] TableControllerService? TableControllerService { get; set; }
        [Inject] TableService tableService {get; set; } = default!;

        private string? TableGuid { get; set; }
        private string? TableName { get; set; }
        private string? TableManufacturer { get; set; }
        private string? ErrorMessage { get; set; }
        public required int TableApi { get; set; }
        public required string ApiName { get; set; }
        public required List<Apis> Apis { get; set; }

        private List<ITable>? tableinfo { get; set; }

        private HashSet<ITable> SelectedTables { get; set; } = new HashSet<ITable>();
        private bool loading { get; set; } = false;
        private bool selected { get; set; } = false;
        public bool ManualAddition { get; set; } = false;

        private string searchString = "";


        [Inject]
        public ISnackbar Snackbar { get; set; } = default!;

        [Parameter]
        public EventCallback<bool> OnTableAdded { get; set; }

        protected override void OnInitialized()
        {
            Apis = apiService.GetAllApis();
        }

        public async Task OnTableChanged(ITable table)
        {
            TableGuid = table.GUID;
            TableName = table.Name;
            TableManufacturer = table.Manufacturer;

            await Task.CompletedTask;
        }

        private async Task OnTableApiChanged(string newValue)
        {
            ApiName = newValue;
            ITableController? tableController;
            switch (newValue)
            {
                case "Linak Simulator API V2":
                    TableApi = 1;
                    break;
                case "Mock API":
                    TableApi = 2;
                    break;
                case "Linak API":
                    TableApi = 3;
                    break;
            }
            if (tableinfo == null)
            {
                try
                {
                    loading = true;
                    List<ITable> getTables = new List<ITable>();
                    tableController = await TableControllerService!.GetTableControllerByApiName(ApiName, ClientFactory!.CreateClient("default"));
                    if (tableController != null)
                    {
                        var tableIds = await tableController.GetAllTableIds();

                        int count = 0;
                        List<ITable> tables = tableService.GetAllTables();
                        foreach (var tableId in tableIds)
                        {
                            count++;
                            if(tables.Any(x => x.GUID == tableId))
                            {
                                continue;
                            }
                            getTables.Add(await tableController.GetFullTableInfo(tableId));

                        }
                        tableinfo = getTables;
                    }
                    else
                    {
                        Debug.WriteLine("Table controller not found");
                        Snackbar.Add("Table controller not found", Severity.Error);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }

            selected = true;
            loading = false;
            await Task.CompletedTask;
        }





        private async Task Cancel()
        {
            await OnTableAdded.InvokeAsync(true);
        }


        private void AddManualy()
        {
            ManualAddition = !ManualAddition;
        }

        private async Task AddTable()
        {
            if (!ManualAddition)
            {
                if (SelectedTables == null || !SelectedTables.Any())
                {
                    ErrorMessage = "Please select at least one table.";
                    return;
                }

                try
                {   
                    foreach (var table in SelectedTables)
                    {
                        tableService.AddTable(table.GUID, table.Name, table.Manufacturer, TableApi);
                    }
                    AddManualy();
                    Snackbar.Add("Tables added successfully", Severity.Success);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    ErrorMessage = "An error occurred while adding the tables.";
                    Snackbar.Add("An error occurred while adding the tables", Severity.Error);
                    return;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(TableGuid) || string.IsNullOrEmpty(TableName) || string.IsNullOrEmpty(TableManufacturer))
                {
                    ErrorMessage = "Please fill in all fields.";
                    return;
                }

                try
                {
                    tableService.AddTable(TableGuid, TableName, TableManufacturer, TableApi);
                    ErrorMessage = null;
                    AddManualy();
                    Snackbar.Add("Table added successfully", Severity.Success);

                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    ErrorMessage = "An error occurred while adding the table.";
                    Snackbar.Add("An error occurred while adding the table", Severity.Error);
                    return;
                }
            }
            
            await OnTableAdded.InvokeAsync(false);


        }

        private bool FilterFunc(ITable element)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            if (element.GUID.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.Manufacturer.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }

    }
}