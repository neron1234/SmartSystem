using Abp.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.LE.Host
{
    public class MMKSmartSystemLEHostModule: AbpModule
    {
        public override void PreInitialize()
        {

           
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            var listMenu = Configuration.GetOrCreate("WPF.Page", () => new List<string>());
            listMenu.ForEach(d =>
            {
                if (d.Contains(":"))
                {
                    var arr = d.Split(':');
                    SmartSystemLEConsts.SystemMeuns.Add(new ViewModel.MainMenuViewModel() { Title = arr[0], Page = arr[1] });
                }
            });

        }
    }
}
