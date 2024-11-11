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


        public async Task AddTable()
        {
            if(TableGuid != null && TableName != null && TableManufacturer != null)
            {
                tableService.AddTable(TableGuid, TableName, TableManufacturer, null);
            }
            else
            {
                ErrorMessage = "Please fill in all fields.";
            }

            await OnTableAdded.InvokeAsync(null);
        }
    }
}
