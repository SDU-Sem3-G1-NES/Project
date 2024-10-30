namespace Famicom.TableController
{
    public interface ITableController
    {
        ITable Table { get;}
        int GetTableHeight();
        void SetTableHeight();
        float GetTableSpeed();
        string GetTableStatus();
        void GetTableError();
        public List<ITableError>? ErrorList { get;}
    }
}