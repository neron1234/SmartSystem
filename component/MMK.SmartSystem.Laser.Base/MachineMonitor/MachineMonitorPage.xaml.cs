using Abp.Dependency;
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
using MMK.SmartSystem.Laser.Base.MachineMonitor.ViewModel;
using Newtonsoft.Json.Linq;
using MMK.SmartSystem.WebCommon.DeviceModel;
using MMK.SmartSystem.Common.SignalrProxy;
using MMK.SmartSystem.Common.Base;
using MMK.SmartSystem.Common.Model;

namespace MMK.SmartSystem.Laser.Base.MachineMonitor
{

    /// <summary>
    /// MachineMonitorPage.xaml 的交互逻辑
    /// </summary>
    public partial class MachineMonitorPage : SignalrPage
    {

        public MachineMonitorPage()
        {
            InitializeComponent();

        }
        public override string GetModule => "MachineMonitor";

        private async void Btn_Click(object sender, RoutedEventArgs e)
        {
            await SendProxyAction<BaseCNCResultModel<ReadProgramListItemResultModel>>(SinglarCNCHubConsts.ReadProgramListAction, "//CNC_MEM/USER/PATH1/");
        }

        public override void PageSignlarLoaded()
        {

        }



        public override List<object> GetResultViewModelMap()
        {
            List<object> list = new List<object>()
            {
                new SingalrResultMapModel<ReadPositionResultItemModel>()
                {
                    ViewModels = coordinateControl.PositionViewModel,
                    MapType = SignalrMapModelEnum.AutoPropMap,
                    AutoPropMapAction = (node, propName) => node.FirstOrDefault(d => d.Id == propName)?.Value
                },
               new SingalrResultMapModel<ReadProgramStrResultModel>()
                 {
                    ViewModels = programPathControl.PathViewModel,
                    MapType = SignalrMapModelEnum.CustomAction,
                    MapAction = (node) => programPathControl.PathViewModel.Text = node[0].Value,
                 },
               new SingalrResultMapModel<ReadProgramNameResultModel>(){
                    ViewModels = programPathControl.ProgramNameViewModel,
                    MapType = SignalrMapModelEnum.CustomAction,
                    MapAction = (node) => programPathControl.ProgramNameViewModel.ProgramName = node[0].Value.Name,
               },
               new SingalrResultMapModel<ReadAlarmResultModel>(){
                    ViewModels = warnControl.wnListViewModel,
                    MapType = SignalrMapModelEnum.CustomAction,
                    MapAction=(node)=>{
                        Dispatcher.BeginInvoke(new Action(()=>{
                              warnControl.wnListViewModel.WarningList.Clear();
                        node.ForEach(d=>{
                            d.Value.ForEach(g=>{
                                 warnControl.wnListViewModel.WarningList.Add(new WarningInfo(){
                                     Id=g.TtypeStr,
                                     Message=g.Message+g.AxisStr,
                                     Number=g.NumStr
                                 } );
                            });
                        });

                        }));
                      
                    }
               }
            };
            return list;
        }

        public override void CncOnError(string message)
        {
        }
    }
}
