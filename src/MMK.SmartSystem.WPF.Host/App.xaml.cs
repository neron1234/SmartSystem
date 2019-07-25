using Abp;
using Abp.Castle.Logging.Log4Net;
using Castle.Facilities.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MMK.SmartSystem.WPF.Host
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private readonly AbpBootstrapper _bootstrapper;
        private MainWindow _mainWindow;
        public App()
        {
            _bootstrapper = AbpBootstrapper.Create<MMKSmarkSystemWPFHostModule>();
            _bootstrapper.IocManager.IocContainer.AddFacility<LoggingFacility>(
                f => f.UseAbpLog4Net().WithConfig("log4net.config"));
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _bootstrapper.Initialize();
            _mainWindow = _bootstrapper.IocManager.Resolve<MainWindow>();
            _bootstrapper.IocManager.Resolve(Type.GetType(""));
            _mainWindow.Show();
            // base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _bootstrapper.IocManager.Release(_mainWindow);
            _bootstrapper.Dispose();
            // base.OnExit(e);
        }
    }
}
