using System;
using Npgsql;

namespace DataModify
{
    internal class DatabaseConnection
    {
        DatabaseCredentials credentials = new DatabaseCredentials();
        string connectionString;

        public DatabaseConnection()
        {
            connectionString = $"Host=my_host;Port={credentials.Port};Database={credentials.DbName};User Id={credentials.User};Password={credentials.Password};";
        }

        public void OpenConnection()
        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();
        }

        public void CloseConnection() 
        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Close();
        }
    }
}
