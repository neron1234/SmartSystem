using Abp.Authorization;
using MMK.SmartSystem.Authorization.Roles;
using MMK.SmartSystem.Authorization.Users;

namespace MMK.SmartSystem.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
