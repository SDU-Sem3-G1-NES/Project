using System.Diagnostics;
using SharedModels;

namespace TableController
{
    public class MockTableController : ITableController
    {
        public MockTableController(ITable table)
        {
            Table = table;
        }
        public ITable Table { get; private set; }
        public Task<int> GetTableHeight(string guid)
        {
            Debug.WriteLine("MockTableController.GetTableHeight");
            return Task.FromResult(15);
        }
        public Task SetTableHeight(int height, string guid)
        {
            Debug.WriteLine("MockTableController.SetTableHeight");
            return Task.CompletedTask;

        }
        public Task<int> GetTableSpeed(string guid)
        {
            Debug.WriteLine("MockTableController.GetTableSpeed");
            return Task.FromResult(0);
        }
        public Task<string> GetTableStatus(string guid)
        {
            Debug.WriteLine("MockTableController.GetTableStatus");
            return Task.FromResult("MockTableController.GetTableStatus");
        }
        public Task GetTableError(string guid)
        {
            Debug.WriteLine("MockTableController.GetTableError");
            return Task.CompletedTask;
        }
        public Task<int> GetActivationCounter(string guid)
        {
            Debug.WriteLine("MockTableController.GetActivationCounter");
            return Task.FromResult(0);
        }
        public Task GetSitStandCounter(string guid)
        {
            Debug.WriteLine("MockTableController.GetSitStandCounter");
            return Task.CompletedTask;
        }

        public Task<string[]> GetAllTableIds()
        {
            Debug.WriteLine("MockTableController.GetAllTableIds");
            return Task.FromResult(new string[] { "MockTableController.GetAllTableIds" });
        }

        public Task<LinakTable> GetFullTableInfo(string guid)
        {
            throw new NotImplementedException();
        }
    }    
}