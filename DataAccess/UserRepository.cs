using SharedModels;
namespace DataAccess
{
    public class UserRepository
    {
        private readonly DbAccess dbAccess;

        public UserRepository()
        {
            dbAccess = new DbAccess();
        }
        // Constructor for testing.
        public UserRepository(DbAccess dbAccess)
        {
            this.dbAccess = dbAccess;
        }

        #region Insert Methods
        public void InsertUser(string name, string email, int userType)
        {
            var sql = "INSERT INTO users (u_name, u_mail, u_type) VALUES (@name, @email, @userType)";
            dbAccess.ExecuteNonQuery(sql, ("@name", name), ("@email", email), ("@userType", userType));
        }

        public void InsertUserCredentials(byte[] emailHash, byte[] passwordHash)
        {
            var sql = "INSERT INTO user_credentials (umail_hash, upass_hash) VALUES (@emailHash, @passwordHash)";
            dbAccess.ExecuteNonQuery(sql, ("@emailHash", emailHash), ("@passwordHash", passwordHash));
        }

        public void InsertUserType(string name, string permissions)
        {
            var sql = "INSERT INTO user_types (ut_name, ut_permissions) VALUES (@name, @permissions::jsonb)";
            dbAccess.ExecuteNonQuery(sql, ("@name", name), ("@permissions", permissions));
        }

        public void InsertUserHabit(int userId, string eventJson)
        {
            var sql = "INSERT INTO user_habits (u_id, h_event) VALUES (@userId, @eventJson)";
            dbAccess.ExecuteNonQuery(sql, ("@userId", userId), ("@eventJson", eventJson));
        }
        #endregion

        #region Edit Methods

        public void EditUserTypeName(int userTypeId, string userTypeName)
        {
            var sql = "UPDATE user_types SET ut_name = @userTypeName WHERE ut_id = @userTypeId";
            dbAccess.ExecuteNonQuery(sql, ("@userTypeName", userTypeName), ("@userTypeId", userTypeId));
        }

        public void EditUserTypePermissions(int userTypeId, string userTypePermissions)
        {
            var sql = "UPDATE user_types SET ut_permissions = @userTypePermissions WHERE ut_id = @userTypeId";
            dbAccess.ExecuteNonQuery(sql, ("@userTypePermissions", userTypePermissions), ("@userTypeId", userTypeId));
        }

        public void EditUserName(int userId, string userName)
        {
            var sql = "UPDATE users SET u_name = @userName WHERE u_id = @userId";
            dbAccess.ExecuteNonQuery(sql, ("@userName", userName), ("@userId", userId));
        }

        public void EditUserMail(int userId, string userMail)
        {
            var sql = "UPDATE users SET u_mail = @userMail WHERE u_id = @userId";
            dbAccess.ExecuteNonQuery(sql, ("@userMail", userMail), ("@userId", userId));
        }

        public void EditUserType(int userId, string userType)
        {
            var sql = "UPDATE users SET u_type = @userType WHERE u_id = @userId";
            dbAccess.ExecuteNonQuery(sql, ("@userType", userType), ("@userId", userId));
        }

        // Method to update user table
        public void EditUserTable(int userId, string tableId)
        {
            var sql = "UPDATE user_tables SET t_guid = @tableId WHERE u_id = @userId";
            dbAccess.ExecuteNonQuery(sql, ("@tableId", tableId), ("@userId", userId));
        }

        // Method for editing habit event by habit id
        public void EditHabitEvent(int habitId, string habitEvent)
        {
            var sql = "UPDATE user_habits SET t_guid = @habitEvent WHERE h_id = @habitId";
            dbAccess.ExecuteNonQuery(sql, ("@habitEvent", habitEvent), ("@habitId", habitId));
        }

        public void EditHashPass(string mail_hash, string newPass_hash)
        {
            var sql = "UPDATE user_credentials SET upass_hash = @pass WHERE umail_hash = @mail";
            dbAccess.ExecuteNonQuery(sql, ("@pass", newPass_hash), ("@mail", mail_hash));
        }

