using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModify
{
    internal class RoomRepository
    {
        private readonly DbAccess dbAccess;

        public RoomRepository()
        {
            dbAccess = new DbAccess();
        }

        #region Insert Methods
        public void InsertRoom(string name, string number, int floor)
        {
            var sql = "INSERT INTO rooms (r_name, r_number, r_floor) VALUES (@name, @number, @floor)";
            dbAccess.ExecuteNonQuery(sql, ("@name", name), ("@number", number), ("@floor", floor));
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
    }
}
