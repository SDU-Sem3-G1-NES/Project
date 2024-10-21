using System;
using System.Diagnostics;
using Npgsql;

namespace DataModify
{
    internal class QueryBuilder
    {
        private readonly DbAccess dbAccess;

        public QueryBuilder()
        {
            dbAccess = new DbAccess();
        }


        // INSERTION
        public void InsertRoom(string name, string number, int floor)
        {
            var sql = "INSERT INTO rooms (r_name, r_number, r_floor) VALUES (@name, @number, @floor)";
            dbAccess.ExecuteNonQuery(sql, ("@name", name), ("@number", number), ("@floor", floor));
        }

        public void InsertUser(string name, string email, int userType)
        {
            var sql = "INSERT INTO users (u_name, u_mail, u_type) VALUES (@name, @email, @userType)";
            dbAccess.ExecuteNonQuery(sql, ("@name", name), ("@email", email), ("@userType", userType));
        }

        public void InsertUserCredentials(byte[] emailHash, byte[] passwordHash)
        {
            var sql = "INSERT INTO user_credentials (umail_hash, upass_hash) VALUES (@emailHash, @passwordHash)";
            dbAccess.ExecuteNonQuery(sql, ("@emailHash", emailHash), ("@passwordHash", passwordHash));
        }

        public void InsertUserType(string name, string permissions)
        {
            var sql = "INSERT INTO user_types (ut_name, ut_permissions) VALUES (@name, @permissions)";
            dbAccess.ExecuteNonQuery(sql, ("@name", name), ("@permissions", permissions));
        }

        public void InsertUserHabit(int userId, string eventJson)
        {
            var sql = "INSERT INTO user_habits (u_id, h_event) VALUES (@userId, @eventJson)";
            dbAccess.ExecuteNonQuery(sql, ("@userId", userId), ("@eventJson", eventJson));
        }

        public void InsertPreset(string name, int user, int height, string options)
        {
            var sql = "INSERT INTO presets (p_name, p_user, p_height, p_options) VALUES (@name, @user, @height, @options)";
            dbAccess.ExecuteNonQuery(sql, ("@name", name), ("@user", user), ("@height", height), ("@options", options));
        }

        public void InsertTable(string name, string manufacturer, int api)
        {
            var sql = "INSERT INTO tables (t_name, t_manufacturer, t_api) VALUES (@name, @manufacturer, @api)";
            dbAccess.ExecuteNonQuery(sql, ("@name", name), ("@manufacturer", manufacturer), ("@api", api));
        }

        public void InsertApi(string name, string config)
        {
            var sql = "INSERT INTO apis (a_name, a_config) VALUES (@name, @config)";
            dbAccess.ExecuteNonQuery(sql, ("@name", name), ("@config", config));
        }

        
        // MODIFICATION
        public void EditApiName(int apiId, string apiName)
        {
            var sql = "UPDATE apis SET a_name = @apiName WHERE a_id = @apiId";
            dbAccess.ExecuteNonQuery(sql, ("@apiName", apiName), ("@apiId", apiId));
        }

        public void EditApiConfig(int apiId, string apiConfig)
        {
            var sql = "UPDATE apis SET a_config = @apiConfig WHERE a_id = @apiId";
            dbAccess.ExecuteNonQuery(sql, ("@apiConfig", apiConfig), ("@apiId", apiId));
        }

        // 4 methods to update presets
        public void EditPresetName(int presetId, string presetName)
        {
            var sql = "UPDATE presets SET p_name = @presetName WHERE p_id = @presetId";
            dbAccess.ExecuteNonQuery(sql, ("@presetName", presetName), ("@presetId", presetId));
        }

        public void EditPresetUser(int presetId, int presetUser)
        {
            var sql = "UPDATE presets SET p_user = @presetUser WHERE p_id = @presetId";
            dbAccess.ExecuteNonQuery(sql, ("@presetUser", presetUser), ("@presetId", presetId));
        }

        public void EditPresetHeight(int presetId, int presetHeight)
        {
            var sql = "UPDATE presets SET p_height = @presetHeight WHERE p_id = @presetId";
            dbAccess.ExecuteNonQuery(sql, ("@presetHeight", presetHeight), ("@presetId", presetId));
        }

        public void EditPresetOptions(int presetId, string presetOptions)
        {
            var sql = "UPDATE presets SET p_options = @presetOptions WHERE p_id = @presetId";
            dbAccess.ExecuteNonQuery(sql, ("@presetOptions", presetOptions), ("@presetId", presetId));
        }

        // Method to update room table
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

        // Method to update table for schedule
        public void EditScheduleTable(int scheduleId, int tableId)
        {
            var sql = "UPDATE schedule_tables SET t_id = @tableId WHERE s_id = @scheduleId";
            dbAccess.ExecuteNonQuery(sql, ("@tableId", tableId), ("@scheduleId", scheduleId));
        }

        // 3 methods to update schedules
        public void EditScheduleName(int scheduleId, string scheduleName)
        {
            var sql = "UPDATE schedules SET s_name = @scheduleName WHERE s_id = @scheduleId";
            dbAccess.ExecuteNonQuery(sql, ("@scheduleName", scheduleName), ("@scheduleId", scheduleId));
        }

        public void EditScheduleConfig(int scheduleId, string scheduleConfig)
        {
            var sql = "UPDATE schedules SET s_config = @scheduleConfig WHERE s_id = @scheduleId";
            dbAccess.ExecuteNonQuery(sql, ("@scheduleConfig", scheduleConfig), ("@scheduleId", scheduleId));
        }

