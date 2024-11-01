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
        private readonly Mock<ILinakSimulatorTasks> _linakSimulatorTasksMock;
        private readonly LinakTable _table;
        private readonly LinakSimulatorController _controller;

        public LinakSimulatorControllerTest()
        {
            _linakSimulatorTasksMock = new Mock<ILinakSimulatorTasks>();
            _table = new LinakTable("test-guid", "test-name");
            _controller = new LinakSimulatorController(_table);

            var field = typeof(LinakSimulatorController)
                .GetField("_tasks", BindingFlags.NonPublic | BindingFlags.Instance);
            field!.SetValue(_controller, _linakSimulatorTasksMock.Object);
        }

        [Fact]
        public void GetAllTableIds_ReturnsAllTableIds() 
        {
            // Arrange
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
            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            };

            _linakSimulatorTasksMock
                .Setup(x => x.SetTableInfo(It.IsAny<LinakApiTable>()))
                .ReturnsAsync(responseMessage);

            // Act
            _controller.SetTableHeight(200);

            // Assert
            Assert.Equal(200, _table.Height);
        }

        [Fact]
        public void GetTableSpeed_ReturnsSpeed()
        {
            // Arrange
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
            var apiTable = new LinakApiTable { status = "active" };
            _linakSimulatorTasksMock
                .Setup(x => x.GetTableInfo(It.IsAny<string>()))
                .ReturnsAsync(apiTable);

            // Act
            var status = _controller.GetTableStatus();

            // Assert
            Assert.Equal("active", status);
        }

        [Fact]
        public void GetTableError_ThrowsNotImplementedException()
        {
            // Act & Assert
            Assert.Throws<NotImplementedException>(() => _controller.GetTableError());
        }

        [Fact]
        public void GetActivationCounter_ThrowsNotImplementedException()
        {
            // Act & Assert
            Assert.Throws<NotImplementedException>(() => _controller.GetActivationCounter());
        }

        [Fact]
        public void GetSitStandCounter_ThrowsNotImplementedException()
        {
            // Act & Assert
            Assert.Throws<NotImplementedException>(() => _controller.GetSitStandCounter());
        }
    }
}
