﻿using SharedModels;

namespace TableController
{
    public interface ITableController
    {
        public Task<string[]> GetAllTableIds();
        public Task<LinakTable> GetFullTableInfo(string guid);
        public Task<int> GetTableHeight(string guid);
        public Task SetTableHeight(int height, string guid, IProgress<ITableStatusReport> progress);
        public Task<int> GetTableSpeed(string guid);
        public Task<string> GetTableStatus(string guid);
        public Task<ITableError[]> GetTableError(string guid);

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

    public interface ITableStatusReport
    {
        public string guid { get; set; }
        public TableStatus Status { get; set; }
        public string Message { get; set; }
    }

    public interface ITableError
    {
        public string guid { get; set; }
        public int TimeSinceError { get; set; }
        public int ErrorCode { get; set; }
        public string Message { get; set; }
    }
}