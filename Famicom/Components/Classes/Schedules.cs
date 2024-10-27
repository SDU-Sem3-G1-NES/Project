namespace Famicom.Components.Classes
{
    public class Schedules
    {
        public int ScheduleId { get; set; }
        public string ScheduleName { get; set; }
        public string ScheduleConfig { get; set; }
        public string TableName { get; set; }

        public Schedules(int scheduleId, string scheduleName, string scheduleConfig, string tableName)
        {
            ScheduleId = scheduleId;
            ScheduleName = scheduleName;
            ScheduleConfig = scheduleConfig;
            TableName = tableName;
        }

        public Schedules()
        {
        }
    }
}
