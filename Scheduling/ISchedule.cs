using System;

namespace Famicom.Scheduling
{
    public interface ISchedule
    {
        public string GUID { get; set; }
        public DateTime TimeStamp { get; set; }
        public float Height { get; set; }
        public void GetFromDb();
        public void DeleteFromDb();
    }
}
