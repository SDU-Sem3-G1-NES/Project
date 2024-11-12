using SharedModels;

namespace TableController
{
    public interface ITableController
    {
        public Task<string[]> GetAllTableIds();
        public Task<LinakTable> GetFullTableInfo(string guid);
        public Task<int> GetTableHeight(string guid);
        public Task SetTableHeight(int height, string guid);
        public Task<int> GetTableSpeed(string guid);
        public Task<string> GetTableStatus(string guid);
        public Task GetTableError(string guid);
        public Task<int> GetActivationCounter(string guid);
        public Task GetSitStandCounter(string guid);
    }
}