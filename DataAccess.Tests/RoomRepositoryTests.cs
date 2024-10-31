using Xunit;
using Moq;
using DataAccess;

namespace DataAccess.Tests
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
        public void InsertRoom_ShouldExecuteNonQuery_WithCorrectParameters()
        {
            // Arrange
            string name = "Room1";
            string number = "101";
            int floor = 1;

            // Act
            _roomRepository.InsertRoom(name, number, floor);

            // Assert
            var param1 = ("@name", (object)name);
            var param2 = ("@number", (object)number);
            var param3 = ("@floor", (object)floor);

            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                "INSERT INTO rooms (r_name, r_number, r_floor) VALUES (@name, @number, @floor)",
                param1, param2, param3
            ), Times.Once);
        }

        [Fact]
        public void EditRoomTable_ShouldExecuteNonQuery_WithCorrectParameters()
        {
            // Arrange
            int roomId = 1;
            int tableId = 3;

            // Act
            _roomRepository.EditRoomTable(roomId, tableId);

            // Assert
            var param1 = ("@tableId", (object)tableId);
            var param2 = ("@roomId", (object)roomId);

            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                "UPDATE room_tables SET t_id = @tableId WHERE r_id = @roomId",
                param1, param2
            ), Times.Once);
        }

        [Fact]
        public void EditRoomName_ShouldExecuteNonQuery_WithCorrectParameters()
        {
            // Arrange
            int roomId = 1;
            string roomName = "UpdatedRoom";

            // Act
            _roomRepository.EditRoomName(roomId, roomName);

            // Assert
            var param1 = ("@roomName", (object)roomName);
            var param2 = ("@roomId", (object)roomId);

            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                "UPDATE rooms SET r_name = @roomName WHERE r_id = @roomId",
                param1, param2
            ), Times.Once);
        }

        [Fact]
        public void EditRoomNumber_ShouldExecuteNonQuery_WithCorrectParameters()
        {
            // Arrange
            int roomId = 1;
            string roomNumber = "102";

            // Act
            _roomRepository.EditRoomNumber(roomId, roomNumber);

            // Assert
            var param1 = ("@roomNumber", (object)roomNumber);
            var param2 = ("@roomId", (object)roomId);

            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                "UPDATE rooms SET r_number = @roomNumber WHERE r_id = @roomId",
                param1, param2
            ), Times.Once);
        }

        [Fact]
        public void EditRoomFloor_ShouldExecuteNonQuery_WithCorrectParameters()
        {
            // Arrange
            int roomId = 1;
            int roomFloor = 2;

            // Act
            _roomRepository.EditRoomFloor(roomId, roomFloor);

            // Assert
            var param1 = ("@roomFloor", (object)roomFloor);
            var param2 = ("@roomId", (object)roomId);

            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                "UPDATE rooms SET r_floor = @roomFloor WHERE r_id = @roomId",
                param1, param2
            ), Times.Once);
        }

        [Fact]
        public void DeleteRoom_ShouldExecuteNonQuery_WithCorrectParameters()
        {
            // Arrange
            int roomId = 1;

            // Act
            _roomRepository.DeleteRoom(roomId);

            // Assert
            var param1 = ("@id", (object)roomId);

            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                "DELETE FROM rooms WHERE r_id = @id",
                param1
            ), Times.Once);
        }
    }
}
