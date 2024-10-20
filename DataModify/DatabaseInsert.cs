using System;
using System.Diagnostics;
using Npgsql;

namespace DataModify
{
    internal class DatabaseInsert
    {
        private readonly DatabaseCredentials credentials;
        private NpgsqlDataSource dataSource;

        public DatabaseInsert()
        {
            dataSource = NpgsqlDataSource.Create(credentials.GetconnectionString());
        }

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

        public void InsertRoom(string name, string number, string floor)
        {
            var sql = "INSERT INTO rooms (r_name, r_number, r_floor) VALUES (@name, @number, @floor)";
            ExecuteNonQuery(sql, ("@name", name), ("@number", number), ("@floor", floor));
        }

        public void InsertUser(string name, string email, int userType)
        {
            var sql = "INSERT INTO users (u_name, u_mail, u_type) VALUES (@name, @email, @userType)";
            ExecuteNonQuery(sql, ("@name", name), ("@email", email), ("@userType", userType));
        }

        public void InsertUserCredentials(byte[] emailHash, byte[] passwordHash)
        {
            var sql = "INSERT INTO user_credentials (umail_hash, upass_hash) VALUES (@emailHash, @passwordHash)";
            ExecuteNonQuery(sql, ("@emailHash", emailHash), ("@passwordHash", passwordHash));
        }

        public void InsertUserType(string name, string permissions)
        {
            var sql = "INSERT INTO user_types (ut_name, ut_permissions) VALUES (@name, @permissions)";
            ExecuteNonQuery(sql, ("@name", name), ("@permissions", permissions));
        }

        public void InsertUserHabit(int userId, string eventJson)
        {
            var sql = "INSERT INTO user_habits (u_id, h_event) VALUES (@userId, @eventJson)";
            ExecuteNonQuery(sql, ("@userId", userId), ("@eventJson", eventJson));
        }

        public void InsertPreset(string name, int user, int height, string options)
        {
            var sql = "INSERT INTO presets (p_name, p_user, p_height, p_options) VALUES (@name, @user, @height, @options)";
            ExecuteNonQuery(sql, ("@name", name), ("@user", user), ("@height", height), ("@options", options));
        }

        public void InsertTable(string name, string manufacturer, int api)
        {
            var sql = "INSERT INTO tables (t_name, t_manufacturer, t_api) VALUES (@name, @manufacturer, @api)";
            ExecuteNonQuery(sql, ("@name", name), ("@manufacturer", manufacturer), ("@api", api));
        }

        public void InsertApi(string name, string config)
        {
            var sql = "INSERT INTO apis (a_name, a_config) VALUES (@name, @config)";
            ExecuteNonQuery(sql, ("@name", name), ("@config", config));
        }

      
    }
}
