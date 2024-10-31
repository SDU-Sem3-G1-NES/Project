using System;

namespace Famicom.Scheduling
{
    public interface ISchedule
    {
        public string GUID { get; private set; }
        public DateTime TimeStamp { get; private set; }
        public float Height { get; private set; }
        public void GetFromDb();
        public void DeleteFromDb();
    }
}
