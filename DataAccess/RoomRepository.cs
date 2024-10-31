using System;

namespace DataAccess
{
    public class RoomRepository
    {
        private readonly DbAccess dbAccess;

        public RoomRepository()
        {
            dbAccess = new DbAccess();
        }

        // Constructor for testing.
        public RoomRepository(DbAccess dbAccess)
        {
            this.dbAccess = dbAccess;
        }

        #region Insert Methods
        public void InsertRoom(string name, string number, int floor)
        {
            var sql = "INSERT INTO rooms (r_name, r_number, r_floor) VALUES (@name, @number, @floor)";
            dbAccess.ExecuteNonQuery(sql, ("@name", name), ("@number", number), ("@floor", floor));
        }

        public void InsertRoomTable(int roomId, int tableId)
        {
            var sql = "INSERT INTO room_tables (r_id, t_id) VALUES (@roomId, @tableId)";
            dbAccess.ExecuteNonQuery(sql, ("@roomId", roomId), ("@tableId", tableId));
        }

        #endregion

        #region Edit Methods
        public void EditRoomTable(int roomId, int tableId)
        {
            var sql = "UPDATE room_tables SET t_id = @tableId WHERE r_id = @roomId";
            dbAccess.ExecuteNonQuery(sql, ("@tableId", tableId), ("@roomId", roomId));
        }

        // 3 methods to update rooms
        public void EditRoomName(int roomId, string roomName)
        {
            var sql = "UPDATE rooms SET r_name = @roomName WHERE r_id = @roomId";
            dbAccess.ExecuteNonQuery(sql, ("@roomName", roomName), ("@roomId", roomId));
        }

        public void EditRoomNumber(int roomId, string roomNumber)
        {
            var sql = "UPDATE rooms SET r_number = @roomNumber WHERE r_id = @roomId";
            dbAccess.ExecuteNonQuery(sql, ("@roomNumber", roomNumber), ("@roomId", roomId));
        }

        public void EditRoomFloor(int roomId, int roomFloor)
        {
            var sql = "UPDATE rooms SET r_floor = @roomFloor WHERE r_id = @roomId";
            dbAccess.ExecuteNonQuery(sql, ("@roomFloor", roomFloor), ("@roomId", roomId));
        }
        #endregion

        #region Delete Methods

        public void DeleteRoom(int id)
        {
            var sql = "DELETE FROM rooms WHERE r_id = @id";
            dbAccess.ExecuteNonQuery(sql, ("@id", id));
        }

        #endregion

        #region Get Methods

        public List<Rooms> GetRooms()
        {
            var sql = "SELECT * FROM rooms";

            List<Rooms> rooms = new List<Rooms>();

            using (var cmd = dbAccess.dbDataSource.CreateCommand(sql))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Rooms room = new Rooms()
                        {
                            RoomId = reader.GetInt32(0),
                            RoomName = reader.GetString(1),
                            RoomNumber = reader.GetString(2),
                            RoomFloor = reader.GetInt32(3)

                        };
                        rooms.Add(room);
                    }
                }
            }

            return rooms;
        }

      
        #endregion
    }
}
