using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MMK.SmartSystem.Common;

namespace System
{
    public static class GrantedPermission
    {
        public static bool IsGrantedPermission(this string permission)
        {
            var pageAuth = SmartSystemCommonConsts.UserConfiguration?.Auth?.GrantedPermissions ?? new Dictionary<string, string>();
           
            if (pageAuth.ContainsKey(permission))
            {
                return true;
            }
            return false;
        }
    }
}
