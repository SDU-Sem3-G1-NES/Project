using Xunit;
using Moq;
using DataAccess;

namespace DataAccess.Tests
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
        public void InsertApi_ShouldExecuteNonQuery_WithCorrectParameters()
        {
            // Arrange
            string name = "Api1";
            string config = "ConfigData";

            // Act
            _apiRepository.InsertApi(name, config);

            // Assert
            var param1 = ("@name", (object)name);
            var param2 = ("@config", (object)config);

            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                "INSERT INTO apis (a_name, a_config) VALUES (@name, @config::jsonb)",
                param1, param2
            ), Times.Once);
        }

        [Fact]
        public void EditApiName_ShouldExecuteNonQuery_WithCorrectParameters()
        {
            // Arrange
            int apiId = 1;
            string apiName = "UpdatedApi";

            // Act
            _apiRepository.EditApiName(apiId, apiName);

            // Assert
            var param1 = ("@apiName", (object)apiName);
            var param2 = ("@apiId", (object)apiId);

            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                "UPDATE apis SET a_name = @apiName WHERE a_id = @apiId",
                param1, param2
            ), Times.Once);
        }

        [Fact]
        public void EditApiConfig_ShouldExecuteNonQuery_WithCorrectParameters()
        {
            // Arrange
            int apiId = 1;
            string apiConfig = "UpdatedConfig";

            // Act
            _apiRepository.EditApiConfig(apiId, apiConfig);

            // Assert
            var param1 = ("@apiConfig", (object)apiConfig);
            var param2 = ("@apiId", (object)apiId);

            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                "UPDATE apis SET a_config = @apiConfig WHERE a_id = @apiId",
                param1, param2
            ), Times.Once);
        }

        [Fact]
        public void DeleteApi_ShouldExecuteNonQuery_WithCorrectParameters()
        {
            // Arrange
            int apiId = 1;

            // Act
            _apiRepository.DeleteApi(apiId);

            // Assert
            var param1 = ("@id", (object)apiId);

            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                "DELETE FROM apis WHERE a_id = @id",
                param1
            ), Times.Once);
        }
    }
}
