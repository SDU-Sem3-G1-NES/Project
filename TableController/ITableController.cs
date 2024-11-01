using SharedModels;

namespace Famicom.TableController
{
    public interface ITableController
    {
        public int GetTableHeight();
        public void SetTableHeight(int height);
        public int GetTableSpeed();
        public string GetTableStatus();
        public void GetTableError();
        public List<ITableError>? ErrorList { get; }
        public int GetActivationCounter();
        public void GetSitStandCounter();
    }
}