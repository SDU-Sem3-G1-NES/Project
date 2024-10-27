using Xunit;
using Moq;
using DataModify;

namespace DataModify.Tests
{
    public class ScheduleRepositoryTests
    {
        private readonly Mock<DbAccess> _dbAccessMock;
        private readonly ScheduleRepository _scheduleRepository;

        public ScheduleRepositoryTests()
        {
            _dbAccessMock = new Mock<DbAccess>();
            _scheduleRepository = new ScheduleRepository(_dbAccessMock.Object);
        }

        [Fact]
        public void InsertSchedule_ShouldExecuteNonQuery_WithCorrectParameters()
        {
            // Arrange
            string name = "Schedule1";
            string config = "ConfigData";
            string owner = "User1";

            // Act
            _scheduleRepository.InsertSchedule(name, config, owner);

            // Assert
            var param1 = ("@name", (object)name);
            var param2 = ("@config", (object)config);
            var param3 = ("@owner", (object)owner);

            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                "INSERT INTO schedules (s_name, s_config, s_owner) VALUES (@name, @config, @owner)",
                param1, param2, param3
            ), Times.Once);
        }

        [Fact]
        public void InsertScheduleTable_ShouldExecuteNonQuery_WithCorrectParameters()
        {
            // Arrange
            int scheduleId = 1;
            int tableId = 2;

            // Act
            _scheduleRepository.InsertScheduleTable(scheduleId, tableId);

            // Assert
            var param1 = ("@scheduleId", (object)scheduleId);
            var param2 = ("@tableId", (object)tableId);

            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                "INSERT INTO schedule_tables (s_id, t_id) VALUES (@scheduleId, @tableId)",
                param1, param2
            ), Times.Once);
        }

        [Fact]
        public void EditScheduleTable_ShouldExecuteNonQuery_WithCorrectParameters()
        {
            // Arrange
            int scheduleId = 1;
            int tableId = 3;

            // Act
            _scheduleRepository.EditScheduleTable(scheduleId, tableId);

            // Assert
            var param1 = ("@tableId", (object)tableId);
            var param2 = ("@scheduleId", (object)scheduleId);

            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                "UPDATE schedule_tables SET t_id = @tableId WHERE s_id = @scheduleId",
                param1, param2
            ), Times.Once);
        }

        [Fact]
        public void EditScheduleName_ShouldExecuteNonQuery_WithCorrectParameters()
        {
            // Arrange
            int scheduleId = 1;
            string scheduleName = "UpdatedSchedule";

            // Act
            _scheduleRepository.EditScheduleName(scheduleId, scheduleName);

            // Assert
            var param1 = ("@scheduleName", (object)scheduleName);
            var param2 = ("@scheduleId", (object)scheduleId);

            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                "UPDATE schedules SET s_name = @scheduleName WHERE s_id = @scheduleId",
                param1, param2
            ), Times.Once);
        }

        [Fact]
        public void EditScheduleConfig_ShouldExecuteNonQuery_WithCorrectParameters()
        {
            // Arrange
            int scheduleId = 1;
            string scheduleConfig = "UpdatedConfig";

            // Act
            _scheduleRepository.EditScheduleConfig(scheduleId, scheduleConfig);

            // Assert
            var param1 = ("@scheduleConfig", (object)scheduleConfig);
            var param2 = ("@scheduleId", (object)scheduleId);

            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                "UPDATE schedules SET s_config = @scheduleConfig WHERE s_id = @scheduleId",
                param1, param2
            ), Times.Once);
        }

        [Fact]
        public void EditScheduleOwner_ShouldExecuteNonQuery_WithCorrectParameters()
        {
            // Arrange
            int scheduleId = 1;
            string scheduleOwner = "NewOwner";

            // Act
            _scheduleRepository.EditScheduleOwner(scheduleId, scheduleOwner);

            // Assert
            var param1 = ("@scheduleOwner", (object)scheduleOwner);
            var param2 = ("@scheduleId", (object)scheduleId);

            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                "UPDATE schedules SET s_owner = @scheduleOwner WHERE s_id = @scheduleId",
                param1, param2
            ), Times.Once);
        }

        [Fact]
        public void DeleteSchedule_ShouldExecuteNonQuery_WithCorrectParameters()
        {
            // Arrange
            int scheduleId = 1;

            // Act
            _scheduleRepository.DeleteSchedule(scheduleId);

            // Assert
            var param1 = ("@id", (object)scheduleId);

            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                "DELETE FROM schedules WHERE s_id = @id",
                param1
            ), Times.Once);
        }

        [Fact]
        public void DeleteScheduleTable_ShouldExecuteNonQuery_WithCorrectParameters()
        {
            // Arrange
            int scheduleId = 1;
            int tableId = 2;

            // Act
            _scheduleRepository.DeleteScheduleTable(scheduleId, tableId);

            // Assert
            var param1 = ("@scheduleId", (object)scheduleId);
            var param2 = ("@tableId", (object)tableId);

            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                "DELETE FROM schedule_tables WHERE s_id = @scheduleId AND t_id = @tableId",
                param1, param2
            ), Times.Once);
        }
    }
}
