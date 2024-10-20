using System;
using System.Diagnostics;
using Npgsql;

namespace DataModify
{
    internal class DatabaseModify
    {
        private readonly DatabaseCredentials credentials;
        private NpgsqlDataSource dataSource;

        public DatabaseModify()
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


        // You can moddify the table by passing the table name, record id and the fields you want to modify
        // You pass fields as a tuple with the field name and the value you want to set
        public void ModifyRecord(string tableName, int recordId, params (string, object)[] fields)
        {
            var sql = $"UPDATE {tableName} SET ";
            var parameters = new List<(string, object)>();

            foreach (var (fieldName, value) in fields)
            {
                sql += $"{fieldName} = @{fieldName}, ";
                parameters.Add((fieldName, value));
            }

            sql = sql.TrimEnd(',', ' ');
            sql += $" WHERE id = @recordId";
            parameters.Add(("recordId", recordId));

            ExecuteNonQuery(sql, parameters.ToArray());
        }
    }
}
