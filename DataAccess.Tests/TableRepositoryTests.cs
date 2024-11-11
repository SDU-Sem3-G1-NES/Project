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
            string guid = "sds92922";
            string name = "Table1";
            string manufacturer = "Manufacturer1";
            int api = 1;

            // Act
            _tableRepository.InsertTable(guid,name, manufacturer, api);

            // Assert
            var param1 = ("@guid", (object)guid);
            var param2 = ("@name", (object)name);
            var param3 = ("@manufacturer", (object)manufacturer);
            var param4 = ("@api", (object)api);

            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                "INSERT INTO tables (t_guid ,t_name, t_manufacturer, t_api) VALUES (@guid ,@name, @manufacturer, @api)",
                param1, param2, param3, param4
            ), Times.Once);
        }

        [Fact]
        public void EditTableName_ShouldExecuteNonQuery_WithCorrectParameters()
        {
            // Arrange
            string tableId = "1";
            string tableName = "UpdatedTable";

            // Act
            _tableRepository.EditTableName(tableId, tableName);

            // Assert
            var param1 = ("@tableName", (object)tableName);
            var param2 = ("@tableId", (object)tableId);

            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                "UPDATE tables SET t_name = @tableName WHERE t_guid = @tableId",
                param1, param2
            ), Times.Once);
        }

        [Fact]
        public void EditTableManufacturer_ShouldExecuteNonQuery_WithCorrectParameters()
        {
            // Arrange
            string tableId = "1";
            string tableManufacturer = "UpdatedManufacturer";

            // Act
            _tableRepository.EditTableManufacturer(tableId, tableManufacturer);

            // Assert
            var param1 = ("@tableManufacturer", (object)tableManufacturer);
            var param2 = ("@tableId", (object)tableId);

            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                "UPDATE tables SET t_manufacturer = @tableManufacturer WHERE t_guid = @tableId",
                param1, param2
            ), Times.Once);
        }

        [Fact]
        public void EditTableAPI_ShouldExecuteNonQuery_WithCorrectParameters()
        {
            // Arrange
            string tableId = "1";
            int tableApi = 2;

            // Act
            _tableRepository.EditTableAPI(tableId, tableApi);

            // Assert
            var param1 = ("@tableApi", (object)tableApi);
            var param2 = ("@tableId", (object)tableId);

            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                "UPDATE tables SET t_api = @tableApi WHERE t_guid = @tableId",
                param1, param2
            ), Times.Once);
        }

        [Fact]
        public void DeleteTable_ShouldExecuteNonQuery_WithCorrectParameters()
        {
            // Arrange
            string tableId = "1";

            // Act
            _tableRepository.DeleteTable(tableId);

            // Assert
            var param1 = ("@id", (object)tableId);

            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                "DELETE FROM tables WHERE t_guid = @id",
                param1
            ), Times.Once);
        }
    }
}
