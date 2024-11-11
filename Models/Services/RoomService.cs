using DataAccess;
using SharedModels;

namespace Models.Services
{
    public class RoomService
    {
        private readonly RoomRepository roomRepository;

        public RoomService(RoomRepository roomRepository)
        {
            this.roomRepository = roomRepository;
        }

        public void AddRoom(string name, string number, int floor)
        {
            roomRepository.InsertRoom(name, number, floor);
        }

        public void AddRoomTable(int roomId, string tableId)
        {
            roomRepository.InsertRoomTable(roomId, tableId);
        }

        public void UpdateRoomTable(int roomId, string tableId)
        {
            roomRepository.EditRoomTable(roomId, tableId);
        }

        public void UpdateRoomName(int roomId, string roomName)
        {
            roomRepository.EditRoomName(roomId, roomName);
        }

        public void UpdateRoomNumber(int roomId, string roomNumber)
        {
            roomRepository.EditRoomNumber(roomId, roomNumber);
        }

        public void UpdateRoomFloor(int roomId, int roomFloor)
        {
            roomRepository.EditRoomFloor(roomId, roomFloor);
        }

        public void RemoveRoom(int id)
        {
            roomRepository.DeleteRoom(id);
        }

        public List<Rooms> GetRooms()
        {
            return roomRepository.GetRooms();
        }
    }
}