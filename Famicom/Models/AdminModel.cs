using SharedModels;
using Models.Services;
using System.Collections.Generic;

namespace Famicom.Models
{
    public class AdminModel
    {
        private readonly TableService tableService;

        public AdminModel()
        {
            this.tableService = new TableService();
        }

        public List<ITable> GetAllTables()
        {
            var tables = tableService.GetAllTables();
            return tables ?? new List<ITable>();
        }
    }
}
