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
        //private MainWindow _mainWindow;
        private LoginWindow _loginWindow;
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
            //_mainWindow = _bootstrapper.IocManager.Resolve<MainWindow>();
            //_mainWindow.Show();
            _loginWindow = _bootstrapper.IocManager.Resolve<LoginWindow>();
            _loginWindow.Show();

            //Task.Factory.StartNew(() => AutoLogin());
            LoadPluginAssemblies();
        }

        private void AutoLogin()
        {
            EventBus.Default.Trigger(new UserConfigEventData()
            {
                UserName = SmartSystemLEConsts.DefaultUser,
                Pwd = SmartSystemLEConsts.DefaultPwd,
                Culture = SmartSystemLEConsts.Culture,
                IsChangeUser = true
            });
        }

        private void LoadPluginAssemblies()
        {
            foreach (var plug in _bootstrapper.PlugInSources)
            {
                foreach (var item in plug.GetAssemblies())
                {
                    SmartSystemLEConsts.SystemModules.ToList().ForEach((s) => s.MainMenuViews.Where(d => !d.IsLoad).ToList().ForEach(d =>
                    {
                        var type = item.GetType(d.Page);
                        if (type != null)
                        {
                            d.IsLoad = true;
                            d.PageType = type;
                        }

                    }));
                }
            }
        }
        protected override void OnExit(ExitEventArgs e)
        {
            _bootstrapper.IocManager.Release(_loginWindow);
            _bootstrapper.Dispose();
            // base.OnExit(e);
        }
    }
}
