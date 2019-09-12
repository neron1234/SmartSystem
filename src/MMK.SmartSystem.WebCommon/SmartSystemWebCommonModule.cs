using Abp.Modules;
using Abp.Reflection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMK.SmartSystem.WebCommon
{
    public class SmartSystemWebCommonModule: AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SmartSystemWebCommonModule).GetAssembly());
        }
    }
}
