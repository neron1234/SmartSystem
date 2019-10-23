using Abp.Dependency;
using Abp.Events.Bus;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common;
using MMK.SmartSystem.Common.EventDatas;
using MMK.SmartSystem.Common.Model;
using MMK.SmartSystem.Common.SignalrProxy;
using MMK.SmartSystem.Common.ViewModel;
using MMK.SmartSystem.LE.Host.CustomControl;
using MMK.SmartSystem.LE.Host.SystemControl.ViewModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MMK.SmartSystem.LE.Host
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, ISingletonDependency
    {
        IIocManager iocManager;
        SignalrRouteProxyClient signalrRouteProxyClient;
        public MainWindow(IIocManager iocManager)
        {
            this.iocManager = iocManager;
            AllowsTransparency = true;

            InitializeComponent();
            signalrRouteProxyClient = new SignalrRouteProxyClient();
            signalrRouteProxyClient.RouteErrorEvent += SignalrRouteProxyClient_RouteErrorEvent;
            signalrRouteProxyClient.GetHomeEvent += SignalrRouteProxyClient_GetHomeEvent;
            this.Loaded += MainWindow_Loaded;
            this.Closed += MainWindow_Closed;
        }

        private void SignalrRouteProxyClient_GetHomeEvent(string obj)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                ctnTest.Visibility = Visibility.Collapsed;
                viewBox.Visibility = Visibility.Visible;

            }));
         
        }

        private void SignalrRouteProxyClient_RouteErrorEvent(string obj)
        {

        }

        private async void MainWindow_Closed(object sender, EventArgs e)
        {
            await signalrRouteProxyClient.Close();
            Environment.Exit(0);
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {

            ctnTest.Visibility = Visibility.Visible;
            viewBox.Visibility = Visibility.Collapsed;
            mainHome.InitMessenger(iocManager);
            Messenger.Default.Register<MainSystemNoticeModel>(this, (model) =>
            {
                if (model.HashCode == this.GetHashCode())
                {
                    model.SuccessAction?.Invoke();
                }
            });
            Messenger.Default.Register<PageChangeModel>(this, (type) =>
            {
                Dispatcher.BeginInvoke(new Action(() => pageChange(type)));
            });
            await Task.Factory.StartNew(new Action(() => Dispatcher.BeginInvoke(new Action(loadWebApp))));
            await AutoLogin();
            await signalrRouteProxyClient.Start();
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

        }
        void pageChange(PageChangeModel changeModel)
        {
            if (changeModel.Page == PageEnum.WPFPage)
            {
                ctnTest.Visibility = Visibility.Collapsed;
                viewBox.Visibility = Visibility.Visible;
                mainHome.ChangeWPFPage(changeModel);

            }
            else if (changeModel.Page == PageEnum.WebPage)
            {
                ctnTest.Visibility = Visibility.Visible;
                viewBox.Visibility = Visibility.Collapsed;
                Task.Factory.StartNew(() => EventBus.Default.Trigger(new NavigateEventData()
                {
                    Url = changeModel.Url
                }));
            }
        }
        public void LoginSuccess()
        {
            EventBus.Default.Trigger(new UserInfoEventData() { UserId = (int)SmartSystemCommonConsts.AuthenticateModel.UserId, Tagret = ErrorTagretEnum.UserControl });
            App.CloseScreen();
            ///AllowsTransparency = false;
        }

        void loadWebApp()
        {
            string path = System.IO.Path.Combine(System.Environment.CurrentDirectory, "WebApp", "cncapp.exe");
            if (System.IO.File.Exists(path))
            {

                ctnTest.StartAndEmbedProcess(path);
            }
        }




    }
}
