using SharedModels;

namespace Famicom.TableController
{
    public class LinakTableController : ITableController
    {
        public LinakTableController(LinakTable table)
        {
            Table = table;
        }
        public LinakTable Table { get; private set;}
        public List<String> GetAllTables()
        {
            throw new NotImplementedException();
        }
        public int GetTableHeight()
        {
            throw new NotImplementedException();
        }
        public void SetTableHeight()
        {
            throw new NotImplementedException();
        }
        public float GetTableSpeed()
        {
            throw new NotImplementedException();
        }
        public string GetTableStatus()
        {
            throw new NotImplementedException();
        }
        public void GetTableError()
        {
            throw new NotImplementedException();
        }
        public List<ITableError>? ErrorList { get; private set; }
        public event EventHandler TableCollisionDetected;
        protected virtual void OnTableCollisionDetected()
        {
            TableCollisionDetected?.Invoke(this, EventArgs.Empty);
        }
        public int GetActivationCounter()
        {
            throw new NotImplementedException();
        }
        public void GetSitStandCounter()
        {
            throw new NotImplementedException();
        }
    }
}