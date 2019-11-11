using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using MMK.CNC.Core;
using System;

namespace MMK.CNC.Application
{

    [DependsOn(typeof(CNCCoreModule), typeof(AbpAutoMapperModule))]
    public class CNCApplicationModule : AbpModule
    {
        public override void Initialize()
        {
            var thisAssembly = typeof(CNCApplicationModule).GetAssembly();
          

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
            Configuration.Caching.ConfigureAll(cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromHours(4);
            });
        }
    }
}
