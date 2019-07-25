using Abp.Modules;
using Abp.Reflection.Extensions;
using Microsoft.Extensions.Configuration;
using MMK.CNC.Application;
using MMK.SmartSystem.Configuration;
using MMK.SmartSystem.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.WPF.Host
{
    [DependsOn(typeof(CNCApplicationModule), typeof(SmartSystemEntityFrameworkModule))]
    public class MMKSmarkSystemWPFHostModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public MMKSmarkSystemWPFHostModule()
        {

            _appConfiguration = AppConfigurations.Get(
                typeof(MMKSmarkSystemWPFHostModule).GetAssembly().GetDirectoryPathOrNull()
            );
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                SmartSystemConsts.ConnectionStringName
            );
            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;

        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            var listMenu = Configuration.GetOrCreate("WPF.Page", () => new List<string>());
            listMenu.ForEach(d =>
            {
                if (d.Contains(":"))
                {
                    var arr = d.Split(':');
                    SmartSystemWPFConsts.SystemMeuns.Add(new ViewModel.MainMenuViewModel() { Title = arr[0], Page = arr[1] });
                }
            });

        }
    }
}
