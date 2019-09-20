using Abp.Dependency;
using Abp.Events.Bus;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common.EventDatas;
using MMK.SmartSystem.Common.Model;
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
        public MainWindowViewModel MainViewModel = new MainWindowViewModel();
        public MainWindow(IIocManager iocManager)
        {
            this.iocManager = iocManager;
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            this.DataContext = MainViewModel;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ctnTest.Visibility = Visibility.Visible;
            viewBox.Visibility = Visibility.Collapsed;
            Messenger.Default.Register<PageChangeModel>(this, (type) =>
            {
                Dispatcher.BeginInvoke(new Action(() => pageChange(type)));



            });

            Messenger.Default.Register<UserControl>(this, (control) =>
            {
                MainViewModel.PopupControl = control;
            });

            Messenger.Default.Register<Common.AuthenticateResultModel>(this, (userConfig) =>
            {
                MainViewModel.MainFrame = null;
            });
            Task.Factory.StartNew(new Action(() => Dispatcher.BeginInvoke(new Action(() =>
            {
                new LoginWindow().ShowDialog();
               // loadWebApp();
            }

           ))));

            Task.Factory.StartNew(new Action(() => Dispatcher.BeginInvoke(new Action(loadWebApp))));

            //loadWebApp();
        }

        void pageChange(PageChangeModel changeModel)
        {
            if (changeModel.Page == PageEnum.WPFPage)
            {
                var page = iocManager.Resolve(changeModel.FullType);
                MainViewModel.MainFrame = page;
                ctnTest.Visibility = Visibility.Collapsed;
                viewBox.Visibility = Visibility.Visible;
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

        void loadWebApp()
        {
            //Thread.Sleep(5000);
            string path = System.IO.Path.Combine(System.Environment.CurrentDirectory, "WebApp", "cncapp.exe");
            if (System.IO.File.Exists(path))
            {

                ctnTest.StartAndEmbedProcess(path);
            }
        }

        private void WebBtn_Click(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(new Action(() => Dispatcher.BeginInvoke(new Action(loadWebApp))));

        }
    }
}
