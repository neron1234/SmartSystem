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

        private async void Btn_Click(object sender, RoutedEventArgs e)
        {
            await SendProxyAction<BaseCNCResultModel<ReadProgramListItemResultModel>>(SinglarCNCHubConsts.ReadProgramListAction, "//CNC_MEM/USER/PATH1/");
        }

        public override void PageSignlarLoaded()
        {

        }

        public override List<CncEventData> GetCncEventData()
        {
            List<CncEventData> cncEventDatas = new List<CncEventData>();
            cncEventDatas.Add(new CncEventData()
            {
                Kind = CncEventEnum.ReadPmc,

                Para = Newtonsoft.Json.JsonConvert.SerializeObject(new ReadPmcModel()
                {
                    Decompilers = new List<DecompReadPmcItemModel>()
                    {
                        new DecompReadPmcItemModel() {Id="AbsX", AdrType=5, Bit=null, DataType=DataTypeEnum.Int32,RelStartAdr=0 },
                        new DecompReadPmcItemModel() {Id="AbsY", AdrType=5, Bit=null, DataType=DataTypeEnum.Int32,RelStartAdr=4 },
                        new DecompReadPmcItemModel() {Id="AbsZ",AdrType=5, Bit=null, DataType=DataTypeEnum.Int32,RelStartAdr=8 },


                    },
                    Readers = new List<ReadPmcTypeModel>()
                    {
                        new ReadPmcTypeModel(){ AdrType=5,DwordQuantity=10,StartNum=0}
                    }
                })
            });
            cncEventDatas.Add(new CncEventData()
            {
                Kind = CncEventEnum.ReadPosition,
                Para = Newtonsoft.Json.JsonConvert.SerializeObject(new ReadPositionModel()
                {
                    Decompilers = new List<DecompReadPositionItemModel>()
                    {
                        new DecompReadPositionItemModel() {Id="MachineX", AxisNum=1,PositionType=CncPositionTypeEnum.Machine },
                        new DecompReadPositionItemModel() {Id="MachineY", AxisNum=2,PositionType=CncPositionTypeEnum.Machine  },
                        new DecompReadPositionItemModel() {Id="MachineZ",AxisNum=3,PositionType=CncPositionTypeEnum.Machine },

                         new DecompReadPositionItemModel() {Id="ResidualMoveNumber1", AxisNum=1,PositionType=CncPositionTypeEnum.Distance },
                         new DecompReadPositionItemModel() {Id="ResidualMoveNumber2", AxisNum=2,PositionType=CncPositionTypeEnum.Distance  },
                         new DecompReadPositionItemModel() {Id="ResidualMoveNumber3",AxisNum=3,PositionType=CncPositionTypeEnum.Distance },
                    },
                    Readers = new List<ReadPositionTypeModel>()
                    {
                        new ReadPositionTypeModel(){ PositionType=CncPositionTypeEnum.Machine},
                       new ReadPositionTypeModel(){ PositionType=CncPositionTypeEnum.Distance},

                    }
                })
            });

            cncEventDatas.Add(new CncEventData()
            {
                Kind = CncEventEnum.ReadMacro,
                Para = Newtonsoft.Json.JsonConvert.SerializeObject(new ReadMacroModel()
                {
                    Decompilers = new List<DecompReadMacroItemModel>(){
                       new DecompReadMacroItemModel()
                       {
                           Id = "AutoFindSide_GET_H"
                       }
                    },
                    Readers = new List<ReadMacroTypeModel>()
                    {
                        new ReadMacroTypeModel(){ Quantity=10,StartNum=813}
                    }
                })
            });

            //cncEventDatas.Add(new CncEventData()
            //{
            //    Kind = CncEventEnum.ReadProgramStr,
            //    Para = Newtonsoft.Json.JsonConvert.SerializeObject(new ReadProgramNameModel()
            //    {
            //        Readers = new List<string>() { "programPathControl" }
            //    })
            //});

            cncEventDatas.Add(new CncEventData()
            {
                Kind = CncEventEnum.ReadProgramName,
                Para = Newtonsoft.Json.JsonConvert.SerializeObject(new ReadProgramNameModel()
                {
                    Readers = new List<string>() { "programPathControl" }
                })
            });

            return cncEventDatas;
        }

        public override List<object> GetResultViewModelMap()
        {
            List<object> list = new List<object>()
            {
                new SingalrResultMapModel<ReadPmcResultItemModel>()
                {
                    ViewModels = coordinateControl.PMCViewModel,
                    MapType = SignalrMapModelEnum.AutoPropMap,
                    AutoPropMapAction = (node, propName) => node.FirstOrDefault(d => d.Id == propName)?.Value
                },
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
                 }
            };
            return list;
        }

        public override void CncOnError(string message)
        {
        }
    }
}
