using DataAccess;
using SharedModels;

namespace Models.Services
{
    public class ScheduleService
    {
        private readonly ScheduleRepository scheduleRepository;

        public ScheduleService(ScheduleRepository scheduleRepository)
        {
            this.scheduleRepository = scheduleRepository;
        }

        public void AddSchedule(string name, string config, int owner)
        {
            scheduleRepository.InsertSchedule(name, config, owner);
        }

        public void AddScheduleTable(int scheduleId, string tableId)
        {
            scheduleRepository.InsertScheduleTable(scheduleId, tableId);
        }

        public void UpdateScheduleTable(int scheduleId, string tableId)
        {
            scheduleRepository.EditScheduleTable(scheduleId, tableId);
        }

        public void UpdateScheduleName(int scheduleId, string scheduleName)
        {
            scheduleRepository.EditScheduleName(scheduleId, scheduleName);
        }

        public void UpdateScheduleConfig(int scheduleId, string scheduleConfig)
        {
            scheduleRepository.EditScheduleConfig(scheduleId, scheduleConfig);
        }

        public void UpdateScheduleOwner(int scheduleId, string scheduleOwner)
        {
            scheduleRepository.EditScheduleOwner(scheduleId, scheduleOwner);
        }

        public void RemoveSchedule(int id)
        {
            scheduleRepository.DeleteSchedule(id);
        }

        public void RemoveScheduleTable(int scheduleId, string tableId)
        {
            scheduleRepository.DeleteScheduleTable(scheduleId, tableId);
        }

        public List<Schedules> GetSchedules(int userId)
        {
            return scheduleRepository.GetSchedules(userId);
        }
    }
}