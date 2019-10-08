using Abp.Modules;
using Abp.Reflection.Extensions;
using MMK.SmartSystem.WebCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Common
{
    [DependsOn(typeof(SmartSystemWebCommonModule))]
    public class MMKSmartSystemCommonModule : AbpModule
    {
        public override void PreInitialize()
        {
            
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

    }
}
