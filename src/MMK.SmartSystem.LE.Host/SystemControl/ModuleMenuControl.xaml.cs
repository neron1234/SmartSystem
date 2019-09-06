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

namespace MMK.SmartSystem.LE.Host.SystemControl
{
    /// <summary>
    /// ModuleMenuControl.xaml 的交互逻辑
    /// </summary>
    public partial class ModuleMenuControl : UserControl
    {
        public List<SystemMenuModuleViewModel> SysModuleViews { get; set; }
        public ModuleMenuControl()
        {
            InitializeComponent();
            Loaded += ModuleMenuControl_Loaded;
            Unloaded += ModuleMenuControl_Unloaded;
        }

        private void ModuleMenuControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Unregister<MainMenuViewModel>(this, Navigation);
        }

        private void ModuleMenuControl_Loaded(object sender, RoutedEventArgs e)
        {
            SysModuleViews = SmartSystemLEConsts.SystemModules;
            this.DataContext = this;
            Messenger.Default.Register<MainMenuViewModel>(this, Navigation);
        }

        private void Navigation(MainMenuViewModel model)
        {
            if (model.IsLoad)
            {
                //var page = IocManager.Resolve(model.PageType);
                var mainWindow = (MainWindow)Window.GetWindow(this);
                mainWindow.Navigation(model);
            }
        }
    }
}
