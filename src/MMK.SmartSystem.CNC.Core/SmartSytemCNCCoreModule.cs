using Abp.Modules;
using Abp.Reflection.Extensions;
using MMK.SmartSystem.WebCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.CNC.Core
{
    [DependsOn(typeof(SmartSystemWebCommonModule))]
    public class SmartSytemCNCCoreModule : AbpModule
    {
        public override void Initialize()
        {
            var thisAssembly = typeof(SmartSytemCNCCoreModule).GetAssembly();
            IocManager.RegisterAssemblyByConvention(thisAssembly);

        }
    }
}
