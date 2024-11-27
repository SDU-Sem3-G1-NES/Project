using SharedModels;
using System.Diagnostics;


namespace Models.Services
{
    public class AddTableComponentService
    {
        private TableControllerService _tableControllerService = new TableControllerService();
        public AddTableComponentService()
        {
            
        }

        public async Task<List<ITable>> HandleApiRequest(string ApiName)
        {
            List<ITable>? tableinfo = new List<ITable>();
            try
            {
                var tableController = await _tableControllerService.GetTableControllerByApiName(ApiName);
                if (tableController != null)
                {
                    var tableIds = await tableController.GetAllTableIds();
                    tableinfo = new List<ITable>();

                    int count = 0;
                    foreach (var tableId in tableIds)
                    {
                        tableinfo.Add(await tableController.GetFullTableInfo(tableId));
                        Debug.WriteLine("Table " + count + " added");

                    }
                }
                else
                {
                    Debug.WriteLine("Table controller not found");
                    return tableinfo;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return tableinfo;
        }
    }
}
