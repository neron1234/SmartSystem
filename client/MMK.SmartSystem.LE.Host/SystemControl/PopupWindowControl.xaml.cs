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
        private PopupWindowViewModel popupWindowViewModel { get; set; }
        
        public PopupWindowControl(UserControl userControl,int width = 600,int height = 300)
        {
            InitializeComponent();
            this.DataContext = popupWindowViewModel =new PopupWindowViewModel();
            popupWindowViewModel.PopupContent = userControl;
            Loaded += PopupWindowControl_Loaded;
            this.Width = PopupGrid.Width = width;
            this.Height = PopupGrid.Height = height;
            
        }

        private void PopupWindowControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.maskLayer.SetValue(MaskLayerBehavior.IsOpenProperty, true);
            Loaded -= PopupWindowControl_Loaded;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.maskLayer.SetValue(MaskLayerBehavior.IsOpenProperty, false);
            Messenger.Default.Send("");
        }
    }
}
