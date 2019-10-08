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
using MMK.SmartSystem.Laser.Base.MachineMonitor.ViewModel;

namespace MMK.SmartSystem.Laser.Base.MachineMonitor
{
    /// <summary>
    /// WarningListPage.xaml 的交互逻辑
    /// </summary>
    public partial class WarningListControl : UserControl
    {
        public WarningListViewModel wnListViewModel { get; private set; }
        public WarningListControl()
        {
            InitializeComponent();
            wnListViewModel = new WarningListViewModel();
            this.SysItemControl.ItemsSource = wnListViewModel.WarningList;
        }
    }
}
