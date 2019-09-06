using Abp.Dependency;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.LE.Host.SystemControl.ViewModel;
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
        public MainWindow(IIocManager iocManager)
        {
            this.iocManager = iocManager;
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private  void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.systemMenuFrame.Content = new SystemControl.ModuleMenuControl();
        }

        public void Navigation(MainMenuViewModel model)
        {
            var page = iocManager.Resolve(model.PageType);
            mainFrame.Content = page;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = new AccountControl.LoginControl();
        }
    }
}
