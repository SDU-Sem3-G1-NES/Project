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

            Env.Load(envPath);

            User = Env.GetString("DB_USER");

            Password = Env.GetString("DB_PASSWORD");

            DbName = Env.GetString("DB_NAME");

            Port = Env.GetString("DB_PORT");

        }
    }
}
