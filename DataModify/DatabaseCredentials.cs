using System;
using DotNetEnv;
namespace DataModify
{
    public class DatabaseCredentials
    {
        public string User { get; set; }
        public string Password { get; set; }
        public string DbName { get; set; }
        public string Port { get; set; }

        public DatabaseCredentials()
        {
            var envPath = @"../DataAccess/_Setup/.env";
            LoadEnvVariables(envPath);


        }

        private void LoadEnvVariables(string envPath)
        {
            Env.Load(envPath);
            User = Env.GetString("DB_USER");
            Password = Env.GetString("DB_PASSWORD");
            DbName = Env.GetString("DB_NAME");
            Port = Env.GetString("DB_PORT");
        }

        public string GetconnectionString()
        {
            return $"Host=my_host;Port={Port};Database={DbName};User Id={User};Password={Password};";
        }

    }
}
