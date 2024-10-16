using System;
using Npgsql;

namespace DataModify
{
    internal class DatabaseInsert
    {
        private readonly DatabaseConnection connection;
        private readonly NpgsqlCommand command;

        public DatabaseInsert(DatabaseConnection connection)
        {
            this.connection = connection;
            command = new NpgsqlCommand();
        }

        public void InsertRoom(string name, string number, string floor)
        {
            try
            {
                connection.OpenConnection();
                command.CommandText = "INSERT INTO rooms (r_name, r_number, r_floor) VALUES (@name, @number, @floor)";
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@number", number);
                command.Parameters.AddWithValue("@floor", floor);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                connection.CloseConnection();
            }
        }

        public void InsertUsers(string name, string mail, string userType)
        {
            try
            {
                connection.OpenConnection();
                command.CommandText = "INSERT INTO users (u_name, u_mail, u_type) VALUES (@name, @mail, @userType";
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@mail", mail);
                command.Parameters.AddWithValue("@userType", userType);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                connection.CloseConnection();
            }
        }
    }
}
