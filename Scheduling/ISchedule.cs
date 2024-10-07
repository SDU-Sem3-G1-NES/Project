using System;

namespace Scheduling
{
    public interface ISchedule
    {
        int GUID { get; private set; }
        DateTime TimeStamp { get; private set; }
        int Height { get; private set; }
        void Set(int ScheduleGUID, DateTime timeStamp, int TableGUID, int height);
        void Cancel(int GUID);
    }
}
