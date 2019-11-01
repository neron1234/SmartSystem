using MMK.SmartSystem.Common.Base;
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

namespace MMK.SmartSystem.Laser.Base.MachineProcessWork
{
    /// <summary>
    /// MachineProcessWorkPage.xaml 的交互逻辑
    /// </summary>
    public partial class MachineProcessWorkPage : SignalrPage
    {
        public MachineProcessWorkPage()
        {
            InitializeComponent();
        }

        public override List<object> GetResultViewModelMap()
        {
            return default;
        }
        public override void CncOnError(string message)
        {

        }
    }
}