        #endregion

        #region Delete Methods

        public void DeleteUser(int id)
        {
            var sql = "DELETE FROM users WHERE u_id = @id";
            dbAccess.ExecuteNonQuery(sql, ("@id", id));
        }

        public void DeleteUserCredentials(int id)
        {
            var sql = "DELETE FROM user_credentials WHERE uc_id = @id";
            dbAccess.ExecuteNonQuery(sql, ("@id", id));
        }

        public void DeleteUserType(int id)
        {
            var sql = "DELETE FROM user_types WHERE ut_id = @id";
            dbAccess.ExecuteNonQuery(sql, ("@id", id));
        }

        public void DeleteUserHabit(int id)
        {
            var sql = "DELETE FROM user_habits WHERE uh_id = @id";
            dbAccess.ExecuteNonQuery(sql, ("@id", id));
        }

        #endregion

        #region Get Methods

        public List<Employee> GetAllUsers()
        {
            var sql = $"SELECT u_id,u_name,u_mail FROM users";

            List<Employee> Users = new List<Employee>();

            using (var cmd = dbAccess.dbDataSource.CreateCommand(sql))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        Employee employee = new Employee
                        {
                            UserID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Email = reader.GetString(2),

                        };
                        Users.Add(employee);
                    }
                }
            }

            return Users;

        }

        // Todo: Implement fetching user by email
        public List<Employee> GetEmployee(string? email = null)
        {
            var sql = $"SELECT u.u_id,u.u_name,u.u_mail FROM users as u INNER JOIN user_types AS ut ON u.u_type = ut.ut_id WHERE ut.ut_name = 'EMPLOYEE'";

            List<Employee> Employees = new List<Employee>();

            using (var cmd = dbAccess.dbDataSource.CreateCommand(sql))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        
                        Employee employee = new Employee
                        {
                            UserID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Email = reader.GetString(2),

                        };
                        Employees.Add(employee);
                    }
                }
            }

            return Employees;

        }
        public List<Admin> GetAdmin(string email)
        {
            var sql = $"SELECT u.u_id,u.u_name,u.u_mail FROM users as u INNER JOIN user_types AS ut ON u.u_type = ut.ut_id WHERE ut.ut_name = 'ADMIN'";
            List<Admin> Admins = new List<Admin>();

            using (var cmd = dbAccess.dbDataSource.CreateCommand(sql))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        
                        Admin admin = new Admin
                        {
                            UserID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Email = reader.GetString(2)

                        };
                        Admins.Add(admin);
                    }
                }
            }

            return Admins;

        }
        public List<Cleaner> GetCleaner(string email)
        {
            var sql = $"SELECT u.u_id,u.u_name,u.u_mail FROM users as u INNER JOIN user_types AS ut ON u.u_type = ut.ut_id WHERE ut.ut_name = 'CLEANER'";

            List<Cleaner> Cleaners = new List<Cleaner>();

            using (var cmd = dbAccess.dbDataSource.CreateCommand(sql))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        
                        Cleaner cleaner = new Cleaner
                        {
                            UserID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Email = reader.GetString(2),

                        };
                        Cleaners.Add(cleaner);
                    }
                }
            }

            return Cleaners;

        }

        public string? GetHashedPassword(string hashedEmailHex)
        {
            string sql = "SELECT upass_hash FROM user_credentials WHERE umail_hash = decode(@hashedEmailHex, 'hex')";

            using (var cmd = dbAccess.dbDataSource.CreateCommand(sql))
            {
                cmd.Parameters.Add("@hashedEmailHex", NpgsqlTypes.NpgsqlDbType.Varchar).Value = hashedEmailHex;

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        byte[] passwordHashBytes = (byte[])reader["upass_hash"];
                        
                        return BitConverter.ToString(passwordHashBytes).Replace("-", "").ToLower();
                    }
                }
            }
            return null;
        }
        
        #endregion

    }

}
