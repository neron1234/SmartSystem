using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Laser.Base.MachineProcess.ViewModel;
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

namespace MMK.SmartSystem.Laser.Base.MachineProcess
{
    /// <summary>
    /// PopupPage.xaml 的交互逻辑
    /// </summary>
    public partial class PopupWindow : Window
    {
        private PopupWindowViewModel popupWindowViewModel { get; set; }

        public PopupWindow(UserControl userControl, int width = 600, int height = 300,string title = "")
        {
            InitializeComponent();
            this.DataContext = popupWindowViewModel = new PopupWindowViewModel();
            popupWindowViewModel.PopupContent= userControl;
            Loaded += PopupWindowControl_Loaded;
            this.Width = PopupGrid.Width = width;
            this.Height = PopupGrid.Height = height + 40;
            popupWindowViewModel.Title = title;

            Messenger.Default.Register<string>(this,(s) => {
                MessageBox.Show(s);
                this.Close();
            });
        }

        private void PopupWindowControl_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= PopupWindowControl_Loaded;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
