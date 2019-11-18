using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common.Base;
using MMK.SmartSystem.Laser.Base.MachineProcessWork.ViewModel;
using netDxf.Entities;
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
            Messenger.Default.Register<PageStatus>(this, (status) => {
                if (status == PageStatus.Max)
                {
                    ProcessPanel.Visibility = Visibility.Collapsed;
                    this.SimulationControl.Width = this.ActualWidth;
                }
                else
                {
                    ProcessPanel.Visibility = Visibility.Visible;
                    this.SimulationControl.Width = 916;
                }
            });
        }

        protected override void PageSignlarLoaded()
        {
            
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
