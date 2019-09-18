using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MMK.SmartSystem.Common;

namespace System
{
    public static class IsEditToPermission
    {
        public static bool ToPermission(this bool isEdit, string permission)
        {
            var pageAuth = SmartSystemCommonConsts.UserConfiguration?.Auth?.GrantedPermissions ?? new Dictionary<string, string>();
            if (pageAuth == null)
            {
                return false;
            }
            if (pageAuth.ContainsKey(permission))
            {
                return true;
            }
            return isEdit;
        }
    }
}
