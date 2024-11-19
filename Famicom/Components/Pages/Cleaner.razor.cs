using Microsoft.AspNetCore.Components;
using System;
using Famicom.Models;

namespace Famicom.Components.Pages
{
    public partial class CleanerBase : ComponentBase
    {
        public bool IsCleaningMode { get; private set; }
        public bool CanActivateCleaningMode => DateTime.Now.Hour >= 18;

        private TableModel tableModel = new TableModel();

        public void ToggleCleaningMode()
        {
            if (CanActivateCleaningMode)
            {
                IsCleaningMode = !IsCleaningMode;

                if (IsCleaningMode)
                {
                    SetAllTablesToMaxHeight();
                }
                else
                {
                    ResetTablesToNormalHeight();
                }
            }
        }

        private void SetAllTablesToMaxHeight()
        {
            try
            {
                tableModel.UpdateAllTablesMaxHeight(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting tables to max height: {ex.Message}");
            }
        }

        private void ResetTablesToNormalHeight()
        {
            try
            {
                tableModel.UpdateAllTablesMaxHeight(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error resetting tables: {ex.Message}");
            }
        }
    }
}
