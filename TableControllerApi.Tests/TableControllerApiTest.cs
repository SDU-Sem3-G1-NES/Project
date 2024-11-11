using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TableController;
using TableControllerApi.Controllers;
using Xunit;
using SharedModels;

namespace TableControllerApi.Tests
{
    public class TablesControllerTests
    {
        private readonly Mock<ITableController> _mockTableController;
        private readonly Controllers.TableController _controller;

        public TablesControllerTests()
        {
            _mockTableController = new Mock<ITableController>();
            _controller = new Controllers.TableController(_mockTableController.Object);
        }

        [Fact]
        public async Task GetTables_ReturnsOkResult_WithListOfTableIds()
        {
            // Arrange
            var tableIds = new[] { "table1", "table2" };
            _mockTableController.Setup(tc => tc.GetAllTableIds()).Returns(tableIds);

            // Act
            var result = await _controller.GetTables();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<string[]>(okResult.Value);
            Assert.Equal(tableIds, returnValue);
        }

        [Fact]
        public async Task GetTables_ReturnsNotFound_WhenNoTablesFound()
        {
            // Arrange
            _mockTableController.Setup(tc => tc.GetAllTableIds()).Returns(new string[] { });

            // Act
            var result = await _controller.GetTables();

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetTableHeight_ReturnsOkResult_WithHeight()
        {
            // Arrange
            var guid = "table1";
            var height = 100;
            _mockTableController.Setup(tc => tc.GetTableHeight(guid)).Returns(height);

            // Act
            var result = await _controller.GetTableHeight(guid);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<int>(okResult.Value);
            Assert.Equal(height, returnValue);
        }

        [Fact]
        public async Task GetTableHeight_ReturnsNotFound_WhenTableNotFound()
        {
            // Arrange
            var guid = "table1";
            _mockTableController.Setup(tc => tc.GetTableHeight(guid)).Returns(-1);

            // Act
            var result = await _controller.GetTableHeight(guid);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task SetTableHeight_ReturnsOkResult_WhenHeightSetSuccessfully()
        {
            // Arrange
            var guid = "table1";
            var height = 100;
            _mockTableController.Setup(tc => tc.SetTableHeight(height, guid));
            _mockTableController.Setup(tc => tc.GetTableHeight(guid)).Returns(height);

            // Act
            var result = await _controller.SetTableHeight(guid, height);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Table height set successfully.", okResult.Value);
        }

        [Fact]
        public async Task SetTableHeight_ReturnsBadRequest_WhenHeightNotSet()
        {
            // Arrange
            var guid = "table1";
            var height = 100;
            _mockTableController.Setup(tc => tc.SetTableHeight(height, guid));
            _mockTableController.Setup(tc => tc.GetTableHeight(guid)).Returns(-1);

            // Act
            var result = await _controller.SetTableHeight(guid, height);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Failed to set table height.", badRequestResult.Value);
        }

        [Fact]
        public async Task GetTableSpeed_ReturnsOkResult_WithSpeed()
        {
            // Arrange
            var guid = "table1";
            var speed = 50;
            _mockTableController.Setup(tc => tc.GetTableSpeed(guid)).Returns(speed);

            // Act
            var result = await _controller.GetTableSpeed(guid);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<int>(okResult.Value);
            Assert.Equal(speed, returnValue);
        }

        [Fact]
        public async Task GetTableSpeed_ReturnsNotFound_WhenTableNotFound()
        {
            // Arrange
            var guid = "table1";
            _mockTableController.Setup(tc => tc.GetTableSpeed(guid)).Returns(-1);

            // Act
            var result = await _controller.GetTableSpeed(guid);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetTableStatus_ReturnsOkResult_WithStatus()
        {
            // Arrange
            var guid = "table1";
            var status = "Active";
            _mockTableController.Setup(tc => tc.GetTableStatus(guid)).Returns(status);

            // Act
            var result = await _controller.GetTableStatus(guid);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<string>(okResult.Value);
            Assert.Equal(status, returnValue);
        }

        [Fact]
        public async Task GetTableStatus_ReturnsNotFound_WhenTableNotFound()
        {
            // Arrange
            var guid = "table1";
            _mockTableController.Setup(tc => tc.GetTableStatus(guid)).Returns(string.Empty);

            // Act
            var result = await _controller.GetTableStatus(guid);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetTableError_ReturnsOkResult_WithErrorList()
        {
            // Arrange
            var guid = "table1";
            var errorList = new List<ITableError> { new TableError { TimeStamp = DateTime.Now, ErrorMessage = "table is breakdancing", ErrorId = 932 } };
            _mockTableController.Setup(tc => tc.GetTableError(guid));
            _mockTableController.Setup(tc => tc.ErrorList).Returns(errorList);

            // Act
            var result = await _controller.GetTableError(guid);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<ITableError>>(okResult.Value);
            Assert.Equal(errorList, returnValue);
        }
    }

    internal class TableError : ITableError
    {
        public DateTime TimeStamp { get ; set ; }
        public string ErrorMessage { get ; set ; } = String.Empty;
        public int ErrorId { get ; set ; }
    }
}