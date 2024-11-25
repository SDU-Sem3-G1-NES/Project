using Microsoft.AspNetCore.Components;
using System;
using Famicom.Models;

namespace Famicom.Components.Pages
{
    public partial class CleanerBase : ComponentBase
    {
        public bool IsCleaningMode { get; private set; }

        private TableModel tableModel = new TableModel();

        // Method to toggle cleaning mode
        public void ToggleCleaningMode()
        {
            IsCleaningMode = !IsCleaningMode;

            if (IsCleaningMode)
            {
                SetAllTablesToMaxHeight(); // Set all tables to max height when cleaning mode is activated
            }
            else
            {
                ResetTablesToNormalHeight(); // Reset tables to normal height when cleaning mode is deactivated
            }
        }

        // Method to set all tables to max height
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

        // Method to reset all tables to normal height
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