        public void EditScheduleOwner(int scheduleId, string scheduleOwner)
        {
            var sql = "UPDATE schedules SET s_owner = @scheduleOwner WHERE s_id = @scheduleId";
            dbAccess.ExecuteNonQuery(sql, ("@scheduleOwner", scheduleOwner), ("@scheduleId", scheduleId));
        }

        // Methods for user_types
        public void EditUserTypeName(int userTypeId, string userTypeName)
        {
            var sql = "UPDATE user_types SET ut_name = @userTypeName WHERE ut_id = @userTypeId";
            dbAccess.ExecuteNonQuery(sql, ("@userTypeName", userTypeName), ("@userTypeId", userTypeId));
        }

        public void EditUserTypePermissions(int userTypeId, string userTypePermissions)
        {
            var sql = "UPDATE user_types SET ut_permissions = @userTypePermissions WHERE ut_id = @userTypeId";
            dbAccess.ExecuteNonQuery(sql, ("@userTypePermissions", userTypePermissions), ("@userTypeId", userTypeId));
        }

        // Methods for users
        public void EditUserName(int userId, string userName)
        {
            var sql = "UPDATE users SET u_name = @userName WHERE u_id = @userId";
            dbAccess.ExecuteNonQuery(sql, ("@userName", userName), ("@userId", userId));
        }

        public void EditUserMail(int userId, string userMail)
        {
            var sql = "UPDATE users SET u_mail = @userMail WHERE u_id = @userId";
            dbAccess.ExecuteNonQuery(sql, ("@userMail", userMail), ("@userId", userId));
        }

        public void EditUserType(int userId, string userType)
        {
            var sql = "UPDATE users SET u_type = @userType WHERE u_id = @userId";
            dbAccess.ExecuteNonQuery(sql, ("@userType", userType), ("@userId", userId));
        }

        // Method to update user table
        public void EditUserTable(int userId, int tableId)
        {
            var sql = "UPDATE user_tables SET t_id = @tableId WHERE u_id = @userId";
            dbAccess.ExecuteNonQuery(sql, ("@tableId", tableId), ("@userId", userId));
        }

        // Method for editing habit event by habit id
        public void EditHabitEvent(int habitId, string habitEvent)
        {
            var sql = "UPDATE user_habits SET t_id = @habitEvent WHERE h_id = @habitId";
            dbAccess.ExecuteNonQuery(sql, ("@habitEvent", habitEvent), ("@habitId", habitId));
        }

        // 3 methods for editing tables by table id
        public void EditTableName(int tableId, string tableName)
        {
            var sql = "UPDATE tables SET t_name = @tableName WHERE t_id = @tableId";
            dbAccess.ExecuteNonQuery(sql, ("@tableName", tableName), ("@tableId", tableId));
        }

        public void EditTableManufacturer(int tableId, string tableManufacturer)
        {
            var sql = "UPDATE tables SET t_manufacturer = @tableManufacturer WHERE t_id = @tableId";
            dbAccess.ExecuteNonQuery(sql, ("@tableManufacturer", tableManufacturer), ("@tableId", tableId));
        }

        public void EditTableAPI(int tableId, int tableApi)
        {
            var sql = "UPDATE tables SET t_api = @tableApi WHERE t_id = @tableId";
            dbAccess.ExecuteNonQuery(sql, ("@tableApi", tableApi), ("@tableId", tableId));
        }
        // Method for changing hashed password.
        public void EditHashPass(string mail_hash, string newPass_hash)
        {
            var sql = "UPDATE user_credentials SET upass_hash = @pass WHERE umail_hash = @mail";
            dbAccess.ExecuteNonQuery(sql, ("@pass", newPass_hash), ("@mail", mail_hash));
        }


        // DELETION
        public void DeleteRoom(int id)
        {
            var sql = "DELETE FROM rooms WHERE r_id = @id";
            dbAccess.ExecuteNonQuery(sql, ("@id", id));
        }

        public void DeleteUser(int id)
        {
            var sql = "DELETE FROM users WHERE u_id = @id";
            dbAccess.ExecuteNonQuery(sql, ("@id", id));
        }

        public void DeleteUserCredentials(int id)
        {
            var sql = "DELETE FROM user_credentials WHERE uc_id = @id";
            dbAccess.ExecuteNonQuery(sql, ("@id", id));
        }

        public void DeleteUserType(int id)
        {
            var sql = "DELETE FROM user_types WHERE ut_id = @id";
            dbAccess.ExecuteNonQuery(sql, ("@id", id));
        }

        public void DeleteUserHabit(int id)
        {
            var sql = "DELETE FROM user_habits WHERE uh_id = @id";
            dbAccess.ExecuteNonQuery(sql, ("@id", id));
        }

        public void DeletePreset(int id)
        {
            var sql = "DELETE FROM presets WHERE p_id = @id";
            dbAccess.ExecuteNonQuery(sql, ("@id", id));
        }

        public void DeleteTable(int id)
        {
            var sql = "DELETE FROM tables WHERE t_id = @id";
            dbAccess.ExecuteNonQuery(sql, ("@id", id));
        }

        public void DeleteApi(int id)
        {
            var sql = "DELETE FROM apis WHERE a_id = @id";
            dbAccess.ExecuteNonQuery(sql, ("@id", id));
        }
    }
}
