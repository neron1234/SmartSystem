using MMK.SmartSystem.Common.SignalrProxy;
using MMK.SmartSystem.Laser.Base.MachineMonitor.ViewModel;
using MMK.SmartSystem.WebCommon.DeviceModel;
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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MMK.SmartSystem.Laser.Base.MachineMonitor
{
    /// <summary>
    /// CoordinateControl.xaml 的交互逻辑
    /// </summary>
    public partial class CoordinateControl : UserControl
    {


        public CoordinatePositionViewModel PositionViewModel { get; private set; }

        public CoordinateControl()
        {
            InitializeComponent();
            PositionViewModel = new CoordinatePositionViewModel();
            this.DataContext = this;
        }
    }
}
