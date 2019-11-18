using Abp.Dependency;
using MMK.SmartSystem.Common.Base;
using MMK.SmartSystem.Common.Model;
using MMK.SmartSystem.Laser.Base.MachineOperation.ViewModel;
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

namespace MMK.SmartSystem.Laser.Base.MachineOperation
{
    /// <summary>
    /// MaualioPage.xaml 的交互逻辑
    /// </summary>
    public partial class MaualioPage : SignalrPage
    {
        public override string GetModule => "MachineOperation";
        public override bool IsRequestResponse => false;
        public ManualioViewModel mioVm { get; set; }
        public MaualioPage()
        {
            InitializeComponent();
            this.DataContext = mioVm = new ManualioViewModel();
            mioVm.IoBtnClickEvent += MioVm_IoBtnClickEvent;
            manualControl.SetHeaderActive(this);
        }

        protected async override void PageSignlarLoaded(){
            await Sleep();
        }

        private async Task Sleep()
        {
            await Task.Run(new Action(() =>
            {
                System.Threading.Thread.Sleep(5000);
                MessageBox.Show("线程醒了");
            }));
        }

        private void MioVm_IoBtnClickEvent(IoBtnInfo obj)
        {
            SendReaderWriter(new WebCommon.HubModel.HubReadWriterModel()
            {
                ProxyName = "PmcInOut",
                Action = "Reversal",
                ConnectId = CurrentConnectId,
                Data = new object[] { obj.AdrType, obj.Adr, obj.Bit }
            });
        }

        public override void CncOnError(string message)
        {

        }

        public override List<object> GetResultViewModelMap()
        {
            List<object> list = new List<object>()
            {
                new SingalrResultMapModel<ReadPmcResultItemModel>()
                {
                    ViewModels =new IoBtnInfo(),
                    MapType = SignalrMapModelEnum.CustomAction,
                    MapAction = (node) =>
                    {
                        node.ForEach(d=>
                        {
                          var info=  mioVm.IoBtnList.FirstOrDefault(f=>f.Id==d.Id);
                            if(info!=null)
                            {
                                if(d.Value.Equals("True",StringComparison.OrdinalIgnoreCase))
                                {
                                     info.StartActive();
                                }
                                else
                                {
                                     info.ClearActive();

                                }

                            }
                        });
                    }
                },
                new SingalrResultMapModel<ReadPmcResultItemModel>()
                {
                    ViewModels=mioVm.OperaModel,
                    MapType=SignalrMapModelEnum.AutoPropMap,
                    AutoPropMapAction=(n,s)=>n.FirstOrDefault(d=>d.Id.Equals(s,StringComparison.OrdinalIgnoreCase))?.Value
                }
            };
            return list;
        }
    }
}
