namespace Famicom.Components.Classes
{
    public class Rooms
    {
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public string RoomNumber { get; set; }
        public int RoomFloor { get; set; }

        public Rooms(int roomId, string roomName, string roomNumber, int floor)
        {
            RoomId = roomId;
            RoomName = roomName;
            RoomNumber = roomNumber;
            RoomFloor = floor;
        }
    }
}
