using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MMK.SmartSystem.Authorization;
using MMK.SmartSystem.Authorization.Roles;
using MMK.SmartSystem.Authorization.Users;
using MMK.SmartSystem.Editions;
using MMK.SmartSystem.MultiTenancy;

namespace MMK.SmartSystem.Identity
{
    public static class IdentityRegistrar
    {
        public static IdentityBuilder Register(IServiceCollection services)
        {
            services.AddLogging();

            return services.AddAbpIdentity<Tenant, User, Role>()
                .AddAbpTenantManager<TenantManager>()
                .AddAbpUserManager<UserManager>()
                .AddAbpRoleManager<RoleManager>()
                .AddAbpEditionManager<EditionManager>()
                .AddAbpUserStore<UserStore>()
                .AddAbpRoleStore<RoleStore>()
                .AddAbpLogInManager<LogInManager>()
                .AddAbpSignInManager<SignInManager>()
                .AddAbpSecurityStampValidator<SecurityStampValidator>()
                .AddAbpUserClaimsPrincipalFactory<UserClaimsPrincipalFactory>()
                .AddPermissionChecker<CustomPermissionCheker>()
                .AddDefaultTokenProviders();
        }
    }
}
