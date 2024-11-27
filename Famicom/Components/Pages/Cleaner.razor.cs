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

        private CleanerModel cleanerModel { get; set; } = null!;

        public async Task ToggleCleaningMode()
        {
            IsCleaningMode = !IsCleaningMode;
            if (IsCleaningMode)
            {
                await cleanerModel.UpdateAllTablesMaxHeight(true);
            }
        }
    }
}
