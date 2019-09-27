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
using System.Windows.Threading;

namespace MMK.SmartSystem.LE.Host.SystemControl
{
    /// <summary>
    /// BottomWarningLogControl.xaml 的交互逻辑
    /// </summary>
    public partial class BottomWarningLogControl : UserControl
    {
        public BottomWarningLogViewModel LogViewModel { get; set; }
        private DispatcherTimer timer = new DispatcherTimer();
        public BottomWarningLogControl()
        {
            InitializeComponent();
            this.DataContext = LogViewModel = new BottomWarningLogViewModel();

            timer.Interval = TimeSpan.FromMilliseconds(3000);
            timer.Tick += Timer_Tick;

            Messenger.Default.Register<BottomWarningLogViewModel>(this,(vm) => {
                LogViewModel.WarningLogStr = vm.WarningLogStr;
                timer.Start();
            });
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            LogViewModel.WarningLogStr = "";
            timer.Stop();
        }
    }
}
