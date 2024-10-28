namespace Famicom.TableController
{
    public interface ITableController
    {
        float GetTableHeight(ITable Table);
        void SetTableHeight(ITable Table);
        float GetTableSpeed();
        string GetTableStatus();
        List<ITableError> GetTableError();
        int GetActivationCounter();
        int GetSitStandCounter();
    }
}