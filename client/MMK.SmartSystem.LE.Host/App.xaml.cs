using Abp;
using Abp.Castle.Logging.Log4Net;
using Abp.Events.Bus;
using Abp.PlugIns;
using Castle.Facilities.Logging;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using MMK.SmartSystem.Common.EventDatas;
using MMK.SmartSystem.Common.SerivceProxy;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MMK.SmartSystem.LE.Host
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private readonly AbpBootstrapper _bootstrapper;
        public App()
        {
            _bootstrapper = AbpBootstrapper.Create<MMKSmartSystemLEHostModule>();
            _bootstrapper.IocManager.IocContainer.AddFacility<LoggingFacility>(
                f => f.UseAbpLog4Net().WithConfig("log4net.config"));
        }


        protected override void OnStartup(StartupEventArgs e)
        {


            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");
            _bootstrapper.PlugInSources.AddFolder(path);
            _bootstrapper.Initialize();
            DispatcherHelper.Initialize();
            LoadPluginAssemblies();

            _bootstrapper.IocManager.Resolve<MainWindow>().Show();

        }

        private void LoadPluginAssemblies()
        {
            foreach (var plug in _bootstrapper.PlugInSources)
            {
                foreach (var item in plug.GetAssemblies())
                {
                    SmartSystemLEConsts.SystemModules.ToList().ForEach((s) => s.MainMenuViews.Where(d => !d.IsLoad).ToList().ForEach(d =>
                    {
                        if (!d.WebPage && !string.IsNullOrEmpty(d.Page))
                        {
                            var type = item.GetType(d.Page);
                            if (type != null)
                            {
                                d.IsLoad = true;
                                d.PageType = type;
                            }
                        }
                    }));
                }
            }
        }
        protected override void OnExit(ExitEventArgs e)
        {
            _bootstrapper.Dispose();
            base.OnExit(e);
        }
    }
}
