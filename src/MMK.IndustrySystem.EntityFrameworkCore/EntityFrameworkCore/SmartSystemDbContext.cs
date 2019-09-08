using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using MMK.SmartSystem.Authorization.Roles;
using MMK.SmartSystem.Authorization.Users;
using MMK.SmartSystem.MultiTenancy;
using MMK.CNC.Core.Managements;

namespace MMK.SmartSystem.EntityFrameworkCore
{
    public class SmartSystemDbContext : AbpZeroDbContext<Tenant, Role, User, SmartSystemDbContext>
    {
        /* Define a DbSet for each entity of the application */

        public DbSet<Department> Departments { get; set; }
        public SmartSystemDbContext(DbContextOptions<SmartSystemDbContext> options)
            : base(options)
        {
        }
    }
}
