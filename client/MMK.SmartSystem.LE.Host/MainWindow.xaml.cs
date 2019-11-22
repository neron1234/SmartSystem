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
using MMK.SmartSystem.LE.Host.MainHome;
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
        LoadWindow loadWindow;

        MainPage mainPage;
        public MainWindow(IocManager iocManager)
        {
            InitializeComponent();
            mainPage = new MainPage(iocManager);
            Loaded += MainWindow_Loaded;
            mainPage.WebOrHostDisplayEvent += MainPage_WebOrHostDisplayEvent;
            mainFrame.Content = mainPage;
        }

        private void MainPage_WebOrHostDisplayEvent(Visibility arg1, Visibility arg2)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                ctnTest.Visibility = arg1;
                viewBox.Visibility = arg2;
            }));
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //ctnTest.Visibility = Visibility.Hidden;
            //viewBox.Visibility = Visibility.Collapsed;

            ctnTest.Visibility = Visibility.Hidden;
            viewBox.Visibility = Visibility.Collapsed;
            loadImage.Visibility = Visibility.Visible;

            Messenger.Default.Register<MainSystemNoticeModel>(this, (model) =>
            {
                LoadNotice(model);
            });

            Task.Factory.StartNew(async () => await AutoLogin());

            Task.Factory.StartNew(new Action(() => loadWebApp()));
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
