using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common.Base;
using MMK.SmartSystem.Laser.Base.MachineProcessWork.ViewModel;
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

        protected override void PageSignlarLoaded()
        {
            Messenger.Default.Register<PageStatus>(this, (status) => {
                if (status == PageStatus.Max)
                {
                    this.OperateGrid.Visibility = Visibility.Collapsed;
                    this.ParametersGrid.Visibility = Visibility.Collapsed;
                    this.XLine.Visibility = Visibility.Collapsed;
                    this.YLine.Visibility = Visibility.Collapsed;
                    this.SimulationControl.Width = this.ActualWidth;
                    //this.SimulationGrid.SetValue(Grid.ColumnSpanProperty, 3);
                }
                else
                {
                    this.OperateGrid.Visibility = Visibility.Visible;
                    this.ParametersGrid.Visibility = Visibility.Visible;
                    this.XLine.Visibility = Visibility.Visible;
                    this.YLine.Visibility = Visibility.Visible;
                    this.SimulationControl.Width = 658;
                    //this.SimulationGrid.SetValue(Grid.ColumnSpanProperty, 1);
                    this.SimulationGrid.SetValue(Grid.ColumnProperty, 0);
                }
            });
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
