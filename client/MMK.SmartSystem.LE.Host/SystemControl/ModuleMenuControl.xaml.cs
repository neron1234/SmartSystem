using Abp.Dependency;
using GalaSoft.MvvmLight.Messaging;
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
using System.Collections.ObjectModel;
namespace MMK.SmartSystem.LE.Host.SystemControl
{
    /// <summary>
    /// ModuleMenuControl.xaml 的交互逻辑
    /// </summary>
    public partial class ModuleMenuControl : UserControl
    {
        public ObservableCollection<SystemMenuModuleViewModel> SysModuleViews { get; set; }
        public ModuleMenuControl()
        {
            InitializeComponent();
            SysModuleViews = SmartSystemLEConsts.SystemModules;
            this.DataContext = this;
        }
    }
}
