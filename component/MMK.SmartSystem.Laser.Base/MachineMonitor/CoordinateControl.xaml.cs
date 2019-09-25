using MMK.SmartSystem.Common.SignalrProxy;
using MMK.SmartSystem.Laser.Base.MachineMonitor.ViewModel;
using MMK.SmartSystem.WebCommon.DeviceModel;
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

namespace MMK.SmartSystem.Laser.Base.MachineMonitor
{
    /// <summary>
    /// CoordinateControl.xaml 的交互逻辑
    /// </summary>
    public partial class CoordinateControl : UserControl
    {
        SignalrProxyClient signalrProxyClient;
        CoordinateControlViewModel controlViewModel;
        public CoordinateControl()
        {
            InitializeComponent();
            this.DataContext = controlViewModel = new CoordinateControlViewModel();
            signalrProxyClient = new SignalrProxyClient();
            signalrProxyClient.CncErrorEvent += SignalrProxyClient_CncErrorEvent;
            signalrProxyClient.HubRefreshModelEvent += SignalrProxyClient_HubRefreshModelEvent;
            this.Loaded += CoordinateControl_Loaded;
            this.Unloaded += CoordinateControl_Unloaded;
        }

        private void SignalrProxyClient_HubRefreshModelEvent(WebCommon.HubModel.HubResultModel obj)
        {
            var jsonData = obj.Data;
           
        }

        private void SignalrProxyClient_CncErrorEvent(string obj)
        {
        }

        private async void CoordinateControl_Unloaded(object sender, RoutedEventArgs e)
        {
            await signalrProxyClient.Close();
        }

        private async void CoordinateControl_Loaded(object sender, RoutedEventArgs e)
        {
            await signalrProxyClient.Start();



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
            signalrProxyClient.SendCncData(cncEventDatas);
        }
    }
}
