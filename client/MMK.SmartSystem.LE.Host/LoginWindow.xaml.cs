using Abp.Dependency;
using Abp.Events.Bus;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using MMK.SmartSystem.Common;
using MMK.SmartSystem.Common.EventDatas;
using MMK.SmartSystem.Common.ViewModel;
using MMK.SmartSystem.LE.Host.ViewModel;
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
            this.DataContext = new MainTranslateViewModel();
        }

        private async void LoginWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(1000);
                this.Dispatcher.InvokeAsync(async () =>
                {
                    await AutoLogin();
                });
            });
        }

        private async Task AutoLogin()
        {
            await EventBus.Default.TriggerAsync(new UserLoginEventData()
            {
                UserName = SmartSystemLEConsts.DefaultUser,
                Pwd = SmartSystemLEConsts.DefaultPwd,
                Tagret = ErrorTagretEnum.Window,
                HashCode = this.GetHashCode(),
                SuccessAction = LoginSuccess
            });
            MainWindow mainWindow = new MainWindow(iocManager);
            mainWindow.Show();
            Messenger.Default.Register<MainSystemNoticeModel>(this, (model) => {
                if (model.HashCode == this.GetHashCode())
                {
                    model.SuccessAction?.Invoke();
                }
            });
            Close();
        }

        public void LoginSuccess()
        {
            EventBus.Default.Trigger(new UserInfoEventData() { UserId = (int)SmartSystemCommonConsts.AuthenticateModel.UserId, Tagret = ErrorTagretEnum.UserControl });
        }
    }
}
