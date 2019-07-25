using Abp.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Device.Host
{
    public class SmartSystemDeviceModule:AbpModule
    {
        public override void PreInitialize()
        {
            List<string> meun = new List<string>()
            {
                $"设备监控:MMK.SmartSystem.Device.Host.Pages.DevicePage",
                $"设备管理:MMK.SmartSystem.Device.Host.Pages.ManagementPage",

            };
            Configuration.GetOrCreate("WPF.Page", () => new List<string>()).AddRange(meun);

        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
