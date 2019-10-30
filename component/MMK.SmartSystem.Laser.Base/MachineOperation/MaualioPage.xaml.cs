using Abp.Dependency;
using MMK.SmartSystem.Common.Base;
using MMK.SmartSystem.Laser.Base.MachineOperation.ViewModel;
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

namespace MMK.SmartSystem.Laser.Base.MachineOperation
{
    /// <summary>
    /// MaualioPage.xaml 的交互逻辑
    /// </summary>
    public partial class MaualioPage : SignalrPage
    {
        public ManualioViewModel mioVm { get; set; }
        public MaualioPage()
        {
            InitializeComponent();
            this.DataContext = mioVm = new ManualioViewModel();
            manualControl.SetHeaderActive(this);
        }

        public override void CncOnError(string message)
        {

        }

        public override List<object> GetResultViewModelMap()
        {
            return default;
        }
    }
}
