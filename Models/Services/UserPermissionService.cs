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
        private IUser? user { get; set; }
        
        public UserPermissionService()
        {
            _userService = new UserService();
        }
        
        public bool RequireOne(string perm) => RequireOne(new[] { Enum.Parse<UserPermissions>(perm) });
        public bool RequireOne(string[] perms) => RequireOne(perms.Select(perm => Enum.Parse<UserPermissions>(perm)).ToArray());
        public bool RequireOne(UserPermissions perm) => RequireOne(new[] { perm });
        public bool RequireOne(UserPermissions[] perms)
        {
            if (user == null)
            {
                throw new InvalidOperationException("User not set");
            }

            if(user.HasPermission(UserPermissions.GODMODE)) return true;

            foreach(var perm in perms)
            {
                if (user.HasPermission(perm))
                {
                    return true;
                }
            }
            return false;
        }

        public bool RequireAll(string perm) => RequireAll(new[] { Enum.Parse<UserPermissions>(perm) });
        public bool RequireAll(string[] perms) => RequireAll(perms.Select(perm => Enum.Parse<UserPermissions>(perm)).ToArray());
        public bool RequireAll(UserPermissions perm) => RequireAll(new[] { perm });
        public bool RequireAll(UserPermissions[] perms)
        {
            if (user == null)
            {
                throw new InvalidOperationException("User not set");
            }

            if(user.HasPermission(UserPermissions.GODMODE)) return true;

            foreach(var perm in perms)
            {
                if (!user.HasPermission(perm))
                {
                    return false;
                }
            }
            return true;
        }

        public void SetUser(IUser user) {
            this.user = user;
        }
    }
}