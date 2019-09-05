using Abp.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Common
{
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
