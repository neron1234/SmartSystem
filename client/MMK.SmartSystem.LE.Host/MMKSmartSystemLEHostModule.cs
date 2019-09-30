using Abp.Events.Bus;
using Abp.Modules;
using MMK.SmartSystem.Common;
using MMK.SmartSystem.Common.Model;
using MMK.SmartSystem.LE.Host.SystemControl.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;



        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            var listModule = Configuration.GetOrCreate(SmartSystemCommonConsts.ModulePageKey, () => new List<SystemMenuModule>());

            listModule.ForEach(d =>
            {
                var pages = new List<MainMenuViewModel>();
                d.Pages.OrderBy(f => f.Sort).ToList().ForEach(g =>
                {
                    pages.Add(new MainMenuViewModel()
                    {
                        Id = g.Id,
                        Title = g.Title,
                        Page = g.FullName,
                        Auth = g.IsAuth,
                        Permission = g.Permission,
                        PageKey = g.Title,
                        Url = g.Url,
                        WebPage = g.WebPage,
                        BackColor = d.BackColor,
                        Sort = g.Sort
                    });
                });
                SmartSystemLEConsts.SystemModules.Add(new SystemMenuModuleViewModel()
                {
                    BackColor = d.BackColor,
                    Icon = d.Icon,
                    ModuleName = d.ModuleName,
                    MainMenuViews = new ObservableCollection<MainMenuViewModel>(pages),
                    ModuleKey = d.ModuleName,
                    Sort = d.Sort
                });
            });
            var listSignalrParm = Configuration.GetOrCreate(SmartSystemCommonConsts.ModuleQueryParmKey, () => new List<SignalrQueryParmModel>());
            SmartSystemCommonConsts.SignalrQueryParmModels = listSignalrParm;
        }
    }
}
