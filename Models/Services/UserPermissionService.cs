using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharedModels;

namespace Models.Services
{
    public class UserPermissionService
    {
        private readonly UserService _userService;

        public UserPermissionService()
        {
            _userService = new UserService();
        }
        
        public Task<bool> CheckPermission(UserPermissions perm, int userId)
        {
            var user = _userService.GetUser(userId);
            if (user == null)
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(user.HasPermission(perm));
        }
    }
}