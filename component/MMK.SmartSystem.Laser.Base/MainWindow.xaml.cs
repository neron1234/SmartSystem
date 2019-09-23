using MMK.SmartSystem.Laser.Base.MachineOperation;
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
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            switch (btn.Content)
            {
                case "自动巡边":
                    Frame.Content = new AutoFindSidePage();
                    break;
                case "割嘴复归":
                    Frame.Content = new CutterResetCheckPage();
                    break;
                case "割嘴清洁":
                    Frame.Content = new AutoCutterCleanPage();
                    break;
                case "割嘴对中":
                    Frame.Content = new CutCenterPage();
                    break;
                case "辅助气体":
                    Frame.Content = new AuxGasCheckPage();
                    break;
                case "手动寻边":
                    Frame.Content = new ManualFindSidePage();
                    break;
                case "激光状态":
                    Frame.Content = new LaserStatePage();
                    break;
                default:
                        Frame.Content = null;
                break;
            }
        }
    }
}
