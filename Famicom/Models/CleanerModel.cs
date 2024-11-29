using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Services;
using SharedModels;
using TableController;

namespace Famicom.Models
{
    public class CleanerModel
    {
        private readonly CleanerService cleanerService;

        public CleanerModel(CleanerService cleanerService)
        {
            this.cleanerService = cleanerService;
        }

        public async Task UpdateAllTablesMaxHeight()
        {
            await cleanerService.UpdateAllTablesMaxHeight();
        }

        public async Task RevertAllTables()
        {
            await cleanerService.RevertAllTables();
        }
    }
}
