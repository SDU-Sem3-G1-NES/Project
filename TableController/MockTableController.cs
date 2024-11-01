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
        public int GetTableHeight()
        {
            Debug.WriteLine("MockTableController.GetTableHeight");
            return 15;
        }
        public void SetTableHeight(int height)
        {
            Debug.WriteLine("MockTableController.SetTableHeight");

        }
        public int GetTableSpeed()
        {
            Debug.WriteLine("MockTableController.GetTableSpeed");
            return 0;
        }
        public string GetTableStatus()
        {
            Debug.WriteLine("MockTableController.GetTableStatus");
            return "MockTableController.GetTableStatus";
        }
        public void GetTableError()
        {
            Debug.WriteLine("MockTableController.GetTableError");
        }
        public List<ITableError>? ErrorList { get; private set; }
        public int GetActivationCounter()
        {
            Debug.WriteLine("MockTableController.GetActivationCounter");
            return 0;
        }
        public void GetSitStandCounter()
        {
            Debug.WriteLine("MockTableController.GetSitStandCounter");
        }
        
    }    
}