using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TableController;

namespace Famicom.Models
{
    public class CleanerModel
    {
        private readonly ITableController tableController;

        public CleanerModel(ITableController tableController)
        {
            this.tableController = tableController;
        }

        public async Task UpdateAllTablesMaxHeight(bool isMaxHeight)
        {
            try
            {
                // Retrieve all table GUIDs
                var tableGuids = await tableController.GetAllTableIds();

                if (tableGuids.Length == 0)
                {
                    Console.WriteLine("No tables found in the database.");
                    return;
                }

                int targetHeight = isMaxHeight ? 1320 : 1000;

                foreach (var guid in tableGuids)
                {
                    Console.WriteLine($"Setting table {guid} to height {targetHeight}mm.");

                    try
                    {
                        await tableController.SetTableHeight(targetHeight, guid, new Progress<ITableStatusReport>());
                        Console.WriteLine($"Successfully updated table {guid}.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to update table {guid}: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating tables: {ex.Message}");
            }
        }
    }
}
