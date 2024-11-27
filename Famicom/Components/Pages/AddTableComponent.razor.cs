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
        AddTableComponentService addTableComponentService = new AddTableComponentService();

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

            tableinfo = await addTableComponentService.HandleApiRequest(ApiName);


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
