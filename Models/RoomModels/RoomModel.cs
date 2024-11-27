using DataAccess;


namespace SharedModels
{
    internal class RoomModel
    {
        public RoomRepository RoomRepository { get; set; } = new RoomRepository();

        public RoomModel() 
        {
            
        }

        public List<string> RoomNames() 
        {
            List<Rooms> rooms = RoomRepository.GetRooms();

            List<string> roomNames = new List<string>();

            foreach (Rooms room in rooms) {
                if (room.RoomName != null)
                {
                    roomNames.Add(room.RoomName);
                }
            }
            return roomNames;

        }

    }
}
