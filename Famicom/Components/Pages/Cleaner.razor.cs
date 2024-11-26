using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using Famicom.Models;
using SharedModels;
using TableController;

namespace Famicom.Components.Pages
{
    public partial class CleanerBase : ComponentBase
    {
        public bool IsCleaningMode { get; private set; }

        private CleanerModel? cleanerModel;

        protected CleanerModel? CleanerModel { get; set; }

        [Inject]
        public ITableController? TableController { get; set; }

        protected override void OnInitialized()
        {
            if (TableController == null)
            {
                throw new InvalidOperationException("TableController is not initialized.");
            }
            cleanerModel = new CleanerModel(TableController);
            CleanerModel = cleanerModel;
        }
        public async Task ToggleCleaningMode()
        {
            IsCleaningMode = !IsCleaningMode;

            if (IsCleaningMode)
            {
                await SetAllTablesToMaxHeight();
            }
            else
            {
                await ResetTablesToNormalHeight();
            }
        }

        private async Task SetAllTablesToMaxHeight()
        {
            if (cleanerModel != null)
            {
                await cleanerModel.UpdateAllTablesMaxHeight(true);
            }
        }

        private async Task ResetTablesToNormalHeight()
        {
            if (cleanerModel != null)
            {
                await cleanerModel.UpdateAllTablesMaxHeight(false);
            }
        }
    }
}
