using Abp.Dependency;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common.Model;
using MMK.SmartSystem.LE.Host.SystemControl.ViewModel;
using MMK.SmartSystem.LE.Host.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            Messenger.Default.Register<Type>(this, (type) =>
            {
                var page = iocManager.Resolve(type);
                MainViewModel.MainFrame = page;
            });

            Messenger.Default.Register<UserControl>(this, (control) =>
            {
                MainViewModel.PopupControl = control;
            });

            Messenger.Default.Register<Common.AuthenticateResultModel>(this, (userConfig) =>
            {
                MainViewModel.MainFrame = null;
            });
            loadWebApp();
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
