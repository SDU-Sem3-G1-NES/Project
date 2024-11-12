using Microsoft.AspNetCore.Components;
using Models.Services;
using MudBlazor;
using System.Diagnostics;

namespace Famicom.Components.Pages
{
    public partial class AddTableComponent : ComponentBase
    {
        TableService tableService = new TableService();
        private string? TableGuid { get; set; }
        private string? TableName { get; set; }
        private string? TableManufacturer { get; set; }

        private string? ErrorMessage { get; set; }

        [Inject]
        public ISnackbar Snackbar { get; set; } = default!;

        [Parameter]
        public EventCallback OnTableAdded { get; set; }


        private async Task AddTable()
        {
            if (string.IsNullOrEmpty(TableGuid) || string.IsNullOrEmpty(TableName) || string.IsNullOrEmpty(TableManufacturer))
            {
                ErrorMessage = "Please fill in all fields.";
                return;
            }

            try
            {
                tableService.AddTable(TableGuid, TableName, TableManufacturer, 1);
                ErrorMessage = null;
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
