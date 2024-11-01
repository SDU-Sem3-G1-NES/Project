using SharedModels;

namespace TableController
{
    public interface ITableController
    {
        public int GetTableHeight(string? guid = null);
        public void SetTableHeight(int height, string? guid = null);
        public int GetTableSpeed(string? guid = null);
        public string GetTableStatus(string? guid = null);
        public void GetTableError(string? guid = null);
        public List<ITableError>? ErrorList { get; }
        public int GetActivationCounter(string? guid = null);
        public void GetSitStandCounter(string? guid = null);
    }
}