using System.Runtime.CompilerServices;
using SharedModels;

namespace TableController
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
        public int GetTableHeight(string? guid = null)
        {
            throw new NotImplementedException();
        }
        public void SetTableHeight(int height, string? guid = null)
        {
            throw new NotImplementedException();
        }
        public int GetTableSpeed(string? guid = null)
        {
            throw new NotImplementedException();
        }
        public string GetTableStatus(string? guid = null)
        {
            throw new NotImplementedException();
        }
        public void GetTableError(string? guid = null)
        {
            throw new NotImplementedException();
        }
        public List<ITableError>? ErrorList { get; private set; }

        public int GetActivationCounter(string? guid = null)
        {
            throw new NotImplementedException();
        }
        public void GetSitStandCounter(string? guid = null)
        {
            throw new NotImplementedException();
        }
    }
}