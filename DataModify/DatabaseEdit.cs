using System.Diagnostics;
using Npgsql;

namespace DataModify
{
    internal class DatabaseEdit
    {
        private readonly DatabaseCredentials credentials;
        private NpgsqlDataSource dataSource;

        private void ExecuteNonQuery(string sql, params (string, object)[] parameters)
        {
            try
            {
                using (var cmd = dataSource.CreateCommand(sql))
                {
                    foreach (var (name, value) in parameters)
                    {
                        cmd.Parameters.AddWithValue(name, value);
                    }
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }
        public DatabaseEdit()
        {
            dataSource = NpgsqlDataSource.Create(credentials.GetconnectionString());
        }
        // 2 methods to update api's.
        public void EditApiName(int apiId, string apiName)
        {
            var sql = "UPDATE apis SET a_name = @apiName WHERE a_id = @apiId";
            ExecuteNonQuery(sql, ("@apiName", apiName), ("@apiId", apiId));
        }

        public void EditApiConfig(int apiId, string apiConfig)
        {
            var sql = "UPDATE apis SET a_config = @apiConfig WHERE a_id = @apiId";
            ExecuteNonQuery(sql, ("@apiConfig", apiConfig), ("@apiId", apiId));
        }

        // 4 methods to update presets
        public void EditPresetName(int presetId, string presetName)
        {
            var sql = "UPDATE presets SET p_name = @presetName WHERE p_id = @presetId";
            ExecuteNonQuery(sql, ("@presetName", presetName), ("@presetId", presetId));
        }

        public void EditPresetUser(int presetId, int presetUser)
        {
            var sql = "UPDATE presets SET p_user = @presetUser WHERE p_id = @presetId";
            ExecuteNonQuery(sql, ("@presetUser", presetUser), ("@presetId", presetId));
        }

        public void EditPresetHeight(int presetId, int presetHeight)
        {
            var sql = "UPDATE presets SET p_height = @presetHeight WHERE p_id = @presetId";
            ExecuteNonQuery(sql, ("@presetHeight", presetHeight), ("@presetId", presetId));
        }

        public void EditPresetOptions(int presetId, string presetOptions)
        {
            var sql = "UPDATE presets SET p_options = @presetOptions WHERE p_id = @presetId";
            ExecuteNonQuery(sql, ("@presetOptions", presetOptions), ("@presetId", presetId));
        }

        // Method to update room table
        public void EditRoomTable(int roomId, int tableId)
        {
            var sql = "UPDATE room_tables SET t_id = @tableId WHERE r_id = @roomId";
            ExecuteNonQuery(sql, ("@tableId", tableId), ("@roomId", roomId));
        }

        // 3 methods to update rooms
        public void EditRoomName(int roomId, string roomName)
        {
            var sql = "UPDATE rooms SET r_name = @roomName WHERE r_id = @roomId";
            ExecuteNonQuery(sql, ("@roomName", roomName), ("@roomId", roomId));
        }

        public void EditRoomNumber(int roomId, string roomNumber)
        {
            var sql = "UPDATE rooms SET r_number = @roomNumber WHERE r_id = @roomId";
            ExecuteNonQuery(sql, ("@roomNumber", roomNumber), ("@roomId", roomId));
        }

        public void EditRoomFloor(int roomId, int roomFloor)
        {
            var sql = "UPDATE rooms SET r_floor = @roomFloor WHERE r_id = @roomId";
            ExecuteNonQuery(sql, ("@roomFloor", roomFloor), ("@roomId", roomId));
        }

        // Method to update table for schedule
        public void EditScheduleTable(int scheduleId, int tableId)
        {
            var sql = "UPDATE schedule_tables SET t_id = @tableId WHERE s_id = @scheduleId";
            ExecuteNonQuery(sql, ("@tableId", tableId), ("@scheduleId", scheduleId));
        }

        // 3 methods to update schedules
        public void EditScheduleName(int scheduleId, string scheduleName)
        {
            var sql = "UPDATE schedules SET s_name = @scheduleName WHERE s_id = @scheduleId";
            ExecuteNonQuery(sql, ("@scheduleName", scheduleName), ("@scheduleId", scheduleId));
        }

        public void EditScheduleConfig(int scheduleId, string scheduleConfig)
        {
            var sql = "UPDATE schedules SET s_config = @scheduleConfig WHERE s_id = @scheduleId";
            ExecuteNonQuery(sql, ("@scheduleConfig", scheduleConfig), ("@scheduleId", scheduleId));
        }

        public void EditScheduleOwner(int scheduleId, string scheduleOwner)
        {
            var sql = "UPDATE schedules SET s_owner = @scheduleOwner WHERE s_id = @scheduleId";
            ExecuteNonQuery(sql, ("@scheduleOwner", scheduleOwner), ("@scheduleId", scheduleId));
        }

        // Methods for user_types
        public void EditUserTypeName(int userTypeId, string userTypeName)
        {
            var sql = "UPDATE user_types SET ut_name = @userTypeName WHERE ut_id = @userTypeId";
            ExecuteNonQuery(sql, ("@userTypeName", userTypeName), ("@userTypeId", userTypeId));
        }

        public void EditUserTypePermissions(int userTypeId, string userTypePermissions)
        {
            var sql = "UPDATE user_types SET ut_permissions = @userTypePermissions WHERE ut_id = @userTypeId";
            ExecuteNonQuery(sql, ("@userTypePermissions", userTypePermissions), ("@userTypeId", userTypeId));
        }

        // Methods for users
        public void EditUserName(int userId, string userName)
        {
            var sql = "UPDATE users SET u_name = @userName WHERE u_id = @userId";
            ExecuteNonQuery(sql, ("@userName", userName), ("@userId", userId));
        }

        public void EditUserMail(int userId, string userMail)
        {
            var sql = "UPDATE users SET u_mail = @userMail WHERE u_id = @userId";
            ExecuteNonQuery(sql, ("@userMail", userMail), ("@userId", userId));
        }

        public void EditUserType(int userId, string userType)
        {
            var sql = "UPDATE users SET u_type = @userType WHERE u_id = @userId";
            ExecuteNonQuery(sql, ("@userType", userType), ("@userId", userId));
        }

        // Method to update user table
        public void EditUserTable(int userId, int tableId)
        {
            var sql = "UPDATE user_tables SET t_id = @tableId WHERE u_id = @userId";
            ExecuteNonQuery(sql, ("@tableId", tableId), ("@userId", userId));
        }

        // Method for editing habit event by habit id
        public void EditHabitEvent(int habitId, string habitEvent)
        {
            var sql = "UPDATE user_habits SET t_id = @habitEvent WHERE h_id = @habitId";
            ExecuteNonQuery(sql, ("@habitEvent", habitEvent), ("@habitId", habitId));
        }

        // 3 methods for editing tables by table id
        public void EditTableName(int tableId, string tableName)
        {
            var sql = "UPDATE tables SET t_name = @tableName WHERE t_id = @tableId";
            ExecuteNonQuery(sql, ("@tableName", tableName), ("@tableId", tableId));
        }

        public void EditTableManufacturer(int tableId, string tableManufacturer)
        {
            var sql = "UPDATE tables SET t_manufacturer = @tableManufacturer WHERE t_id = @tableId";
            ExecuteNonQuery(sql, ("@tableManufacturer", tableManufacturer), ("@tableId", tableId));
        }

        public void EditTableAPI(int tableId, int tableApi)
        {
            var sql = "UPDATE tables SET t_api = @tableApi WHERE t_id = @tableId";
            ExecuteNonQuery(sql, ("@tableApi", tableApi), ("@tableId", tableId));
        }

    }
}
