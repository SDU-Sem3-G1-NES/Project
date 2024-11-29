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

        private CleanerService? cleanerService { get; set; }
        private CleanerModel? cleanerModel { get; set; }

        [Inject] IHttpClientFactory? httpClientFactory { get; set; } 

        protected override void OnInitialized()
        {
            var tableControllerService = new TableControllerService();
            var client = httpClientFactory!.CreateClient("default");
            cleanerService = new CleanerService(tableControllerService, client);
            cleanerModel = new CleanerModel(cleanerService);
        }

        public async Task ToggleCleaningMode()
        {
            IsCleaningMode = !IsCleaningMode;
            if (IsCleaningMode)
            {
                await cleanerModel.UpdateAllTablesMaxHeight();
            }
            await InvokeAsync(StateHasChanged);
        }
    }
}
