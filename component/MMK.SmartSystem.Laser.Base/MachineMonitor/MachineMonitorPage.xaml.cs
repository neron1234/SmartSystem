using Abp.Dependency;
using MMK.SmartSystem.Common.SignalrProxy;
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
using MMK.SmartSystem.WebCommon.DeviceModel;
using Newtonsoft.Json.Linq;

namespace MMK.SmartSystem.Laser.Base.MachineMonitor
{

    public class DataViewDealModel<T>
    {
        public List<T> InitViewModel(JObject json)
        {
            var name = json["fullNamespace"]?.ToString();
            if (string.IsNullOrEmpty(name))
            {
                return new List<T>();
            }
            var str = json["value"].ToString();
            try
            {
                if (name == typeof(T).FullName)
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<List<T>>(str);

                }
                return new List<T>();
            }
            catch (Exception)
            {
                return new List<T>();

            }
        }

    }


    /// <summary>
    /// MachineMonitorPage.xaml 的交互逻辑
    /// </summary>
    public partial class MachineMonitorPage : Page, ITransientDependency
    {
        SignalrProxyClient signalrProxyClient;
        DataViewDealModel<ReadPmcResultItemModel> pmcResult = new DataViewDealModel<ReadPmcResultItemModel>();
        DataViewDealModel<ReadPositionResultItemModel> pmcPositionResult = new DataViewDealModel<ReadPositionResultItemModel>();

        DataViewDealModel<ReadProgramStrResultModel> progrogramResult = new DataViewDealModel<ReadProgramStrResultModel>();
        public MachineMonitorPage()
        {
            InitializeComponent();
            signalrProxyClient = new SignalrProxyClient();
            signalrProxyClient.CncErrorEvent += SignalrProxyClient_CncErrorEvent;
            signalrProxyClient.HubRefreshModelEvent += SignalrProxyClient_HubRefreshModelEvent;
            this.Loaded += MachineMonitorPage_Loaded;
            this.Unloaded += CoordinateControl_Unloaded;
        }

        private void MachineMonitorPage_Loaded(object sender, RoutedEventArgs e)
        {
            CoordinateControl_Loaded();
        }

        private void SignalrProxyClient_HubRefreshModelEvent(WebCommon.HubModel.HubResultModel obj)
        {
            JObject jobject = JObject.Parse(obj.Data.ToString());
            var listPmc = pmcResult.InitViewModel(jobject);
            if (listPmc.Count > 0)
            {
                foreach (var item in coordinateControl.ControlViewModel.GetType().GetProperties())
                {
                    var propValue = listPmc.FirstOrDefault(d => d.Id == item.Name);
                    if (propValue != null)
                    {
                        item.SetValue(coordinateControl.ControlViewModel, propValue.Value);
                    }
                }
            }

            var listPostion = pmcPositionResult.InitViewModel(jobject);
            if (listPostion.Count > 0)
            {
                foreach (var item in coordinateControl.ControlViewModel.GetType().GetProperties())
                {
                    var propValue = listPostion.FirstOrDefault(d => d.Id == item.Name);
                    if (propValue != null)
                    {
                        item.SetValue(coordinateControl.ControlViewModel, propValue.Value.ToString());
                    }
                }
            }
            var listProgram = progrogramResult.InitViewModel(jobject);
            if (listProgram.Count > 0)
            {
                programPathControl.PathViewModel.Text = listProgram[0].Value;


            }
        }

        private void SignalrProxyClient_CncErrorEvent(string obj)
        {
        }

        private async void CoordinateControl_Unloaded(object sender, RoutedEventArgs e)
        {
            await signalrProxyClient.Close();
        }


        private async void CoordinateControl_Loaded()
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

            cncEventDatas.Add(new CncEventData()
            {
                Kind = CncEventEnum.ReadProgramStr,
                Para = "programPathControl",
            });
            signalrProxyClient.SendCncData(cncEventDatas);
        }

        private async void Btn_Click(object sender, RoutedEventArgs e)
        {
            var res = await signalrProxyClient.SendAction<BaseCNCResultModel<ReadProgramListItemResultModel>>("ReadProgramList", "//CNC_MEM/USER/PATH1/");
        }
    }
}
