using System.Runtime.CompilerServices;
using SharedModels;

namespace TableController
{
    public class LinakTableController : ITableController
    {
        public LinakTableController()
        {
        }

        #pragma warning disable 67 // Disable warning for unused event
        public event EventHandler<TableHeightSetEventArgs>? OnTableHeightSet;
        #pragma warning restore 67 // Restore warning

        public Task<int> GetActivationCounter(string guid)
        {
            throw new NotImplementedException();
        }

        public Task<string[]> GetAllTableIds()
        {
            throw new NotImplementedException();
        }

        public Task<ITable> GetFullTableInfo(string guid)
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

        public Task SetTableHeight(int height, string guid, IProgress<ITableStatusReport> progress)
        {
            throw new NotImplementedException();
        }

        Task<ITableError[]> ITableController.GetTableError(string guid)
        {
            throw new NotImplementedException();
        }
    }
}