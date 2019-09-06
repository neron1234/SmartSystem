using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;
using System.Collections.Generic;

namespace MMK.SmartSystem.Authorization
{
    public class SmartSystemAuthorizationProvider : AuthorizationProvider
    {

        private List<string> PermissionList = new List<string>()
        {
            "MachineOperation.CutCenterPage",
            "MachineOperation.AutoCutterCleanPage",
            "MachineOperation.CutterResetCheckPage",
            "MachineOperation.ManualFindSidePage",
            "MachineOperation.AuxGasCheckPage",
            "MachineOperation.AutoFindSidePage"
        };

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
            context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));
            context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            foreach (var item in PermissionList){
                context.CreatePermission(item, L(item), multiTenancySides: MultiTenancySides.Tenant);
                
            }
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, SmartSystemConsts.LocalizationSourceName);
        }
    }
}
