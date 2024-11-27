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

        public IUser? GetUser(string email)
        {
            var sql = $"SELECT u_id,u_name,u_mail,u_type FROM users WHERE u_mail = @email";

            using(var cmd = dbAccess.dbDataSource.CreateCommand(sql))
            {
                cmd.Parameters.AddWithValue("@email", email);

                using (var reader = cmd.ExecuteReader())
                {
                    if(reader.Read())
                    {
                        int userId = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        string userEmail = reader.GetString(2);
                        int userType = reader.GetInt32(3);

                        switch (userType)
                        {
                            case 1:
                                return new Admin
                                {
                                    UserID = userId,
                                    Name = name,
                                    Email = userEmail
                                };
                            case 2:
                                return new Employee
                                {
                                    UserID = userId,
                                    Name = name,
                                    Email = userEmail
                                };
                            case 3:
                                return new Cleaner
                                {
                                    UserID = userId,
                                    Name = name,
                                    Email = userEmail
                                };
                            default:
                                throw new ArgumentException("Invalid user type");
                        }
                    }
                    return null;
                }
            }
        }

        public List<IUser> GetAllUsers()
        {
            var sql = $"SELECT u_id,u_name,u_mail,u_type FROM users";

            List<IUser> users = new List<IUser>();

            using (var cmd = dbAccess.dbDataSource.CreateCommand(sql))
            {

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int userId = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        string userEmail = reader.GetString(2);
                        int userType = reader.GetInt32(3);

                        if (userType == 1)
                        {
                            Admin admin = new Admin
                            {
                                UserID = userId,
                                Name = name,
                                Email = userEmail
                            };
                            users.Add(admin);
                        }
                        else if (userType == 2)
                        {
                            Employee employe = new Employee
                            {
                                UserID = userId,
                                Name = name,
                                Email = userEmail
                            };
                            users.Add(employe);
                        }
                        else if (userType == 3)
                        {
                            Cleaner cleaner = new Cleaner
                            {
                                UserID = userId,
                                Name = name,
                                Email = userEmail
                            };
                            users.Add(cleaner);
                        }
                        else
                        {
                            throw new ArgumentException("Invalid user type");
                        }
                    }
                }
            }
            return users;
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
