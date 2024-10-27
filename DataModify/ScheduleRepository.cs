using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModify
{
    public class ScheduleRepository
    {
        private readonly DbAccess dbAccess;

        public ScheduleRepository()
        {
            dbAccess = new DbAccess();
        }
        // Constructor for testing.
        public ScheduleRepository(DbAccess dbAccess)
        {
            this.dbAccess = dbAccess;
        }

        #region Insert Methods

        public void InsertSchedule(string name, string config, string owner)
        {
            var sql = "INSERT INTO schedules (s_name, s_config, s_owner) VALUES (@name, @config, @owner)";
            dbAccess.ExecuteNonQuery(sql, ("@name", name), ("@config", config), ("@owner", owner));
        }

        public void InsertScheduleTable(int scheduleId, int tableId)
        {
            var sql = "INSERT INTO schedule_tables (s_id, t_id) VALUES (@scheduleId, @tableId)";
            dbAccess.ExecuteNonQuery(sql, ("@scheduleId", scheduleId), ("@tableId", tableId));
        }

        #endregion

        #region Edit Methods

        public void EditScheduleTable(int scheduleId, int tableId)
        {
            var sql = "UPDATE schedule_tables SET t_id = @tableId WHERE s_id = @scheduleId";
            dbAccess.ExecuteNonQuery(sql, ("@tableId", tableId), ("@scheduleId", scheduleId));
        }

        // 3 methods to update schedules
        public void EditScheduleName(int scheduleId, string scheduleName)
        {
            var sql = "UPDATE schedules SET s_name = @scheduleName WHERE s_id = @scheduleId";
            dbAccess.ExecuteNonQuery(sql, ("@scheduleName", scheduleName), ("@scheduleId", scheduleId));
        }

        public void EditScheduleConfig(int scheduleId, string scheduleConfig)
        {
            var sql = "UPDATE schedules SET s_config = @scheduleConfig WHERE s_id = @scheduleId";
            dbAccess.ExecuteNonQuery(sql, ("@scheduleConfig", scheduleConfig), ("@scheduleId", scheduleId));
        }

        public void EditScheduleOwner(int scheduleId, string scheduleOwner)
        {
            var sql = "UPDATE schedules SET s_owner = @scheduleOwner WHERE s_id = @scheduleId";
            dbAccess.ExecuteNonQuery(sql, ("@scheduleOwner", scheduleOwner), ("@scheduleId", scheduleId));
        }

        #endregion

        #region Delete Methods

        public void DeleteSchedule(int id)
        {
            var sql = "DELETE FROM schedules WHERE s_id = @id";
            dbAccess.ExecuteNonQuery(sql, ("@id", id));
        }

        public void DeleteScheduleTable(int scheduleId, int tableId)
        {
            var sql = "DELETE FROM schedule_tables WHERE s_id = @scheduleId AND t_id = @tableId";
            dbAccess.ExecuteNonQuery(sql, ("@scheduleId", scheduleId), ("@tableId", tableId));
        }

        #endregion
    }
}
