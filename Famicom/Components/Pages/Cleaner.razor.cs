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

        private readonly CleanerModel cleanerModel;

        protected CleanerModel CleanerModel { get; set; }

        public CleanerBase(ITableController tableController)
        {
            cleanerModel = new CleanerModel(tableController);
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
            await cleanerModel.UpdateAllTablesMaxHeight(true);
        }

        private async Task ResetTablesToNormalHeight()
        {
            await cleanerModel.UpdateAllTablesMaxHeight(false);
        }
    }
}
