using System;
using System.Diagnostics;
using Npgsql;

namespace DataModify
{
    internal class DatabaseDelete
    {
        private readonly DatabaseCredentials credentials;
        private NpgsqlDataSource dataSource;

        public DatabaseDelete()
        {
            credentials = new DatabaseCredentials();
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

        public void DeleteRoom(int id)
        {
            var sql = "DELETE FROM rooms WHERE r_id = @id";
            ExecuteNonQuery(sql, ("@id", id));
        }

        public void DeleteUser(int id)
        {
            var sql = "DELETE FROM users WHERE u_id = @id";
            ExecuteNonQuery(sql, ("@id", id));
        }

        public void DeleteUserCredentials(int id)
        {
            var sql = "DELETE FROM user_credentials WHERE uc_id = @id";
            ExecuteNonQuery(sql, ("@id", id));
        }

        public void DeleteUserType(int id)
        {
            var sql = "DELETE FROM user_types WHERE ut_id = @id";
            ExecuteNonQuery(sql, ("@id", id));
        }

        public void DeleteUserHabit(int id)
        {
            var sql = "DELETE FROM user_habits WHERE uh_id = @id";
            ExecuteNonQuery(sql, ("@id", id));
        }

        public void DeletePreset(int id)
        {
            var sql = "DELETE FROM presets WHERE p_id = @id";
            ExecuteNonQuery(sql, ("@id", id));
        }

        public void DeleteTable(int id)
        {
            var sql = "DELETE FROM tables WHERE t_id = @id";
            ExecuteNonQuery(sql, ("@id", id));
        }

        public void DeleteApi(int id)
        {
            var sql = "DELETE FROM apis WHERE a_id = @id";
            ExecuteNonQuery(sql, ("@id", id));
        }
    }
}
