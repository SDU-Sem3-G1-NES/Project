using Moq;
using Xunit;

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
        public void InsertSchedule_ShouldExecuteNonQueryWithCorrectParameters()
        {
            // Arrange
            var name = "Test Schedule";
            var config = "Test Config";
            var owner = "Test Owner";

            // Act
            _scheduleRepository.InsertSchedule(name, config, owner);

            // Assert
            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                It.Is<string>(s => s.Contains("INSERT INTO schedules")),
                It.Is<(string, object)>(p => p.Item1 == "@name" && (string)p.Item2 == name),
                It.Is<(string, object)>(p => p.Item1 == "@config" && (string)p.Item2 == config),
                It.Is<(string, object)>(p => p.Item1 == "@owner" && (string)p.Item2 == owner)
            ), Times.Once);
        }

        [Fact]
        public void InsertScheduleTable_ShouldExecuteNonQueryWithCorrectParameters()
        {
            // Arrange
            var scheduleId = 1;
            var tableId = 2;

            // Act
            _scheduleRepository.InsertScheduleTable(scheduleId, tableId);

            // Assert
            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                It.Is<string>(s => s.Contains("INSERT INTO schedule_tables")),
                It.Is<(string, object)>(p => p.Item1 == "@scheduleId" && (int)p.Item2 == scheduleId),
                It.Is<(string, object)>(p => p.Item1 == "@tableId" && (int)p.Item2 == tableId)
            ), Times.Once);
        }

        [Fact]
        public void EditScheduleTable_ShouldExecuteNonQueryWithCorrectParameters()
        {
            // Arrange
            var scheduleId = 1;
            var tableId = 3;

            // Act
            _scheduleRepository.EditScheduleTable(scheduleId, tableId);

            // Assert
            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                It.Is<string>(s => s.Contains("UPDATE schedule_tables SET t_id")),
                It.Is<(string, object)>(p => p.Item1 == "@tableId" && (int)p.Item2 == tableId),
                It.Is<(string, object)>(p => p.Item1 == "@scheduleId" && (int)p.Item2 == scheduleId)
            ), Times.Once);
        }

        [Fact]
        public void EditScheduleName_ShouldExecuteNonQueryWithCorrectParameters()
        {
            // Arrange
            var scheduleId = 4;
            var scheduleName = "Updated Schedule Name";

            // Act
            _scheduleRepository.EditScheduleName(scheduleId, scheduleName);

            // Assert
            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                It.Is<string>(s => s.Contains("UPDATE schedules SET s_name")),
                It.Is<(string, object)>(p => p.Item1 == "@scheduleName" && (string)p.Item2 == scheduleName),
                It.Is<(string, object)>(p => p.Item1 == "@scheduleId" && (int)p.Item2 == scheduleId)
            ), Times.Once);
        }

        [Fact]
        public void EditScheduleConfig_ShouldExecuteNonQueryWithCorrectParameters()
        {
            // Arrange
            var scheduleId = 5;
            var scheduleConfig = "Updated Config";

            // Act
            _scheduleRepository.EditScheduleConfig(scheduleId, scheduleConfig);

            // Assert
            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                It.Is<string>(s => s.Contains("UPDATE schedules SET s_config")),
                It.Is<(string, object)>(p => p.Item1 == "@scheduleConfig" && (string)p.Item2 == scheduleConfig),
                It.Is<(string, object)>(p => p.Item1 == "@scheduleId" && (int)p.Item2 == scheduleId)
            ), Times.Once);
        }

        [Fact]
        public void EditScheduleOwner_ShouldExecuteNonQueryWithCorrectParameters()
        {
            // Arrange
            var scheduleId = 6;
            var scheduleOwner = "Updated Owner";

            // Act
            _scheduleRepository.EditScheduleOwner(scheduleId, scheduleOwner);

            // Assert
            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                It.Is<string>(s => s.Contains("UPDATE schedules SET s_owner")),
                It.Is<(string, object)>(p => p.Item1 == "@scheduleOwner" && (string)p.Item2 == scheduleOwner),
                It.Is<(string, object)>(p => p.Item1 == "@scheduleId" && (int)p.Item2 == scheduleId)
            ), Times.Once);
        }

        [Fact]
        public void DeleteSchedule_ShouldExecuteNonQueryWithCorrectParameters()
        {
            // Arrange
            var id = 7;

            // Act
            _scheduleRepository.DeleteSchedule(id);

            // Assert
            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                It.Is<string>(s => s.Contains("DELETE FROM schedules WHERE s_id")),
                It.Is<(string, object)>(p => p.Item1 == "@id" && (int)p.Item2 == id)
            ), Times.Once);
        }

        [Fact]
        public void DeleteScheduleTable_ShouldExecuteNonQueryWithCorrectParameters()
        {
            // Arrange
            var scheduleId = 8;
            var tableId = 9;

            // Act
            _scheduleRepository.DeleteScheduleTable(scheduleId, tableId);

            // Assert
            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                It.Is<string>(s => s.Contains("DELETE FROM schedule_tables WHERE s_id = @scheduleId AND t_id = @tableId")),
                It.Is<(string, object)>(p => p.Item1 == "@scheduleId" && (int)p.Item2 == scheduleId),
                It.Is<(string, object)>(p => p.Item1 == "@tableId" && (int)p.Item2 == tableId)
            ), Times.Once);
        }
    }
}
