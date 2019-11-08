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
using System.Windows.Forms.Integration;
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
        private const int LoadMaxTime = 10;
        int loadTask = 0;
        IIocManager iocManager;
        SignalrRouteProxyClient signalrRouteProxyClient;
        LoadWindow loadWindow;

        public MainWindow(IIocManager iocManager)
        {
            this.iocManager = iocManager;

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
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {

            ctnTest.Visibility = Visibility.Hidden;
            viewBox.Visibility = Visibility.Collapsed;
            mainHome.InitMessenger(iocManager);
            InitMessager();

            Task.Factory.StartNew(async () => await signalrRouteProxyClient.Start());
            Task.Factory.StartNew(async () => await AutoLogin());
            Task.Factory.StartNew(new Action(() => loadWebApp()));
        }

        private void InitMessager()
        {
            Messenger.Default.Register<PageChangeModel>(this, (type) =>
            {
                Dispatcher.BeginInvoke(new Action(() => pageChange(type)));
            });
            Messenger.Default.Register<WindowStatus>(this, (ws) =>
            {
                switch (ws)
                {
                    case WindowStatus.Max:
                        if (this.WindowState == WindowState.Maximized)
                        {
                            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                            this.WindowState = WindowState.Normal;
                            Messenger.Default.Send((PathGeometry)FindResource("maxWindowIcon"));
                        }
                        else
                        {
                            this.WindowState = WindowState.Maximized;
                            Messenger.Default.Send((PathGeometry)FindResource("normalWindowIcon"));
                        }
                        mainHome.Width = this.Width;
                        mainHome.Height = this.Height;
                        viewBox.Width = this.Width;
                        viewBox.Height = this.Height;
                        break;
                    case WindowStatus.Min:
                        this.WindowState = WindowState.Minimized;
                        break;
                    default:
                        break;
                }
            });

            // 获取全局系统通知
            Messenger.Default.Register<MainSystemNoticeModel>(this, (model) =>
            {
                LoadNotice(model);
            });

            Messenger.Default.Register<WaringMsgPopup>(this, (pv) =>
            {
                new PopupWindow().ShowDialog();
            });
        }
        private void LoadNotice(MainSystemNoticeModel model)
        {
            Action loadCloseAction = () =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (loadWindow != null)
                    {
                        loadWindow.Close();
                        loadWindow = null;
                    }


                }));

            };

            if (model.EventType == EventEnum.StartLoad)
            {
                if (loadWindow == null)
                {
                    loadWindow = new LoadWindow();
                    Task.Factory.StartNew(new Action(async () =>
                    {
                        await Task.Delay(LoadMaxTime * 1000);
                        loadCloseAction();

                    }));
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        loadWindow.ShowDialog();

                    }));
                }
                return;
            }
            if (model.EventType == EventEnum.EndLoad)
            {
                Thread.Sleep(1000);
                loadCloseAction();
            }
        }

        private void pageChange(PageChangeModel changeModel)
        {
            if (changeModel.Page == PageEnum.WPFPage)
            {
                ctnTest.Visibility = Visibility.Hidden;
                viewBox.Visibility = Visibility.Visible;
                mainHome.ChangeWPFPage(changeModel);

            }
            else if (changeModel.Page == PageEnum.WebPage)
            {
                ctnTest.Visibility = Visibility.Visible;
                viewBox.Visibility = Visibility.Hidden;
                Task.Factory.StartNew(() => EventBus.Default.Trigger(new NavigateEventData()
                {
                    Url = changeModel.Url,
                    NavigateType = NavigateEnum.Url
                }));
            }
            else if (changeModel.Page == PageEnum.WebComponet)
            {
                Task.Factory.StartNew(() => EventBus.Default.Trigger(new NavigateEventData()
                {
                    NavigateType = NavigateEnum.Component,
                    ComponentDto = changeModel.ComponentDto
                }));

            }
        }


        private void loadWebApp()
        {


            string path = System.IO.Path.Combine(System.Environment.CurrentDirectory, "WebApp", "cncapp.exe");
            if (System.IO.File.Exists(path))
            {
                ctnTest.StartAndEmbedProcess(path, Dispatcher);

                ShowHomePanel();

            }

        }

        private void ShowHomePanel()
        {
            loadTask++;
            if (loadTask > 1)
            {
                Thread.Sleep(500);
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    ctnTest.Visibility = Visibility.Visible;
                    loadImage.Visibility = Visibility.Collapsed;

                }));
            }

        }


        private async Task AutoLogin()
        {
            await EventBus.Default.TriggerAsync(new UserLoginEventData()
            {
                UserName = SmartSystemLEConsts.DefaultUser,
                Pwd = SmartSystemLEConsts.DefaultPwd,
                Tagret = ErrorTagretEnum.Window,
                HashCode = this.GetHashCode(),
                SuccessAction = (s) =>
                {
                    ShowHomePanel();
                    EventBus.Default.Trigger(new UserInfoEventData() { UserId = (int)SmartSystemCommonConsts.AuthenticateModel.UserId, Tagret = ErrorTagretEnum.UserControl });

                }
            });

        }

    }
}
