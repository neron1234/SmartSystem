using MMK.SmartSystem.Laser.Base.ViewModel;
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
using System.Windows.Shapes;

namespace MMK.SmartSystem.Laser.Base
{
    /// <summary>
    /// InputWindow.xaml 的交互逻辑
    /// </summary>
    public partial class InputWindow : Window
    {
        InputWindowViewModel inputWindowView;

        public event Action<string> InputWindowFinishEvent;
        public InputWindow(string value, int minValue, int maxValue, string title)
        {
            InitializeComponent();
            inputWindowView = new InputWindowViewModel() { Value = value, MaxValue = maxValue, MinValue = minValue, Title = title };

            DataContext = inputWindowView;
            buttomItem.ItemsSource = inputWindowView.InputButtonItems;
        }



        private void btn_ok_Click(object sender, RoutedEventArgs e)
        {
            InputWindowFinishEvent?.Invoke(inputWindowView.Value);
            this.Close();
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
