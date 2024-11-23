using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Xunit;
using TableController;
using SharedModels;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Reflection;

namespace TableController.Tests
{
    public class LinakSimulatorControllerTest
    {
        private Mock<ILinakSimulatorTasks> _linakSimulatorTasksMock = null!;
        private LinakSimulatorController _controller = null!;

        private Task GetFreshObjects()
        {
            _controller = new LinakSimulatorController();
            _linakSimulatorTasksMock = new Mock<ILinakSimulatorTasks>();

            var field = typeof(LinakSimulatorController)
                .GetField("_tasks", BindingFlags.NonPublic | BindingFlags.Instance);
            field!.SetValue(_controller, _linakSimulatorTasksMock.Object);
            return Task.CompletedTask;
        }

#region Tests with injected GUID

[Fact]
        public async Task GetFullTableInfo_ReturnsFullTableInfoWithInjectedGUID() 
        {
            // Arrange
            await GetFreshObjects();
            var apiTable = new LinakApiTable
            {
                id = "test-guid",
                name = "test-name",
                manufacturer = "Linak A/S",
                position = 100,
                speed = 50,
                status = "active"
            };


            _linakSimulatorTasksMock
                .Setup(x => x.GetTableInfo(It.IsAny<string>()))
                .ReturnsAsync(apiTable);

            // Act
            var table = await _controller.GetFullTableInfo("test-guid");

            // Assert
            Assert.Equal("test-guid", table!.GUID);
            Assert.Equal("test-name", table.Name);
            Assert.Equal("Linak A/S", table.Manufacturer);
            Assert.Equal(100, table.Height);
            Assert.Equal(50, table.Speed);
        }

        [Fact]
        public async Task GetTableHeight_ReturnsHeightWithInjectedGUID()
        {
            // Arrange
            await GetFreshObjects();
            var guid = "test-guid";
            var apiTable = new LinakApiTable {id = guid, name = "test", position = 100 };

            _linakSimulatorTasksMock
                .Setup(x => x.GetTableInfo(guid))
                .ReturnsAsync(apiTable);

            // Act
            var height = await _controller.GetTableHeight(guid);

            // Assert
            Assert.Equal(100, height);
        }

        [Fact]
        public async Task SetTableHeight_SetsHeightWithInjectedGUIDAsync()
        {
            // Arrange
            await GetFreshObjects();
            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            };
            var guid = "test-guid";

            var apiTable = new LinakApiTable {id = guid, name = "test", position = 200 };

            _linakSimulatorTasksMock
                .Setup(x => x.SetTableInfo(It.IsAny<LinakApiTable>()))
                .ReturnsAsync(responseMessage);
            _linakSimulatorTasksMock
                .Setup(x => x.GetTableInfo(It.IsAny<string>()))
                .ReturnsAsync(new LinakApiTable {id = guid, name = "test", position = 200 });

            // Act
            await _controller.SetTableHeight(200, guid);
            int compareHeight = await _controller.GetTableHeight(guid);

            // Assert
            Assert.Equal(200, compareHeight);
        }

        [Fact]
        public async Task GetTableSpeed_ReturnsSpeedWithInjectedGUID()
        {
            // Arrange
            await GetFreshObjects();
            var apiTable = new LinakApiTable { speed = 50 };
            var guid = "test-guid";
            _linakSimulatorTasksMock
                .Setup(x => x.GetTableInfo(guid))
                .ReturnsAsync(apiTable);

            // Act
            var speed = await _controller.GetTableSpeed(guid);

            // Assert
            Assert.Equal(50, speed);
        }

        [Fact]
        public async Task GetTableStatus_ReturnsStatusWithInjectedGUID()
        {
            // Arrange
            await GetFreshObjects();
            var apiTable = new LinakApiTable { status = "active" };
            var guid = "test-guid";
            _linakSimulatorTasksMock
                .Setup(x => x.GetTableInfo(guid))
                .ReturnsAsync(apiTable);

            // Act
            var status = await _controller.GetTableStatus(guid);

            // Assert
            Assert.Equal("active", status);
        }
#endregion


#region Not implemented methods
        [Fact]
        public async Task GetTableError_ThrowsNotImplementedException()
        {
            // Act & Assert
            await GetFreshObjects();
            await Assert.ThrowsAsync<NotImplementedException>(async () => await _controller.GetTableError(""));
        }

