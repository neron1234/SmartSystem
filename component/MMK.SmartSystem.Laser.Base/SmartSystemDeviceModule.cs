using Abp.Modules;
using Abp.Reflection.Extensions;
using MMK.SmartSystem.Common;
using MMK.SmartSystem.Common.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace MMK.SmartSystem.Laser.Base
{
    public class SmartSystemLaserModule : AbpModule
    {
        public override void PreInitialize()
        {

            MMKSmartSystemCommonModule.LoadJsonConfig(typeof(SmartSystemLaserModule), Configuration);
          
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
