using Abp.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.CNC.Host
{
    public class SmartSystemCNCModule : AbpModule
    {
        public override void PreInitialize()
        {
            List<string> meun = new List<string>()
            {
                $"首页:MMK.SmartSystem.CNC.Host.Pages.HelloPage",
                $"配置界面:MMK.SmartSystem.CNC.Host.Pages.ConfigPage",

            };
            Configuration.GetOrCreate("WPF.Page", () => new List<string>()).AddRange(meun);

        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
