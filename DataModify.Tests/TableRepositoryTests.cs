using Moq;
using Xunit;

namespace DataModify.Tests
{
    public class TableRepositoryTests
    {
        private readonly Mock<DbAccess> _dbAccessMock;
        private readonly TableRepository _tableRepository;

        public TableRepositoryTests()
        {
            // Initialize the DbAccess mock
            _dbAccessMock = new Mock<DbAccess>();
            // Inject the mock into TableRepository
            _tableRepository = new TableRepository(_dbAccessMock.Object);
        }

        [Fact]
        public void InsertTable_ShouldExecuteNonQueryWithCorrectParameters()
        {
            // Arrange
            var name = "Test Table";
            var manufacturer = "Test Manufacturer";
            var api = 3;

            // Act
            _tableRepository.InsertTable(name, manufacturer, api);

            // Assert
            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                It.Is<string>(s => s.Contains("INSERT INTO tables")),
                It.Is<(string, object)>(p => p.Item1 == "@name" && (string)p.Item2 == name),
                It.Is<(string, object)>(p => p.Item1 == "@manufacturer" && (string)p.Item2 == manufacturer),
                It.Is<(string, object)>(p => p.Item1 == "@api" && (int)p.Item2 == api)
            ), Times.Once);
        }

        [Fact]
        public void EditTableName_ShouldExecuteNonQueryWithCorrectParameters()
        {
            // Arrange
            var tableId = 1;
            var tableName = "Updated Table Name";

            // Act
            _tableRepository.EditTableName(tableId, tableName);

            // Assert
            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                It.Is<string>(s => s.Contains("UPDATE tables SET t_name")),
                It.Is<(string, object)>(p => p.Item1 == "@tableName" && (string)p.Item2 == tableName),
                It.Is<(string, object)>(p => p.Item1 == "@tableId" && (int)p.Item2 == tableId)
            ), Times.Once);
        }

        [Fact]
        public void EditTableManufacturer_ShouldExecuteNonQueryWithCorrectParameters()
        {
            // Arrange
            var tableId = 2;
            var tableManufacturer = "Updated Manufacturer";

            // Act
            _tableRepository.EditTableManufacturer(tableId, tableManufacturer);

            // Assert
            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                It.Is<string>(s => s.Contains("UPDATE tables SET t_manufacturer")),
                It.Is<(string, object)>(p => p.Item1 == "@tableManufacturer" && (string)p.Item2 == tableManufacturer),
                It.Is<(string, object)>(p => p.Item1 == "@tableId" && (int)p.Item2 == tableId)
            ), Times.Once);
        }

        [Fact]
        public void EditTableAPI_ShouldExecuteNonQueryWithCorrectParameters()
        {
            // Arrange
            var tableId = 3;
            var tableApi = 5;

            // Act
            _tableRepository.EditTableAPI(tableId, tableApi);

            // Assert
            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                It.Is<string>(s => s.Contains("UPDATE tables SET t_api")),
                It.Is<(string, object)>(p => p.Item1 == "@tableApi" && (int)p.Item2 == tableApi),
                It.Is<(string, object)>(p => p.Item1 == "@tableId" && (int)p.Item2 == tableId)
            ), Times.Once);
        }

        [Fact]
        public void DeleteTable_ShouldExecuteNonQueryWithCorrectParameters()
        {
            // Arrange
            var id = 4;

            // Act
            _tableRepository.DeleteTable(id);

            // Assert
            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                It.Is<string>(s => s.Contains("DELETE FROM tables WHERE t_id")),
                It.Is<(string, object)>(p => p.Item1 == "@id" && (int)p.Item2 == id)
            ), Times.Once);
        }
    }
}
