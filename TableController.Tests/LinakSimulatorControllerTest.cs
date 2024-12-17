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
using FluentAssertions;

namespace TableController.Tests
{
    public class LinakSimulatorControllerTest
    {
        private Mock<ILinakSimulatorTasks> _linakSimulatorTasksMock = null!;
        private LinakSimulatorController _controller = null!;
        private LinakApiTable apiTable = null!;
        private IProgress<ITableStatusReport> _progress = null!;

        private async Task GetFreshObjects()
        {
            _controller = new LinakSimulatorController();
            _controller.HttpClient = new HttpClient();
            _linakSimulatorTasksMock = new Mock<ILinakSimulatorTasks>();

            var field = typeof(LinakSimulatorController)
                .GetField("_tasks", BindingFlags.NonPublic | BindingFlags.Instance);
            field!.SetValue(_controller, _linakSimulatorTasksMock.Object);

            apiTable = await GenerateTable();
            _progress = new Progress<ITableStatusReport>();

        }

        private static Task<LinakApiTable> GenerateTable(int position = 100, int speed = 50) {
            var returnTable = new LinakApiTable
            {
                id = "test-guid",
                config = new LinakApiTableConfig
                {
                    name = "test-name",
                    manufacturer = "Linak A/S"
                },
                state = new LinakApiTableState
                {
                    position_mm = position,
                    speed_mms = speed,
                    status = "active",
                    isPositionLost = false,
                    isAntiCollision = false,
                    isOverloadProtectionUp = false,
                    isOverloadProtectionDown = false
                },
                usage = new LinakApiTableUsage
                {
                    activationCounter = 0,
                    sitStandCounter = 0
                },
                lastErrors = new LinakApiTableError[]
                {
                    new LinakApiTableError
                    {
                        time_s = 1,
                        errorCode = 1
                    }
                }
            };
            return Task.FromResult(returnTable);
        }

#region Tests with injected GUID

[Fact]
        public async void GetFullTableInfo_ReturnsFullTableInfoWithInjectedGUID() 
        {
            // Arrange
            await GetFreshObjects();

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
        public async void GetTableHeight_ReturnsHeightWithInjectedGUID()
        {
            // Arrange
            await GetFreshObjects();
            var guid = "test-guid";

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

            _linakSimulatorTasksMock
                .Setup(x => x.SetTableHeight(It.IsAny<int>(), guid))
                .ReturnsAsync(responseMessage);
            _linakSimulatorTasksMock
                .Setup(x => x.GetTableInfo(It.IsAny<string>()))
                .ReturnsAsync(await GenerateTable(200, 50));

            // Act
            await _controller.SetTableHeight(200, guid, _progress);
            int compareHeight = await _controller.GetTableHeight(guid);

            // Assert
            Assert.Equal(200, compareHeight);
        }

        [Fact]
        public async void GetTableSpeed_ReturnsSpeedWithInjectedGUID()
        {
            // Arrange
            await GetFreshObjects();
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
        public async void GetTableStatus_ReturnsStatusWithInjectedGUID()
        {
            // Arrange
            await GetFreshObjects();
            var guid = "test-guid";
            _linakSimulatorTasksMock
                .Setup(x => x.GetTableInfo(guid))
                .ReturnsAsync(apiTable);

            // Act
            var status = await _controller.GetTableStatus(guid);

            // Assert
            Assert.Equal("active", status);
        }

        [Fact]
        public async void GetTableError_ReturnsErrorWithInjectedGUID()
        {
            // Arrange
            await GetFreshObjects();
            var guid = "test-guid";
            _linakSimulatorTasksMock
                .Setup(x => x.GetTableInfo(guid))
                .ReturnsAsync(apiTable);

            // Act
            var errorArray = await _controller.GetTableError(guid);
            var error = errorArray.FirstOrDefault();
            var compareError = new LinakTableError("test-guid", 1, 1, "Position Lost: The desk has an unknown position and needs to be initialized");

            // Assert
            error.Should().BeEquivalentTo(compareError);
        }
#endregion
    }

    public class LinakSimulatorTasksTests
    {
        private LinakApiTable _table = null!;
        private static Task<LinakApiTable> GenerateTable(int position = 100, int speed = 50) {
            var returnTable = new LinakApiTable
            {
                id = "test-guid",
                config = new LinakApiTableConfig
                {
                    name = "test-name",
                    manufacturer = "Linak A/S"
                },
                state = new LinakApiTableState
                {
                    position_mm = position,
                    speed_mms = speed,
                    status = "active",
                    isPositionLost = false,
                    isAntiCollision = false,
                    isOverloadProtectionUp = false,
                    isOverloadProtectionDown = false
                },
                usage = new LinakApiTableUsage
                {
                    activationCounter = 0,
                    sitStandCounter = 0
                },
                lastErrors = new LinakApiTableError[]
                {
                    new LinakApiTableError
                    {
                        time_s = 1,
                        errorCode = 1
                    }
                }
            };
            return Task.FromResult(returnTable);
        }
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
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _tasks = new LinakSimulatorTasks(_httpClient);
            _table = GenerateTable().Result;

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

            var expectedTable = await GenerateTable(502, 50);

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(expectedTable))
            };

            var propertyResponses = new Dictionary<string, HttpResponseMessage>
            {
                { "config", new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent(JsonSerializer.Serialize(expectedTable.config))
                    }
                },
                { "state", new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent(JsonSerializer.Serialize(expectedTable.state))
                    }
                },
                { "usage", new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent(JsonSerializer.Serialize(expectedTable.usage))
                    }
                },
            };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.Is<HttpRequestMessage>(r => 
                        r.RequestUri!.ToString().StartsWith($"{_baseUrl}/desks/{_table.id}") && 
                        r.Method == HttpMethod.Put), 
                    ItExpr.IsAny<CancellationToken>())
                .Returns((HttpRequestMessage request, CancellationToken token) =>
                {
                    var propertyName = request.RequestUri!.ToString().Split('/').Last();
                    if (propertyResponses.ContainsKey(propertyName))
                    {
                        return Task.FromResult(propertyResponses[propertyName]);
                    }
                    return Task.FromResult(response);
                });

            // Act
            expectedTable.lastErrors = null;
            var result = await _tasks.SetTableInfo(expectedTable);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }
        
        [Fact]
        public async Task SetTableHeight_SetsHeightCorrectly()
        {
            // Arrange
            GetFreshObjects();
            var guid = "test-guid";
            var newHeight = 800;
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(new Dictionary<string, object> { { "position_mm", newHeight } }))
            };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.Is<HttpRequestMessage>(r => r.RequestUri == new Uri($"{_baseUrl}/desks/{guid}/state") && r.Method == HttpMethod.Put),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var result = await _tasks.SetTableHeight(newHeight, guid);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }
#endregion
    }
}