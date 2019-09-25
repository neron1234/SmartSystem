using Abp;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Castle.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Authorization
{

    public abstract class CustomPermissionChecker<TRole, TUser> : IPermissionChecker, ITransientDependency, IIocManagerAccessor
    where TRole : AbpRole<TUser>, new()
    where TUser : AbpUser<TUser>
    {
        private readonly AbpUserManager<TRole, TUser> _userManager;

        public IIocManager IocManager { get; set; }

        public ILogger Logger { get; set; }

        public IAbpSession AbpSession { get; set; }

        public ICurrentUnitOfWorkProvider CurrentUnitOfWorkProvider { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected CustomPermissionChecker(AbpUserManager<TRole, TUser> userManager)
        {
            _userManager = userManager;

            Logger = NullLogger.Instance;
            AbpSession = NullAbpSession.Instance;
        }

        public virtual async Task<bool> IsGrantedAsync(string permissionName)
        {
            return AbpSession.UserId.HasValue && await _userManager.IsGrantedAsync(AbpSession.UserId.Value, permissionName);
        }

        public virtual bool IsGranted(string permissionName)
        {
            return AbpSession.UserId.HasValue && _userManager.IsGrantedAsync(AbpSession.UserId.Value, permissionName).Result;
        }

        public virtual async Task<bool> IsGrantedAsync(long userId, string permissionName)
        {
            return await _userManager.IsGrantedAsync(userId, permissionName);
        }

        public virtual bool IsGranted(long userId, string permissionName)
        {
            return _userManager.IsGrantedAsync(userId, permissionName).Result;
        }

        [UnitOfWork]
        public virtual async Task<bool> IsGrantedAsync(UserIdentifier user, string permissionName)
        {
            if (CurrentUnitOfWorkProvider == null || CurrentUnitOfWorkProvider.Current == null)
            {
                return await IsGrantedAsync(user.UserId, permissionName);
            }

            using (CurrentUnitOfWorkProvider.Current.SetTenantId(user.TenantId))
            {
                return await _userManager.IsGrantedAsync(user.UserId, permissionName);
            }
        }

        [UnitOfWork]
        public virtual bool IsGranted(UserIdentifier user, string permissionName)
        {
            if (CurrentUnitOfWorkProvider == null || CurrentUnitOfWorkProvider.Current == null)
            {
                return IsGranted(user.UserId, permissionName);
            }

            using (CurrentUnitOfWorkProvider.Current.SetTenantId(user.TenantId))
            {
                return _userManager.IsGrantedAsync(user.UserId, permissionName).Result;
            }
        }

       
    }

}
