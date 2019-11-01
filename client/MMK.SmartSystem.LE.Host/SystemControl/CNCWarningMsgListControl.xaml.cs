using MMK.SmartSystem.LE.Host.CustomControl;
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
    /// CNCWarningMsgListControl.xaml 的交互逻辑
    /// </summary>
    public partial class CNCWarningMsgListControl : UserControl
    {
        public CNCWarningMsgViewModel waringMsgVM { get; set; }
        public CNCWarningMsgListControl()
        {
            InitializeComponent();
            this.DataContext = waringMsgVM = new CNCWarningMsgViewModel();
            this.Loaded += CNCWarningMsgListControl_Loaded;
        }

        private void CNCWarningMsgListControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.maskLayer.SetValue(MaskLayerBehavior.IsOpenProperty, true);
            Loaded -= CNCWarningMsgListControl_Loaded;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.maskLayer.SetValue(MaskLayerBehavior.IsOpenProperty, false);
        }
    }
}
