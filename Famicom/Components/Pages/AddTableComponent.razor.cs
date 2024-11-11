using Microsoft.AspNetCore.Components;
using Models.Services;
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

        [Parameter]
        public EventCallback OnTableAdded { get; set; }


        private async Task AddTable()
        {
            if (string.IsNullOrEmpty(TableGuid) || string.IsNullOrEmpty(TableName) || string.IsNullOrEmpty(TableManufacturer))
            {
                ErrorMessage = "Please fill in all fields.";
                return;
            }

            tableService.AddTable(TableGuid, TableName, TableManufacturer, null);
            ErrorMessage = null; 

            await OnTableAdded.InvokeAsync(null); 
        }
    }
}
