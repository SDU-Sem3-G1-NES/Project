using Microsoft.AspNetCore.Components;
using Models.Services;
using MudBlazor;
using SharedModels;
using System.Diagnostics;

namespace Famicom.Components.Pages
{
    public partial class AddTableComponent : ComponentBase
    {
        TableService tableService = new TableService();
        ApiService apiService = new ApiService();
        private string? TableGuid { get; set; }
        private string? TableName { get; set; }
        private string? TableManufacturer { get; set; }
        private bool IsAddingTableVisible { get; set; }
        private string? ErrorMessage { get; set; }
        public required int TableApi { get; set; }
        public required List<Apis> Apis { get; set; }

        public AddTableComponent()
        {
            IsAddingTableVisible = false;
        }

        [Inject]
        public ISnackbar Snackbar { get; set; } = default!;

        [Parameter]
        public EventCallback OnTableAdded { get; set; }

        protected override void OnInitialized()
        {
            Apis = apiService.GetAllApis();
        }

        private async Task Cancel()
        {
            await OnTableAdded.InvokeAsync(null);
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
            await OnTableAdded.InvokeAsync(null);


        }
    }
}
