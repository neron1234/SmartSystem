using Abp.Dependency;
using GalaSoft.MvvmLight.Messaging;
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
        public List<MainMenuViewModel> mainMenuViews { set; get; }
        //public List<MainMenuViewModel> mainMenuPageViews { set; get; }
        public List<SystemMenuModuleViewModel> SysModuleViews { get;set; }
        IIocManager iocManager;
        public MainWindow(IIocManager iocManager)
        {
            this.iocManager = iocManager;
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            this.Unloaded += MainWindow_Unloaded;
        }

        private void MainWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Unregister<MainMenuViewModel>(this);
        }

        private  void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //mainMenuViews = SmartSystemLEConsts.SystemMeuns;
            SysModuleViews = SmartSystemLEConsts.SystemModules;
            this.DataContext = this;
            
            Messenger.Default.Register<SystemMenuModuleViewModel>(this, NavigationModule);
            //frame.Content = new Account.UserControls.LoginControl();
            //return;
            if (SysModuleViews.Count > 0)
            {
                NavigationModule(SysModuleViews[0]);
            }
        }

        private void Navigation(MainMenuViewModel model)
        {
            if (model.IsLoad)
            {
                var page = iocManager.Resolve(model.PageType);
                frame.Content = page;
            }
        }

        private void NavigationModule(SystemMenuModuleViewModel model)
        {
            mainMenuViews = model.mainMenuViews;
            Messenger.Default.Register<MainMenuViewModel>(this, Navigation);
            if (mainMenuViews.Count > 0)
            {
                Navigation(mainMenuViews[0]);
            }
        }
    }
}
