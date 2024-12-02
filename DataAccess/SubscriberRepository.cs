using SharedModels;

namespace DataAccess
{
    public class SubscriberRepository
    {
        private readonly DbAccess dbAccess;

        public SubscriberRepository()
        {
            dbAccess = new DbAccess();
        }
        // Constructor for testing.
        public SubscriberRepository(DbAccess dbAccess)
        {
            this.dbAccess = dbAccess;
        }

        #region Insert Methods

        public void InsertSubscriber(string tableGuid, string uri)
        {
            var sql = "INSERT INTO subscribers (t_guid, s_uri) VALUES (@tableGUID, @uri)";
            dbAccess.ExecuteNonQuery(sql, ("@tableGUID", tableGuid), ("@uri", uri));
        }

        #endregion

        #region Edit Methods

        public void EditSubscriberUri(string tableGuid, string uri)
        {
            var sql = "UPDATE subscribers SET s_uri = @uri WHERE t_guid = @tableGUID";
            dbAccess.ExecuteNonQuery(sql, ("@uri", uri), ("@tableGUID", tableGuid));
        }
        #endregion

        #region Delete Methods

        public void DeleteSubscriber(string tableGuid)
        {
            var sql = "DELETE FROM subscribers WHERE t_guid = @tableGUID";
            dbAccess.ExecuteNonQuery(sql, ("@tableGUID", tableGuid));
        }
        #endregion

        #region Get Methods

        public string? GetSubscriber(string tableGuid)
        {
            var sql = $"SELECT s_uri FROM subscribers WHERE t_guid = @x";

            using(var connection = dbAccess.dbDataSource.CreateConnection())
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@x", tableGuid);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            connection.Close();
                            return reader.GetString(reader.GetOrdinal("s_uri"));
                        }
                    }
                }
                connection.Close();
            }

            return null;
        }

        #endregion
    }
}