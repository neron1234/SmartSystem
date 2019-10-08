using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.Reflection.Extensions;
using MMK.SmartSystem.Common.Model;
using MMK.SmartSystem.WebCommon;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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


        public static void LoadJsonConfig(Type type, IAbpStartupConfiguration abpStartup)
        {

            string path = type.GetAssembly().GetDirectoryPathOrNull();
            string fileName = type.GetAssembly().GetName().Name;
            string fullName = Path.Combine(path, fileName + ".json");
            if (File.Exists(fullName))
            {
                var list = JsonConvert.DeserializeObject<List<SystemMenuModule>>(File.ReadAllText(fullName));
                abpStartup.GetOrCreate(SmartSystemCommonConsts.ModulePageKey, () => new List<SystemMenuModule>()).AddRange(list);

            }
            string jsonConfig = typeof(MMKSmartSystemCommonModule).GetAssembly().GetDirectoryPathOrNull();
            List<string> jsonList = new List<string>();
            List<SignalrQueryParmModel> signalrQueryParmModels = new List<SignalrQueryParmModel>();
            GetAllFile(Path.Combine(jsonConfig, "Configs"), jsonList);

            foreach (var item in jsonList)
            {
                string data = File.ReadAllText(item);
                var arr = Path.GetDirectoryName(item).Split('\\');
                string pathModule = arr[arr.Length - 1];

                var pageNode = JsonConvert.DeserializeObject<SignlarPageParm>(data);
                var moduleParm = signalrQueryParmModels.FirstOrDefault(d => d.Module == pathModule);
                if (moduleParm == null)
                {
                    moduleParm = new SignalrQueryParmModel()
                    {
                        Module = pathModule,
                        Pages = new List<SignlarPageParm>()
                    };
                }
                moduleParm.Pages.Add(pageNode);
            }
            abpStartup.GetOrCreate(SmartSystemCommonConsts.ModuleQueryParmKey, () => new List<SignalrQueryParmModel>()).AddRange(signalrQueryParmModels);

        }

        private static void GetAllFile(string path, List<string> jsonList)
        {
            var paths = new DirectoryInfo(path).GetDirectories();
            jsonList.AddRange(Directory.GetFiles(path, "*.json"));
            foreach (var item in paths)
            {
                GetAllFile(item.FullName, jsonList);
            }
        }
    }
}
