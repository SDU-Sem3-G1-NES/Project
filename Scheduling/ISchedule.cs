using System;

namespace Famicom.Scheduling
{
    public interface ISchedule
    {
        int GUID { get; private set; }
        DateTime TimeStamp { get; private set; }
        float Height { get; private set; }
        void GetFromDb();
        void DeleteFromDb();
    }
}
