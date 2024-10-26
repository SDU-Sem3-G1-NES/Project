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
        
    }    
}