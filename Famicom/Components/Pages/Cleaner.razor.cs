using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Famicom.Models;
using Models.Services;
using TableController;

namespace Famicom.Components.Pages
{
    public partial class CleanerBase : ComponentBase
    {
        public bool IsCleaningMode { get; private set; }

        private CleanerService CleanerService { get; set; }
        private CleanerModel CleanerModel { get; set; }

        public CleanerBase()
        {
            var httpClient = new HttpClient();
            var tableControllerService = new TableControllerService(httpClient);
            var tableService = new TableService();
            CleanerService = new CleanerService(tableControllerService, tableService);
            CleanerModel = new CleanerModel(CleanerService);
        }

        public async Task ToggleCleaningMode()
        {
            IsCleaningMode = !IsCleaningMode;
            if (IsCleaningMode)
            {
                await CleanerModel.UpdateAllTablesMaxHeight();
            }
            await InvokeAsync(StateHasChanged);
        }
    }
}
