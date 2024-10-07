using System;

namespace Scheduling
{
    public interface ISchedule //Hangfire seems good for this
    {
        int GUID { get; private set; }
        DateTime TimeStamp { get; private set; }
        int Height { get; private set; }
        void GetFromDb(int ScheduleGUID, DateTime timeStamp, int[] TableGUID, int height);
        void DeleteFromDb(int ScheduleGUID);
    }
}
