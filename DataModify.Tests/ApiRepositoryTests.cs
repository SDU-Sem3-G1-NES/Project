using Moq;
using Xunit;

namespace DataModify.Tests
{
    public class ApiRepositoryTests
    {
        private readonly Mock<DbAccess> _dbAccessMock;
        private readonly ApiRepository _apiRepository;

        public ApiRepositoryTests()
        {
            _dbAccessMock = new Mock<DbAccess>();
            _apiRepository = new ApiRepository(_dbAccessMock.Object);
        }

        [Fact]
        public void InsertApi_ShouldExecuteNonQueryWithCorrectParameters()
        {
            // Arrange
            var name = "Test API";
            var config = "Test Config";

            // Act
            _apiRepository.InsertApi(name, config);

            // Assert
            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                It.Is<string>(s => s.Contains("INSERT INTO apis")),
                It.Is<(string, object)>(p => p.Item1 == "@name" && (string)p.Item2 == name),
                It.Is<(string, object)>(p => p.Item1 == "@config" && (string)p.Item2 == config)
            ), Times.Once);
        }

        [Fact]
        public void EditApiName_ShouldExecuteNonQueryWithCorrectParameters()
        {
            // Arrange
            var apiId = 1;
            var apiName = "Updated API Name";

            // Act
            _apiRepository.EditApiName(apiId, apiName);

            // Assert
            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                It.Is<string>(s => s.Contains("UPDATE apis SET a_name")),
                It.Is<(string, object)>(p => p.Item1 == "@apiName" && (string)p.Item2 == apiName),
                It.Is<(string, object)>(p => p.Item1 == "@apiId" && (int)p.Item2 == apiId)
            ), Times.Once);
        }

        [Fact]
        public void EditApiConfig_ShouldExecuteNonQueryWithCorrectParameters()
        {
            // Arrange
            var apiId = 2;
            var apiConfig = "Updated Config";

            // Act
            _apiRepository.EditApiConfig(apiId, apiConfig);

            // Assert
            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                It.Is<string>(s => s.Contains("UPDATE apis SET a_config")),
                It.Is<(string, object)>(p => p.Item1 == "@apiConfig" && (string)p.Item2 == apiConfig),
                It.Is<(string, object)>(p => p.Item1 == "@apiId" && (int)p.Item2 == apiId)
            ), Times.Once);
        }

        [Fact]
        public void DeleteApi_ShouldExecuteNonQueryWithCorrectParameters()
        {
            // Arrange
            var id = 3;

            // Act
            _apiRepository.DeleteApi(id);

            // Assert
            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                It.Is<string>(s => s.Contains("DELETE FROM apis WHERE a_id")),
                It.Is<(string, object)>(p => p.Item1 == "@id" && (int)p.Item2 == id)
            ), Times.Once);
        }
    }
}
