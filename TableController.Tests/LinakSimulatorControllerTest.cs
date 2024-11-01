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
        private LinakTable _table = null!;
        private LinakSimulatorController _controller = null!;

        private void GetFreshObjects()
        {
            _table = new LinakTable("test-guid", "test-name");
            _controller = new LinakSimulatorController(_table);
            _linakSimulatorTasksMock = new Mock<ILinakSimulatorTasks>();

            var field = typeof(LinakSimulatorController)
                .GetField("_tasks", BindingFlags.NonPublic | BindingFlags.Instance);
            field!.SetValue(_controller, _linakSimulatorTasksMock.Object);
        }

#region Tests with internal Table

        [Fact]
        public void GetAllTableIds_ReturnsAllTableIds() 
        {
            // Arrange
            GetFreshObjects();
            var tableIds = new []{"test-guid-1", "test-guid-2"};
            _linakSimulatorTasksMock
                .Setup(x => x.GetAllTableIds())
                .ReturnsAsync(tableIds);

            // Act
            var ids = _controller.GetAllTableIds();

            //Assert
            Assert.Equal(tableIds, ids);
        }

        [Fact]
        public void GetFullTableInfo_ReturnsFullTableInfo() 
        {
            // Arrange
            GetFreshObjects();
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
            var table = _controller.GetFullTableInfo();

            // Assert
            Assert.Equal("test-guid", table!.GUID);
            Assert.Equal("test-name", table.Name);
            Assert.Equal("Linak A/S", table.Manufacturer);
            Assert.Equal(100, table.Height);
            Assert.Equal(50, table.Speed);
        }

        [Fact]
        public void GetTableHeight_ReturnsHeight()
        {
            // Arrange
            GetFreshObjects();
            var apiTable = new LinakApiTable { position = 100 };
            _linakSimulatorTasksMock
                .Setup(x => x.GetTableInfo(It.IsAny<string>()))
                .ReturnsAsync(apiTable);

            // Act
            var height = _controller.GetTableHeight();

            // Assert
            Assert.Equal(100, height);
        }

        [Fact]
        public void SetTableHeight_SetsHeight()
        {
            // Arrange
            GetFreshObjects();
            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            };

            _linakSimulatorTasksMock
                .Setup(x => x.SetTableInfo(It.IsAny<LinakApiTable>()))
                .ReturnsAsync(responseMessage);

            // Act
            _controller.SetTableHeight(200);

            var fieldInfo = typeof(LinakSimulatorController).GetField("_table", BindingFlags.NonPublic | BindingFlags.Instance);
            var compareValue = typeof(LinakTable).GetProperty("Height")!.GetValue(fieldInfo!.GetValue(_controller));
            // Assert
            Assert.True(compareValue!.Equals(200));
        }

        [Fact]
        public void GetTableSpeed_ReturnsSpeed()
        {
            // Arrange
            GetFreshObjects();
            var apiTable = new LinakApiTable { speed = 50 };
            _linakSimulatorTasksMock
                .Setup(x => x.GetTableInfo(It.IsAny<string>()))
                .ReturnsAsync(apiTable);

            // Act
            var speed = _controller.GetTableSpeed();

            // Assert
            Assert.Equal(50, speed);
        }

        [Fact]
        public void GetTableStatus_ReturnsStatus()
        {
            // Arrange
            GetFreshObjects();
            var apiTable = new LinakApiTable { status = "active" };
            _linakSimulatorTasksMock
                .Setup(x => x.GetTableInfo(It.IsAny<string>()))
                .ReturnsAsync(apiTable);

            // Act
            var status = _controller.GetTableStatus();

            // Assert
            Assert.Equal("active", status);
        }
#endregion

#region Tests with injected GUID

[Fact]
        public void GetFullTableInfo_ReturnsFullTableInfoWithInjectedGUID() 
        {
            // Arrange
            GetFreshObjects();
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
            var table = _controller.GetFullTableInfo();

            // Assert
            Assert.Equal("test-guid", table!.GUID);
            Assert.Equal("test-name", table.Name);
            Assert.Equal("Linak A/S", table.Manufacturer);
            Assert.Equal(100, table.Height);
            Assert.Equal(50, table.Speed);
        }

        [Fact]
        public void GetTableHeight_ReturnsHeightWithInjectedGUID()
        {
            // Arrange
            GetFreshObjects();
            var apiTable = new LinakApiTable { position = 100 };
            var guid = "test-guid";
            _linakSimulatorTasksMock
                .Setup(x => x.GetTableInfo(guid))
                .ReturnsAsync(apiTable);

            // Act
            var height = _controller.GetTableHeight(guid);

            // Assert
            Assert.Equal(100, height);
        }

        [Fact]
        public void SetTableHeight_SetsHeightWithInjectedGUID()
        {
            // Arrange
            GetFreshObjects();
            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            };
            var guid = "test-guid";

            var apiTable = new LinakApiTable {id = guid, position = 200 };

            _linakSimulatorTasksMock
                .Setup(x => x.SetTableInfo(apiTable))
                .ReturnsAsync(responseMessage);
            _linakSimulatorTasksMock
                .Setup(x => x.GetTableInfo(It.IsAny<string>()))
                .ReturnsAsync(new LinakApiTable {id = guid,  position = 200 });

            // Act
            _controller.SetTableHeight(200, guid);
            int compareHeight = _controller.GetTableHeight(guid);

            // Assert
            Assert.Equal(200, compareHeight);
        }

        [Fact]
        public void GetTableSpeed_ReturnsSpeedWithInjectedGUID()
        {
            // Arrange
            GetFreshObjects();
            var apiTable = new LinakApiTable { speed = 50 };
            var guid = "test-guid";
            _linakSimulatorTasksMock
                .Setup(x => x.GetTableInfo(guid))
                .ReturnsAsync(apiTable);

            // Act
            var speed = _controller.GetTableSpeed(guid);

            // Assert
            Assert.Equal(50, speed);
        }

        [Fact]
        public void GetTableStatus_ReturnsStatusWithInjectedGUID()
        {
            // Arrange
            GetFreshObjects();
            var apiTable = new LinakApiTable { status = "active" };
            var guid = "test-guid";
            _linakSimulatorTasksMock
                .Setup(x => x.GetTableInfo(guid))
                .ReturnsAsync(apiTable);

            // Act
            var status = _controller.GetTableStatus(guid);

            // Assert
            Assert.Equal("active", status);
        }
#endregion


#region Not implemented methods
        [Fact]
        public void GetTableError_ThrowsNotImplementedException()
        {
            // Act & Assert
            GetFreshObjects();
            Assert.Throws<NotImplementedException>(() => _controller.GetTableError());
        }

        [Fact]
        public void GetActivationCounter_ThrowsNotImplementedException()
        {
            // Act & Assert
            GetFreshObjects();
            Assert.Throws<NotImplementedException>(() => _controller.GetActivationCounter());
        }

        [Fact]
        public void GetSitStandCounter_ThrowsNotImplementedException()
        {
            // Act & Assert
            GetFreshObjects();
            Assert.Throws<NotImplementedException>(() => _controller.GetSitStandCounter());
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