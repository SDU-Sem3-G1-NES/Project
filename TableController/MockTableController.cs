using System.Diagnostics;
namespace Famicom.TableController
{
    public class MockTableController : ITableController
    {
        public float GetTableHeight(ITable Table)
        {
            Debug.WriteLine("MockTableController.GetTableHeight");
            return 0.0f;
        }
        public void SetTableHeight(ITable Table)
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
        public List<ITableError> GetTableError()
        {
            Debug.WriteLine("MockTableController.GetTableError");
            return new List<ITableError>();
        }
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