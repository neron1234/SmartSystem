using Abp.Events.Bus;
using Abp.Modules;
using MMK.SmartSystem.Common;
using MMK.SmartSystem.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.LE.Host
{
    [DependsOn(typeof(MMKSmartSystemCommonModule))]
    public class MMKSmartSystemLEHostModule : AbpModule
    {
        public override void PreInitialize()
        {

           

        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            var listModule = Configuration.GetOrCreate(SmartSystemCommonConsts.ModulePageKey, () => new List<SystemMenuModule>());

            listModule.ForEach(d =>
            {
                var pages = new System.Collections.ObjectModel.ObservableCollection<ViewModel.MainMenuViewModel>();
                d.Pages.ForEach(g =>{
                    pages.Add(new ViewModel.MainMenuViewModel() { Title = g.Title.Translate(), Page = g.FullName });
                });
                SmartSystemLEConsts.SystemModules.Add(new ViewModel.SystemMenuModuleViewModel() { Icon = d.Icon, ModuleName = d.ModuleName.Translate(), MainMenuViews = pages });
            });

        }
    }
}
