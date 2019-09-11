using Abp.Dependency;
using Abp.Events.Bus;
using GalaSoft.MvvmLight.Threading;
using MMK.SmartSystem.Common.EventDatas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MMK.SmartSystem.LE.Host
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window, ISingletonDependency
    {
        IIocManager iocManager;
        public LoginWindow(IIocManager iocManager)
        {
            this.iocManager = iocManager;
            InitializeComponent();
            Loaded += LoginWindow_Loaded;
        }

        private async void LoginWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //await DispatcherHelper.RunAsync(async () =>
            //{
            //    await AutoLogin();
            //});
            await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(2000);
                this.Dispatcher.InvokeAsync(async () =>
                {
                    await AutoLogin();
                });
            });
        }

        private async Task AutoLogin()
        {
            await EventBus.Default.TriggerAsync(new UserConfigEventData()
            {
                UserName = SmartSystemLEConsts.DefaultUser,
                Pwd = SmartSystemLEConsts.DefaultPwd,
                Culture = SmartSystemLEConsts.Culture,
                IsChangeUser = true
            });
            MainWindow mainWindow = new MainWindow(iocManager);
            mainWindow.Show();
            Close();
        }
    }
}
