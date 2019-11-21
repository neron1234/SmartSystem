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

namespace MMK.SmartSystem.Laser.Base
{
    /// <summary>
    /// ConfirmControl.xaml 的交互逻辑
    /// </summary>
    public partial class ConfirmControl : UserControl
    {


        public event Action ConfirmOkEvent;
        public event Action ConfirmCancelEvent;
        public ConfirmControl(string message)
        {
            InitializeComponent();
            textBlock_message.Text = message;
        }

        private void btn_ok_Click(object sender, RoutedEventArgs e)
        {
            ConfirmOkEvent?.Invoke();
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            ConfirmCancelEvent?.Invoke();
        }
    }
}