        [Fact]
        public async Task GetActivationCounter_ThrowsNotImplementedException()
        {
            // Act & Assert
            await GetFreshObjects();
            await Assert.ThrowsAsync<NotImplementedException>(async () => await _controller.GetActivationCounter(""));
        }

        [Fact]
        public async Task GetSitStandCounter_ThrowsNotImplementedException()
        {
            // Act & Assert
            await GetFreshObjects();
            await Assert.ThrowsAsync<NotImplementedException>(async () => await _controller.GetSitStandCounter(""));
        }
#endregion
    }

    public class LinakSimulatorTasksTests
    {
        private LinakApiTable _table = null!;
        private Mock<HttpMessageHandler> _httpMessageHandlerMock = null!;
        private HttpClient _httpClient = null!;
        private LinakSimulatorTasks _tasks = null!;
        private string _baseUrl = "http://example.com:1234/api/test/test-key";

        public LinakSimulatorTasksTests()
        {
            GetFreshObjects();
        }

#region LinakSimulatorTasksTests
        private void GetFreshObjects()
        {
            _table = new LinakApiTable
            {
                id = "test-guid",
                name = "test-name",
                manufacturer = "Linak A/S",
                position = 100,
                speed = 50,
                status = "active"
            };
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _tasks = new LinakSimulatorTasks();

            var clientField = typeof(LinakSimulatorTasks)
                .GetField("_client", BindingFlags.NonPublic | BindingFlags.Instance);
            clientField!.SetValue(_tasks, _httpClient);

            var optionsField = typeof(LinakSimulatorTasks)
                .GetField("_baseUrl", BindingFlags.NonPublic | BindingFlags.Instance);
            optionsField!.SetValue(_tasks, _baseUrl);
        }

        [Fact] 
        public async Task GetTableInfo_ReturnsTableInfo()
        {
            // Arrange
            GetFreshObjects();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(_table))
            };
            var guid = "test-guid";

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", 
                    ItExpr.Is<HttpRequestMessage>(r => r.RequestUri == new Uri($"{_baseUrl}/desks/{guid}")), 
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);
            
            // Act
            var result = await _tasks.GetTableInfo("test-guid");

            // Assert
            Assert.Equal("test-guid", result!.id);
        }

        [Fact]
        public async Task SetTableInfo_ReturnsOkHttpResponseWithCorrectBody()
        {
            // Arrange
            GetFreshObjects();
            var expectedResponse = new Dictionary<string, string>
            {
                {"id", "test-guid"},
                {"name", "test-name2"},
                {"manufacturer", "test-manufacturer2"},
                {"position", "1002"},
                {"speed", "502"},
                {"status", "test-status2"}
            };

            var expectedTable = new LinakApiTable
            {
                id = "test-guid",
                name = "test-name2",
                //manufacturer = "test-manufacturer2", // Left out on purpose
                //position = 1002, // Left out on purpose
                speed = 502,
                status = "test-status2"
            };

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(expectedResponse))
            };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.Is<HttpRequestMessage>(r => r.RequestUri == new Uri($"{_baseUrl}/desks/{_table.id}") && r.Method == HttpMethod.Put), 
                    ItExpr.IsAny<CancellationToken>())
                .Returns(async (HttpRequestMessage request, CancellationToken token) =>
                {
                    // Make sure only the current property is part of the http response body, not the whole object.
                    // The Method we are testing loops through the properties and sets them one by one.
                    var requestBody = request.Content != null ? await request.Content.ReadAsStringAsync() : string.Empty;
                    var requestData = JsonSerializer.Deserialize<Dictionary<string, object>>(requestBody) ?? new Dictionary<string, object>();
                    var responseData = new Dictionary<string, string> {{requestData.Keys.First().ToString(), expectedResponse[requestData.Keys.First().ToString()]}};

                    var response = new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent(JsonSerializer.Serialize(responseData))
                    };

                    return await Task.FromResult(response);
                });

            // Act
            var result = await _tasks.SetTableInfo(expectedTable);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }
#endregion
    }
}