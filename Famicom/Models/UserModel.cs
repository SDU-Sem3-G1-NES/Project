using SharedModels;
using Models.Services;

namespace Famicom.Models
{
    public class UserModel
    {
        private readonly UserService userService;
        public IUser? user;

        public UserModel()
        {
            this.userService = new UserService();
        }
        public IUser? GetUser(string? email = null, int? userId = null)
        {
            this.user = userService.GetUser(email, userId);
            return this.user;
        }
        
        // Mock here as well logic later
        public string GetUserType()
        {


            return "Admin";

        }
    }
}