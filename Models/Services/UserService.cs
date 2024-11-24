using DataAccess;
using SharedModels;

namespace Models.Services
{
    public class UserService
    {
        private readonly UserRepository userRepository;

        public UserService()
        {
            this.userRepository = new UserRepository();
        }

        public void AddUser(string name, string email, int userType)
        {
            userRepository.InsertUser(name, email, userType);
        }

        public void AddUserCredentials(byte[] emailHash, byte[] passwordHash)
        {
            userRepository.InsertUserCredentials(emailHash, passwordHash);
        }

        public void AddUserType(string name, string permissions)
        {
            userRepository.InsertUserType(name, permissions);
        }

        public void AddUserHabit(int userId, string eventJson)
        {
            userRepository.InsertUserHabit(userId, eventJson);
        }

        public void UpdateUserTypeName(int userTypeId, string userTypeName)
        {
            userRepository.EditUserTypeName(userTypeId, userTypeName);
        }

        public void UpdateUserTypePermissions(int userTypeId, string userTypePermissions)
        {
            userRepository.EditUserTypePermissions(userTypeId, userTypePermissions);
        }

        public void UpdateUserName(int userId, string userName)
        {
            userRepository.EditUserName(userId, userName);
        }

        public void UpdateUserMail(int userId, string userMail)
        {
            userRepository.EditUserMail(userId, userMail);
        }

        public void UpdateUserType(int userId, string userType)
        {
            userRepository.EditUserType(userId, userType);
        }

        public void UpdateUserTable(int userId, string tableId)
        {
            userRepository.EditUserTable(userId, tableId);
        }

        public void UpdateHabitEvent(int habitId, string habitEvent)
        {
            userRepository.EditHabitEvent(habitId, habitEvent);
        }

        public void UpdateHashPass(string mailHash, string newPassHash)
        {
            userRepository.EditHashPass(mailHash, newPassHash);
        }

        public void RemoveUser(int id)
        {
            userRepository.DeleteUser(id);
        }

        public void RemoveUserCredentials(int id)
        {
            userRepository.DeleteUserCredentials(id);
        }

        public void RemoveUserType(int id)
        {
            userRepository.DeleteUserType(id);
        }

        public void RemoveUserHabit(int id)
        {
            userRepository.DeleteUserHabit(id);
        }

        public List<Employee> GetEmployees()
        {
            return userRepository.GetEmployee();
        }

        public List<Admin> GetAdmins(string email)
        {
            return userRepository.GetAdmin(email);
        }

        public List<Cleaner> GetCleaners(string email)
        {
            return userRepository.GetCleaner(email);
        }

        public List<Employee> GetAllUsers()
        {
            return userRepository.GetAllUsers();
        }
    }
}