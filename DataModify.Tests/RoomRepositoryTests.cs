using Moq;
using Xunit;

namespace DataModify.Tests
{
    public class RoomRepositoryTests
    {
        private readonly Mock<DbAccess> _dbAccessMock;
        private readonly RoomRepository _roomRepository;

        public RoomRepositoryTests()
        {
            _dbAccessMock = new Mock<DbAccess>();
            _roomRepository = new RoomRepository(_dbAccessMock.Object);
        }

        [Fact]
        public void InsertRoom_ShouldExecuteNonQueryWithCorrectParameters()
        {
            // Arrange
            var name = "Test Room";
            var number = "101";
            var floor = 1;

            // Act
            _roomRepository.InsertRoom(name, number, floor);

            // Assert
            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                It.Is<string>(s => s.Contains("INSERT INTO rooms")),
                It.Is<(string, object)>(p => p.Item1 == "@name" && (string)p.Item2 == name),
                It.Is<(string, object)>(p => p.Item1 == "@number" && (string)p.Item2 == number),
                It.Is<(string, object)>(p => p.Item1 == "@floor" && (int)p.Item2 == floor)
            ), Times.Once);
        }

        [Fact]
        public void EditRoomTable_ShouldExecuteNonQueryWithCorrectParameters()
        {
            // Arrange
            var roomId = 1;
            var tableId = 2;

            // Act
            _roomRepository.EditRoomTable(roomId, tableId);

            // Assert
            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                It.Is<string>(s => s.Contains("UPDATE room_tables SET t_id")),
                It.Is<(string, object)>(p => p.Item1 == "@tableId" && (int)p.Item2 == tableId),
                It.Is<(string, object)>(p => p.Item1 == "@roomId" && (int)p.Item2 == roomId)
            ), Times.Once);
        }

        [Fact]
        public void EditRoomName_ShouldExecuteNonQueryWithCorrectParameters()
        {
            // Arrange
            var roomId = 3;
            var roomName = "Updated Room Name";

            // Act
            _roomRepository.EditRoomName(roomId, roomName);

            // Assert
            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                It.Is<string>(s => s.Contains("UPDATE rooms SET r_name")),
                It.Is<(string, object)>(p => p.Item1 == "@roomName" && (string)p.Item2 == roomName),
                It.Is<(string, object)>(p => p.Item1 == "@roomId" && (int)p.Item2 == roomId)
            ), Times.Once);
        }

        [Fact]
        public void EditRoomNumber_ShouldExecuteNonQueryWithCorrectParameters()
        {
            // Arrange
            var roomId = 4;
            var roomNumber = "102";

            // Act
            _roomRepository.EditRoomNumber(roomId, roomNumber);

            // Assert
            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                It.Is<string>(s => s.Contains("UPDATE rooms SET r_number")),
                It.Is<(string, object)>(p => p.Item1 == "@roomNumber" && (string)p.Item2 == roomNumber),
                It.Is<(string, object)>(p => p.Item1 == "@roomId" && (int)p.Item2 == roomId)
            ), Times.Once);
        }

        [Fact]
        public void EditRoomFloor_ShouldExecuteNonQueryWithCorrectParameters()
        {
            // Arrange
            var roomId = 5;
            var roomFloor = 2;

            // Act
            _roomRepository.EditRoomFloor(roomId, roomFloor);

            // Assert
            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                It.Is<string>(s => s.Contains("UPDATE rooms SET r_floor")),
                It.Is<(string, object)>(p => p.Item1 == "@roomFloor" && (int)p.Item2 == roomFloor),
                It.Is<(string, object)>(p => p.Item1 == "@roomId" && (int)p.Item2 == roomId)
            ), Times.Once);
        }

        [Fact]
        public void DeleteRoom_ShouldExecuteNonQueryWithCorrectParameters()
        {
            // Arrange
            var id = 6;

            // Act
            _roomRepository.DeleteRoom(id);

            // Assert
            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                It.Is<string>(s => s.Contains("DELETE FROM rooms WHERE r_id")),
                It.Is<(string, object)>(p => p.Item1 == "@id" && (int)p.Item2 == id)
            ), Times.Once);
        }
    }
}
