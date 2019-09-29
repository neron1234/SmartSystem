using Abp.Modules;
using Abp.Reflection.Extensions;
using MMK.SmartSystem.CNC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.CNC.Host
{
    [DependsOn(typeof(SmartSytemCNCCoreModule))]
    public class SmartSystemCNCHostModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SmartSystemCNCHostModule).GetAssembly());
            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;

        }
    }
}
