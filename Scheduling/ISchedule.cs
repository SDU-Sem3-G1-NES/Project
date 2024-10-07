using System;

namespace FamicomBackend
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
