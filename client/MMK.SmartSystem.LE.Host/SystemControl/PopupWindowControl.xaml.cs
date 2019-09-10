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
    /// PopupWindowControl.xaml 的交互逻辑
    /// </summary>
    public partial class PopupWindowControl : UserControl
    {
        public PopupWindowControl()
        {
            InitializeComponent();
            Loaded += PopupWindowControl_Loaded;
        }

        private void PopupWindowControl_Loaded(object sender, RoutedEventArgs e)
        {
            //this.maskLayer.SetValue(MaskLayerBehavior.IsOpenProperty, true);
            this.DataContext = new PopupWindowViewModel();
        }

        public void Close()
        {
            this.maskLayer.SetValue(MaskLayerBehavior.IsOpenProperty, false);
        }

        public void Open()
        {
            btnOpenMaskLayer.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnOpenMaskLayer_Click(object sender, RoutedEventArgs e)
        {
            this.maskLayer.SetValue(MaskLayerBehavior.IsOpenProperty, true);
        }
    }
}
