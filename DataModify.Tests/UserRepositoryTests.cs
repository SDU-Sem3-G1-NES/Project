using Xunit;
using Moq;
using DataModify;
using System.Collections.Generic;
using System.Data.Common;

namespace DataModify.Tests
{
    public class UserRepositoryTests
    {
        private readonly Mock<DbAccess> _dbAccessMock;
        private readonly UserRepository _userRepository;

        public UserRepositoryTests()
        {
            _dbAccessMock = new Mock<DbAccess>();
            _userRepository = new UserRepository(_dbAccessMock.Object);
        }

        [Fact]
        public void InsertUser_ShouldExecuteNonQuery_WithCorrectParameters()
        {
            // Arrange
            string name = "John Doe";
            string email = "johndoe@example.com";
            int userType = 1;

            // Act
            _userRepository.InsertUser(name, email, userType);

            // Assert
            var param1 = ("@name", (object)name);
            var param2 = ("@email", (object)email);
            var param3 = ("@userType", (object)userType);

            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                "INSERT INTO users (u_name, u_mail, u_type) VALUES (@name, @email, @userType)",
                param1, param2, param3
            ), Times.Once);
        }

        [Fact]
        public void InsertUserCredentials_ShouldExecuteNonQuery_WithCorrectParameters()
        {
            // Arrange
            byte[] emailHash = new byte[] { 1, 2, 3 };
            byte[] passwordHash = new byte[] { 4, 5, 6 };

            // Act
            _userRepository.InsertUserCredentials(emailHash, passwordHash);

            // Assert
            var param1 = ("@emailHash", (object)emailHash);
            var param2 = ("@passwordHash", (object)passwordHash);

            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                "INSERT INTO user_credentials (umail_hash, upass_hash) VALUES (@emailHash, @passwordHash)",
                param1, param2
            ), Times.Once);
        }

        [Fact]
        public void InsertUserType_ShouldExecuteNonQuery_WithCorrectParameters()
        {
            // Arrange
            string name = "Admin";
            string permissions = "FullAccess";

            // Act
            _userRepository.InsertUserType(name, permissions);

            // Assert
            var param1 = ("@name", (object)name);
            var param2 = ("@permissions", (object)permissions);

            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                "INSERT INTO user_types (ut_name, ut_permissions) VALUES (@name, @permissions)",
                param1, param2
            ), Times.Once);
        }

        [Fact]
        public void InsertUserHabit_ShouldExecuteNonQuery_WithCorrectParameters()
        {
            // Arrange
            int userId = 1;
            string eventJson = "{\"habit\":\"exercise\"}";

            // Act
            _userRepository.InsertUserHabit(userId, eventJson);

            // Assert
            var param1 = ("@userId", (object)userId);
            var param2 = ("@eventJson", (object)eventJson);

            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                "INSERT INTO user_habits (u_id, h_event) VALUES (@userId, @eventJson)",
                param1, param2
            ), Times.Once);
        }

        [Fact]
        public void EditUserTypeName_ShouldExecuteNonQuery_WithCorrectParameters()
        {
            // Arrange
            int userTypeId = 1;
            string userTypeName = "SuperAdmin";

            // Act
            _userRepository.EditUserTypeName(userTypeId, userTypeName);

            // Assert
            var param1 = ("@userTypeName", (object)userTypeName);
            var param2 = ("@userTypeId", (object)userTypeId);

            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                "UPDATE user_types SET ut_name = @userTypeName WHERE ut_id = @userTypeId",
                param1, param2
            ), Times.Once);
        }

        [Fact]
        public void EditUserTable_ShouldExecuteNonQuery_WithCorrectParameters()
        {
            // Arrange
            int userId = 1;
            int tableId = 2;

            // Act
            _userRepository.EditUserTable(userId, tableId);

            // Assert
            var param1 = ("@tableId", (object)tableId);
            var param2 = ("@userId", (object)userId);

            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                "UPDATE user_tables SET t_id = @tableId WHERE u_id = @userId",
                param1, param2
            ), Times.Once);
        }

        [Fact]
        public void DeleteUser_ShouldExecuteNonQuery_WithCorrectParameters()
        {
            // Arrange
            int userId = 1;

            // Act
            _userRepository.DeleteUser(userId);

            // Assert
            var param1 = ("@id", (object)userId);

            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                "DELETE FROM users WHERE u_id = @id",
                param1
            ), Times.Once);
        }
    }
}
