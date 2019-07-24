using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using MMK.SmartSystem.Configuration;

namespace MMK.SmartSystem.Web.Host.Startup
{
    [DependsOn(
       typeof(SmartSystemWebCoreModule))]
    public class SmartSystemWebHostModule: AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public SmartSystemWebHostModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SmartSystemWebHostModule).GetAssembly());
        }
    }
}
