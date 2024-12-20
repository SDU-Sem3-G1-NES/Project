using SharedModels;

namespace DataAccess
{
    public class ApiRepository
    {
        private readonly DbAccess dbAccess;

        public ApiRepository()
        {
            dbAccess = new DbAccess();
        }

        // Constructor for testing.
        public ApiRepository(DbAccess dbAccess)
        {
            this.dbAccess = dbAccess;
        }

        #region Insert Methods

        public void InsertApi(string name, string config)
        {
            var sql = "INSERT INTO apis (a_name, a_config) VALUES (@name, @config::jsonb)";
            dbAccess.ExecuteNonQuery(sql, ("@name", name), ("@config", config));
        }

        #endregion

        #region Edit Methods

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

        #endregion

        #region Delete Methods

        public void DeleteApi(int id)
        {
            var sql = "DELETE FROM apis WHERE a_id = @id";
            dbAccess.ExecuteNonQuery(sql, ("@id", id));
        }

        #endregion

        #region Get Methods

        public List<Apis> GetAllApis()
        {
            var sql = "SELECT * FROM apis";

            List<Apis> apis = new List<Apis>();

            using(var connection = dbAccess.dbDataSource.CreateConnection())
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = sql;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Apis api = new Apis()
                            {
                                apiID = reader.GetInt32(reader.GetOrdinal("a_id")),
                                apiName = reader.GetString(reader.GetOrdinal("a_name")),
                                apiConfig = reader.GetString(reader.GetOrdinal("a_config"))
                            };
                            apis.Add(api);
                        }
                    }
                }
                connection.Close();
            }

            return apis;
        }

        #endregion


    }
}
