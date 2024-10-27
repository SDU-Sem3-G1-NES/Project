using Moq;
using Xunit;

namespace DataModify.Tests
{
    public class UserRepositoryTests
    {
        private readonly UserRepository _userRepository;
        private readonly Mock<DbAccess> _dbAccessMock;

        public UserRepositoryTests()
        {
            _dbAccessMock = new Mock<DbAccess>(); // Mocking DbAccess
            _userRepository = new UserRepository(_dbAccessMock.Object); // Use DI constructor for testing
        }

        [Fact]
        public void InsertUser_ShouldExecuteNonQueryWithCorrectParameters()
        {
            // Arrange
            var name = "Test User";
            var email = "testuser@example.com";
            var userType = 1;

            // Act
            _userRepository.InsertUser(name, email, userType);

            // Assert
            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                It.Is<string>(s => s.Contains("INSERT INTO users")),
                It.Is<(string, object)>(p => p.Item1 == "@name" && (string)p.Item2 == name),
                It.Is<(string, object)>(p => p.Item1 == "@email" && (string)p.Item2 == email),
                It.Is<(string, object)>(p => p.Item1 == "@userType" && (int)p.Item2 == userType)
            ), Times.Once);
        }

        [Fact]
        public void EditUserTypeName_ShouldExecuteNonQueryWithCorrectParameters()
        {
            // Arrange
            var userTypeId = 1;
            var userTypeName = "Updated Type";

            // Act
            _userRepository.EditUserTypeName(userTypeId, userTypeName);

            // Assert
            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                It.Is<string>(s => s.Contains("UPDATE user_types SET ut_name")),
                It.Is<(string, object)>(p => p.Item1 == "@userTypeName" && (string)p.Item2 == userTypeName),
                It.Is<(string, object)>(p => p.Item1 == "@userTypeId" && (int)p.Item2 == userTypeId)
            ), Times.Once);
        }

        [Fact]
        public void DeleteUser_ShouldExecuteNonQueryWithCorrectParameters()
        {
            // Arrange
            var userId = 1;

            // Act
            _userRepository.DeleteUser(userId);

            // Assert
            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                It.Is<string>(s => s.Contains("DELETE FROM users WHERE u_id")),
                It.Is<(string, object)>(p => p.Item1 == "@id" && (int)p.Item2 == userId)
            ), Times.Once);
        }
    }
}
