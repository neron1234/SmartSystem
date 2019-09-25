using MMK.SmartSystem.Laser.Base.MachineMonitor.ViewModel;
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

namespace MMK.SmartSystem.Laser.Base.MachineMonitor
{
    /// <summary>
    /// ProgramPathControl.xaml 的交互逻辑
    /// </summary>
    public partial class ProgramPathControl : UserControl
    {
        public ProgramPathViewModel PathViewModel { get; private set; }
        public ProgramPathControl()
        {
            InitializeComponent();
            this.DataContext = PathViewModel = new ProgramPathViewModel();
        }
    }
}
