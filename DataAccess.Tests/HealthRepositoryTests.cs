using Xunit;
using Moq;
using DataAccess;

namespace DataAccess.Tests
{
    public class HealthRepositoryTests
    {
        private readonly Mock<DbAccess> _dbAccessMock;
        private readonly HealthRepository _healthRepository;

        public HealthRepositoryTests()
        {
            _dbAccessMock = new Mock<DbAccess>();
            _healthRepository = new HealthRepository(_dbAccessMock.Object);
        }

        [Fact]
        public void InsertHealth_ShouldExecuteNonQuery_WithCorrectParameters()
        {
            // Arrange
            int userId = 1;
            int? presetID = 1;
            int position = 1;

            // Act
            _healthRepository.InsertHealth(userId, presetID, position);

            // Assert
            var param1 = ("@userId", (object)userId);
            var param2 = ("@presetID", (object?)presetID ?? DBNull.Value);
            var param3 = ("@position", (object)position);

            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                "INSERT INTO health (h_user, h_preset, h_position) VALUES (@userId, @presetID, @position)",
                param1, param2, param3
            ), Times.Once);
        }

        [Fact]
        public void GetHealthByUser_ShouldExecuteQuery_WithCorrectParameters()
        {
            // Arrange
            int userID = 1;
            DateTime? endtime = null;

            // Act
            _healthRepository.GetHealthByUser(userID, endtime);

            // Assert
            var param1 = ("@userID", (object)userID);
            var param2 = ("@endtime", (object?)endtime ?? DBNull.Value);

            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                "SELECT * FROM health WHERE h_user = @userID AND h_time < @endtime",
                param1, param2
            ), Times.Once);
        }
    }
}
