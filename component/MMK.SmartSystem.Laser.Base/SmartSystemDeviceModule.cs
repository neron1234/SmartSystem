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
            string path = typeof(SmartSystemLaserModule).GetAssembly().GetDirectoryPathOrNull();
            string fileName = typeof(SmartSystemLaserModule).GetAssembly().GetName().Name;
            string fullName = Path.Combine(path, fileName + ".json");
            if (File.Exists(fullName))
            {
                var list = JsonConvert.DeserializeObject<List<SystemMenuModule>>(File.ReadAllText(fullName));
                Configuration.GetOrCreate(SmartSystemCommonConsts.ModulePageKey, () => new List<SystemMenuModule>()).AddRange(list);

            }
            var coreFullName = Path.Combine(path, fileName + ".core.json");
            if (File.Exists(coreFullName))
            {
                var list = JsonConvert.DeserializeObject<List<SignalrQueryParmModel>>(File.ReadAllText(coreFullName));
                Configuration.GetOrCreate(SmartSystemCommonConsts.ModuleQueryParmKey, () => new List<SignalrQueryParmModel>()).AddRange(list);
            }

        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
