namespace Famicom.TableController
{
    public class LinakTableController : ITableController
    {
        public LinakTableController(ITable table)
        {
            Table = table;
        }
        public ITable Table { get; private set;}
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
        public int GetSitStandCounter()
        {
            throw new NotImplementedException();
        }
    }
}