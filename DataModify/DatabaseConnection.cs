using System;
using Npgsql;
// I'm not sure if this is the best way to handle the connection string, but it's a start
// I'm going to leave this class in case it's useful later on
// Right now i handle conncetions using NpgsqlDataSource
namespace DataModify
{
    internal class DatabaseConnection
    {
        private readonly DatabaseCredentials credentials;
        private readonly NpgsqlConnection connection;
        private string connectionString;

        public DatabaseConnection(DatabaseCredentials credentials)
        {
            this.credentials = credentials;
            connectionString = $"Host=my_host;Port={credentials.Port};Database={credentials.DbName};User Id={credentials.User};Password={credentials.Password};";
            connection = new NpgsqlConnection(connectionString);
        }

        public void OpenConnection()
        {
            connection.Open();
        }

        public void CloseConnection()
        {
            connection.Close();
        }
    }
}
