using SharedModels;

namespace TableController
{
    public interface ITableController
    {
        public Task<string[]> GetAllTableIds();
        public Task<LinakTable> GetFullTableInfo(string guid);
        public Task<int> GetTableHeight(string guid);
        public Task SetTableHeight(int height, string guid, IProgress<TableStatusReport> progress);
        public Task<int> GetTableSpeed(string guid);
        public Task<string> GetTableStatus(string guid);
        public Task GetTableError(string guid);

    }

    public enum TableStatus
    {
        Success,
        Collision,
        Overheat,
        Lost,
        Overload,
        OtherError
    }

    public class TableStatusReport
    {
        public string guid { get; set; }
        public TableStatus Status { get; set; }
        public string Message { get; set; }

        public TableStatusReport(string guid, TableStatus status, string message)
        {
            this.guid = guid;
            this.Status = status;
            this.Message = message;
        }
    }
}