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
            _controller = new TableControllerApi.Controllers.TableController(_mockTableControllerService.Object, _mockFactory.Object);
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
    public class WebhookControllerTest
    {
        private readonly Mock<ISubscriberUriService> _mockSubscriberUriService;
        private readonly WebhookController _controller;

        public WebhookControllerTest()
        {
            _mockSubscriberUriService = new Mock<ISubscriberUriService>();
            _controller = new WebhookController(_mockSubscriberUriService.Object);
        }

        [Fact]
        public async Task ReceiveWebhook_ReturnsOk_WhenSubscriptionAdded()
        {
            // Arrange
            var guid = "test-guid";
            var uri = "http://www.example.com/example";
            _mockSubscriberUriService.Setup(s => s.Add(guid, uri)).Returns(true);

            // Act
            var result = await _controller.ReceiveWebhook(guid, uri);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Subscription added.", okResult.Value);
        }

        [Fact]
        public async Task ReceiveWebhook_ReturnsBadRequest_WhenSubscriptionFails()
        {
            // Arrange
            var guid = "test-guid";
            var uri = "http://www.example.com/example";
            _mockSubscriberUriService.Setup(s => s.Add(guid, uri)).Returns(false);

            // Act
            var result = await _controller.ReceiveWebhook(guid, uri);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Subscribtion failed. This table might already have a subscription. Are you using the right format?\n\"http://www.example.com/example\"", badRequestResult.Value);
        }

        [Fact]
        public async Task ReceiveWebhook_ReturnsBadRequest_WhenExceptionThrown()
        {
            // Arrange
            var guid = "test-guid";
            var uri = "http://www.example.com/example";
            _mockSubscriberUriService.Setup(s => s.Add(guid, uri)).Throws(new Exception("Test exception"));

            // Act
            var result = await _controller.ReceiveWebhook(guid, uri);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Subscribtion failed. Test exception", badRequestResult.Value);
        }

        [Fact]
        public async Task RemoveWebhook_ReturnsOk_WhenUnsubscribedSuccessfully()
        {
            // Arrange
            var guid = "test-guid";
            _mockSubscriberUriService.Setup(s => s.Remove(guid)).Returns(true);

            // Act
            var result = await _controller.RemoveWebhook(guid);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Unsubscribed successfully.", okResult.Value);
        }

        [Fact]
        public async Task RemoveWebhook_ReturnsNotFound_WhenSubscriptionNotFound()
        {
            // Arrange
            var guid = "test-guid";
            _mockSubscriberUriService.Setup(s => s.Remove(guid)).Returns(false);

            // Act
            var result = await _controller.RemoveWebhook(guid);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Subscription not found", notFoundResult.Value);
        }

        [Fact]
        public async Task RemoveWebhook_ReturnsBadRequest_WhenExceptionThrown()
        {
            // Arrange
            var guid = "test-guid";
            _mockSubscriberUriService.Setup(s => s.Remove(guid)).Throws(new Exception("Test exception"));

            // Act
            var result = await _controller.RemoveWebhook(guid);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Failed to unsubscribe. Test exception", badRequestResult.Value);
        }
    }
}