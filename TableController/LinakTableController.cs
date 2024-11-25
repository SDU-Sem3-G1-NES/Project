using System.Runtime.CompilerServices;
using SharedModels;

namespace TableController
{
    public class LinakTableController : ITableController
    {
        public LinakTableController()
        {
        }

        public Task<int> GetActivationCounter(string guid)
        {
            throw new NotImplementedException();
        }

        public Task<string[]> GetAllTableIds()
        {
            throw new NotImplementedException();
        }

        public Task<LinakTable> GetFullTableInfo(string guid)
        {
            throw new NotImplementedException();
        }

        public Task GetSitStandCounter(string guid)
        {
            throw new NotImplementedException();
        }

        public Task GetTableError(string guid)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetTableHeight(string guid)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetTableSpeed(string guid)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetTableStatus(string guid)
        {
            throw new NotImplementedException();
        }

        public Task SetTableHeight(int height, string guid)
        {
            throw new NotImplementedException();
        }

        public Task SetTableHeight(int height, string guid, IProgress<TableStatusReport> progress)
        {
            throw new NotImplementedException();
        }
    }
}