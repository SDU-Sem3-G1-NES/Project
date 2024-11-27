using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SharedModels;
using TableControllerApi.Controllers;
using Models.Services;
using Xunit;
using TableController;
using System.Net.Http;
using System.Reflection;

namespace TableControllerApi.Tests
{
    public class TableControllerTest
    {
        private readonly Mock<ITableControllerService> _mockTableControllerService;
        private readonly Mock<ITableController> _mockTableController;
        private readonly TableControllerApi.Controllers.TableController _controller;
        private readonly Progress<ITableStatusReport> _progress;
        private readonly HttpClient _client = new HttpClient();
        private readonly Mock<IHttpClientFactory> _mockFactory;

        public TableControllerTest()
        {
            _mockFactory = new Mock<IHttpClientFactory>();
            _mockFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(_client);
            _mockTableControllerService = new Mock<ITableControllerService>();
            _mockTableController = new Mock<ITableController>();
            _controller = new TableControllerApi.Controllers.TableController(_mockTableControllerService.Object);
            var clientFactoryProperty = typeof(Controllers.TableController).GetProperty("_clientFactory", BindingFlags.NonPublic | BindingFlags.Instance);
            if (clientFactoryProperty != null)
            {
                clientFactoryProperty.SetValue(_controller, _mockFactory.Object);
            }

            _progress = new Progress<ITableStatusReport>();
        }

        [Fact]
        public async Task GetFullTableInfo_ReturnsOkResult_WithTableInfo()
        {
            // Arrange
            var guid = "table1";
            var table = new LinakTable(guid, "name");
            _mockTableControllerService.Setup(s => s.GetTableController(guid, _client)).ReturnsAsync(_mockTableController.Object);
            _mockTableController.Setup(tc => tc.GetFullTableInfo(guid)).ReturnsAsync(table);

            // Act
            var result = await _controller.GetFullTableInfo(guid);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsAssignableFrom<ITable>(okResult.Value);
            Assert.Equal(table, returnValue);
        }

        [Fact]
        public async Task GetFullTableInfo_ReturnsNotFound_WhenTableNotFound()
        {
            // Arrange
            var guid = "table1";
            _mockTableControllerService.Setup(s => s.GetTableController(guid, _client)).ReturnsAsync(_mockTableController.Object);
            _mockTableController.Setup(tc => tc.GetFullTableInfo(guid)).ThrowsAsync(new Exception("Table not found."));

            // Act
            var result = await _controller.GetFullTableInfo(guid);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task SetTableHeight_ReturnsOkResult_WhenHeightSetSuccessfully()
        {
            // Arrange
            var guid = "table1";
            var height = 100;
            _mockTableControllerService.Setup(s => s.GetTableController(guid, _client)).ReturnsAsync(_mockTableController.Object);
            _mockTableController.Setup(tc => tc.SetTableHeight(height, guid, _progress)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.SetTableHeight(guid, height);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Table height set successfully.", okResult.Value);
        }

        [Fact]
        public async Task SetTableHeight_ReturnsServiceUnavailable_WhenHeightNotSet()
        {
            // Arrange
            var guid = "table1";
            var height = 100;
            _mockTableControllerService.Setup(s => s.GetTableController(guid, _client)).ReturnsAsync(_mockTableController.Object);
            _mockTableController.Setup(tc => tc.SetTableHeight(height, guid, It.IsAny<IProgress<ITableStatusReport>>())).ThrowsAsync(new Exception("Failed to set table height!"));

            // Act
            var result = await _controller.SetTableHeight(guid, height);

            // Assert
            var serviceUnavailableResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(503, serviceUnavailableResult.StatusCode);
            Assert.Equal("Failed to set table height!", serviceUnavailableResult.Value);
        }

        [Fact]
        public async Task SetTableHeight_ReturnsNotFound_WhenTableNotFound()
        {
            // Arrange
            var guid = "table1";
            var height = 100;
            _mockTableControllerService.Setup(s => s.GetTableController(guid, _client)).ReturnsAsync(_mockTableController.Object);
            _mockTableController.Setup(tc => tc.SetTableHeight(height, guid, It.IsAny<IProgress<ITableStatusReport>>())).ThrowsAsync(new Exception("Table not found."));

            // Act
            var result = await _controller.SetTableHeight(guid, height);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Table not found.", notFoundResult.Value);
        }
    }
}