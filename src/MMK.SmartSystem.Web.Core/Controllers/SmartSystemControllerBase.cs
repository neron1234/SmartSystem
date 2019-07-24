using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace MMK.SmartSystem.Controllers
{
    public abstract class SmartSystemControllerBase: AbpController
    {
        protected SmartSystemControllerBase()
        {
            LocalizationSourceName = SmartSystemConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
