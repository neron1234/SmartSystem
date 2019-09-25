using MMK.SmartSystem.Common.SignalrProxy;
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
    /// CoordinateControl.xaml 的交互逻辑
    /// </summary>
    public partial class CoordinateControl : UserControl
    {
        SignalrProxyClient signalrProxyClient;
        public CoordinateControl()
        {
            InitializeComponent();
            this.DataContext = new CoordinateControlViewModel();
            signalrProxyClient = new SignalrProxyClient();
            signalrProxyClient.CncErrorEvent += SignalrProxyClient_CncErrorEvent;
            signalrProxyClient.HubRefreshModelEvent += SignalrProxyClient_HubRefreshModelEvent;
            this.Loaded += CoordinateControl_Loaded;
            this.Unloaded += CoordinateControl_Unloaded;
        }

        private void SignalrProxyClient_HubRefreshModelEvent(WebCommon.HubModel.HubResultModel obj)
        {
        }

        private void SignalrProxyClient_CncErrorEvent(string obj)
        {
        }

        private async void CoordinateControl_Unloaded(object sender, RoutedEventArgs e)
        {
            await signalrProxyClient.Close();
        }

        private async void CoordinateControl_Loaded(object sender, RoutedEventArgs e)
        {
            await signalrProxyClient.Start();
            signalrProxyClient.SendCncData(null);
        }
    }
}
