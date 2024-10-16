using System;
using Npgsql;

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
