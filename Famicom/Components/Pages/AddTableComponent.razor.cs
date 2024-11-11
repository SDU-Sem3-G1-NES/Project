using Microsoft.AspNetCore.Components;
using Models.Services;

namespace Famicom.Components.Pages
{
    public partial class AddTableComponent : ComponentBase
    {
        TableService tableService = new TableService();
        private string? TableGuid { get; set; }
        private string? TableName { get; set; }
        private string? TableManufacturer { get; set; }

        private void AddTable()
        {
            
        }
    }
}
