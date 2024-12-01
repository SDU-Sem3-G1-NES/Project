using SharedModels;


namespace DataAccess
{
    public class HealthRepository
    {
        private readonly DbAccess dbAccess;

        public HealthRepository()
        {
            dbAccess = new DbAccess();
        }

        public HealthRepository(DbAccess dbAccess)
        {
            this.dbAccess = dbAccess;
        }

        #region Insert Methods

        public void InsertHealth(int userId, int? presetID, int position)
        {
            if (presetID != null)
            {
                string sql = "INSERT INTO HEALTH (H_DATE, U_ID, P_ID, H_POSITION) VALUES (@H_DATE, @U_ID, @P_ID, @H_POSITION)";
                dbAccess.ExecuteNonQuery(sql, ("@H_DATE", DateTime.Now), ("@U_ID", userId), ("@P_ID", presetID), ("@H_POSITION", position));
            } else 
            {
                string sql = "INSERT INTO HEALTH (H_DATE, U_ID, H_POSITION) VALUES (@H_DATE, @U_ID, @H_POSITION)";
                dbAccess.ExecuteNonQuery(sql, ("@H_DATE", DateTime.Now), ("@U_ID", userId), ("@H_POSITION", position));
            }

        }

        public List<Health> GetHealthByUser(int userID, DateTime? startDate, DateTime? endtime)
        {
            string sql = "SELECT * FROM health WHERE u_id = @userID";
            if (endtime != null)
            {
                sql += " AND h_date < @endtime";
            }
            if (startDate != null)
            {
                sql += " AND h_date > @startDate";
            }

            List<Health> health = new List<Health>();

            using (var cmd = dbAccess.dbDataSource.CreateCommand())
            {
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@userID", userID);
                if (endtime != null)
                {
                    cmd.Parameters.AddWithValue("@endtime", endtime);
                }
                if (startDate != null)
                {
                    cmd.Parameters.AddWithValue("@startDate", startDate);
                }

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Health healthreport = new Health()
                        {
                            HealthId = reader.GetInt32(0),
                            Date = reader.GetDateTime(1),
                            UserId = reader.GetInt32(2),
                            PresetId = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
                            Position = reader.GetInt32(4)
                        };
                        health.Add(healthreport);
                    }
                }
            }

            return health;
        }


        #endregion


    }
}
