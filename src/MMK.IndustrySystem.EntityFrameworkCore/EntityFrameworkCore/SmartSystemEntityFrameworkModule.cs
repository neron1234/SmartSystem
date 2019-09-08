using Abp.EntityFrameworkCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.EntityFrameworkCore;
using MMK.CNC.Core;
using MMK.SmartSystem.EntityFrameworkCore.Seed;

namespace MMK.SmartSystem.EntityFrameworkCore
{
    [DependsOn(
        typeof(SmartSystemCoreModule), 
        typeof(CNCCoreModule),
        typeof(AbpZeroCoreEntityFrameworkCoreModule))]
    public class SmartSystemEntityFrameworkCoreModule : AbpModule
    {
        /* Used it tests to skip dbcontext registration, in order to use in-memory database of EF Core */
        public bool SkipDbContextRegistration { get; set; }

        public bool SkipDbSeed { get; set; }

        public override void PreInitialize()
        {
            if (!SkipDbContextRegistration)
            {
                Configuration.Modules.AbpEfCore().AddDbContext<SmartSystemDbContext>(options =>
                {
                    if (options.ExistingConnection != null)
                    {
                        SmartSystemDbContextConfigurer.Configure(options.DbContextOptions, options.ExistingConnection);
                    }
                    else
                    {
                        SmartSystemDbContextConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
                    }
                });
            }
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SmartSystemEntityFrameworkCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            if (!SkipDbSeed)
            {
                SeedHelper.SeedHostDb(IocManager);
            }
        }
    }
}
