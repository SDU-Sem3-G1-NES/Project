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
        public int GetTableHeight(string? guid = null)
        {
            Debug.WriteLine("MockTableController.GetTableHeight");
            return 15;
        }
        public void SetTableHeight(int height, string? guid = null)
        {
            Debug.WriteLine("MockTableController.SetTableHeight");

        }
        public int GetTableSpeed(string? guid = null)
        {
            Debug.WriteLine("MockTableController.GetTableSpeed");
            return 0;
        }
        public string GetTableStatus(string? guid = null)
        {
            Debug.WriteLine("MockTableController.GetTableStatus");
            return "MockTableController.GetTableStatus";
        }
        public void GetTableError(string? guid = null)
        {
            Debug.WriteLine("MockTableController.GetTableError");
        }
        public List<ITableError>? ErrorList { get; private set; }
        public int GetActivationCounter(string? guid = null)
        {
            Debug.WriteLine("MockTableController.GetActivationCounter");
            return 0;
        }
        public void GetSitStandCounter(string? guid = null)
        {
            Debug.WriteLine("MockTableController.GetSitStandCounter");
        }

        public string[] GetAllTableIds()
        {
            Debug.WriteLine("MockTableController.GetAllTableIds");
            return new string[] { "MockTableController.GetAllTableIds" };
        }
    }    
}