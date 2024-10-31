using Xunit;
using Moq;
using DataAccess;

namespace DataAccess.Tests
{
    public class TableRepositoryTests
    {
        private readonly Mock<DbAccess> _dbAccessMock;
        private readonly TableRepository _tableRepository;

        public TableRepositoryTests()
        {
            _dbAccessMock = new Mock<DbAccess>();
            _tableRepository = new TableRepository(_dbAccessMock.Object);
        }

        [Fact]
        public void InsertTable_ShouldExecuteNonQuery_WithCorrectParameters()
        {
            // Arrange
            string name = "Table1";
            string manufacturer = "Manufacturer1";
            int api = 1;

            // Act
            _tableRepository.InsertTable(name, manufacturer, api);

            // Assert
            var param1 = ("@name", (object)name);
            var param2 = ("@manufacturer", (object)manufacturer);
            var param3 = ("@api", (object)api);

            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                "INSERT INTO tables (t_name, t_manufacturer, t_api) VALUES (@name, @manufacturer, @api)",
                param1, param2, param3
            ), Times.Once);
        }

        [Fact]
        public void EditTableName_ShouldExecuteNonQuery_WithCorrectParameters()
        {
            // Arrange
            int tableId = 1;
            string tableName = "UpdatedTable";

            // Act
            _tableRepository.EditTableName(tableId, tableName);

            // Assert
            var param1 = ("@tableName", (object)tableName);
            var param2 = ("@tableId", (object)tableId);

            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                "UPDATE tables SET t_name = @tableName WHERE t_id = @tableId",
                param1, param2
            ), Times.Once);
        }

        [Fact]
        public void EditTableManufacturer_ShouldExecuteNonQuery_WithCorrectParameters()
        {
            // Arrange
            int tableId = 1;
            string tableManufacturer = "UpdatedManufacturer";

            // Act
            _tableRepository.EditTableManufacturer(tableId, tableManufacturer);

            // Assert
            var param1 = ("@tableManufacturer", (object)tableManufacturer);
            var param2 = ("@tableId", (object)tableId);

            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                "UPDATE tables SET t_manufacturer = @tableManufacturer WHERE t_id = @tableId",
                param1, param2
            ), Times.Once);
        }

        [Fact]
        public void EditTableAPI_ShouldExecuteNonQuery_WithCorrectParameters()
        {
            // Arrange
            int tableId = 1;
            int tableApi = 2;

            // Act
            _tableRepository.EditTableAPI(tableId, tableApi);

            // Assert
            var param1 = ("@tableApi", (object)tableApi);
            var param2 = ("@tableId", (object)tableId);

            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                "UPDATE tables SET t_api = @tableApi WHERE t_id = @tableId",
                param1, param2
            ), Times.Once);
        }

        [Fact]
        public void DeleteTable_ShouldExecuteNonQuery_WithCorrectParameters()
        {
            // Arrange
            int tableId = 1;

            // Act
            _tableRepository.DeleteTable(tableId);

            // Assert
            var param1 = ("@id", (object)tableId);

            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                "DELETE FROM tables WHERE t_id = @id",
                param1
            ), Times.Once);
        }
    }
}
