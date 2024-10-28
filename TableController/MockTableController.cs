using System.Diagnostics;
namespace Famicom.TableController
{
    public class MockTableController : ITableController
    {
        public ITable Table { get; private set; }
        public int GetTableHeight()
        {
            Debug.WriteLine("MockTableController.GetTableHeight");
            return 15;
        }
        public void SetTableHeight()
        {
            Debug.WriteLine("MockTableController.SetTableHeight");

        }
        public float GetTableSpeed()
        {
            Debug.WriteLine("MockTableController.GetTableSpeed");
            return 0.0f;
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
        public int GetSitStandCounter()
        {
            Debug.WriteLine("MockTableController.GetSitStandCounter");
            return 0;
        }
        
    }    
}