using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using MMK.SmartSystem.Authorization;

namespace MMK.SmartSystem
{
    [DependsOn(
        typeof(SmartSystemCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class SmartSystemApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<SmartSystemAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(SmartSystemApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
