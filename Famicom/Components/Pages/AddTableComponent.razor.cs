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
        TableService tableService = new TableService();
        ApiService apiService = new ApiService();
        [Inject] IHttpClientFactory? ClientFactory { get; set; }

        [Inject] TableControllerService? TableControllerService { get; set; }

        private string? TableGuid { get; set; }
        private string? TableName { get; set; }
        private string? TableManufacturer { get; set; }
        private bool IsAddingTableVisible { get; set; }
        private string? ErrorMessage { get; set; }
        public required int TableApi { get; set; }
        public required string ApiName { get; set; }
        public required List<Apis> Apis { get; set; }

        private List<ITable>? tableinfo;
        public bool ManualAddition { get; set; } = false;

        public AddTableComponent()
        {
            IsAddingTableVisible = false;
            

        }

        [Inject]
        public ISnackbar Snackbar { get; set; } = default!;

        [Parameter]
        public EventCallback<bool> OnTableAdded { get; set; }

        protected override void OnInitialized()
        {
            Apis = apiService.GetAllApis();
        }

        public async Task OnTableChanged(LinakTable table)
        {
            TableGuid = table.GUID;
            TableName = table.Name;
            TableManufacturer = table.Manufacturer;

            await Task.CompletedTask;
        }

        private async Task OnTableApiChanged(string newValue)
        {
            ApiName = newValue;

            List<ITable>? tableinfo = new List<ITable>();
            try
            {
                var tableController = await TableControllerService!.GetTableControllerByApiName(ApiName, ClientFactory!.CreateClient("default"));
                if (tableController != null)
                {
                    var tableIds = await tableController.GetAllTableIds();
                    tableinfo = new List<ITable>();

                    int count = 0;
                    foreach (var tableId in tableIds)
                    {
                        tableinfo.Add(await tableController.GetFullTableInfo(tableId));
                        Debug.WriteLine("Table " + count + " added");

                    }
                    this.tableinfo = tableinfo;
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

            await Task.CompletedTask;
        }


        
        

        private async Task Cancel()
        {
            await OnTableAdded.InvokeAsync(true);
        }

        private void ShowAddUser()
        {
            IsAddingTableVisible = !IsAddingTableVisible;
        }

        private async Task AddTable()
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
                ShowAddUser(); // Show the add user question overlay
                Snackbar.Add("Table added successfully", Severity.Success);
                
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                ErrorMessage = "An error occurred while adding the table.";
                Snackbar.Add("An error occurred while adding the table", Severity.Error);
                return;
            }
            await OnTableAdded.InvokeAsync(false);


        }
    }
}
