using System;

namespace Scheduling
{
    public interface ISchedule
    {
        int GUID { get; set; } // Schedule ID needed for cancellation.
        string CronWhen { get; set; } // Cron expression to be used for task scheduling. With Hangfire? Bad naming?
        int Height { get; set; } // Table height to be set.
        void Set(int ScheduleGUID, string cronWhen, int TableGUID, int height);
        void Cancel(int GUID);
    }
}
