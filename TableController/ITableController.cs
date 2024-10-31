using SharedModels;

namespace Famicom.TableController
{
    public interface ITableController
    {
        int GetTableHeight();
        void SetTableHeight();
        float GetTableSpeed();
        string GetTableStatus();
        void GetTableError();
        public List<ITableError>? ErrorList { get;}
        int GetActivationCounter();
        void GetSitStandCounter();
    }
}