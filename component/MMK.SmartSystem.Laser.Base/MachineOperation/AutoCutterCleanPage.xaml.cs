using Abp.Dependency;
using MMK.SmartSystem.Common.Base;
using MMK.SmartSystem.Common.ViewModel;
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
using MMK.SmartSystem.WebCommon.DeviceModel;
using MMK.SmartSystem.Common.SignalrProxy;
using MMK.SmartSystem.Common.Model;
using MMK.SmartSystem.Laser.Base.MachineOperation.ViewModel;

namespace MMK.SmartSystem.Laser.Base.MachineOperation
{
    /// <summary>
    /// AutoCutterCleanPage.xaml 的交互逻辑
    /// 割嘴自动清理
    /// </summary>
    public partial class AutoCutterCleanPage : SignalrPage
    {
        private AutoCutterCleanViewModel AutoCutterCleanVM = new AutoCutterCleanViewModel();
        public AutoCutterCleanPage()
        {
            InitializeComponent();
            AutoCutterCleanVM.InputClickEvent += AutoCutterCleanVM_InputClickEvent;
            manualControl.SetHeaderActive(this);
            PageItemControl.ItemsSource = AutoCutterCleanVM.CutterCleanItems;
        }

        private void AutoCutterCleanVM_InputClickEvent(MacroManualItemViewModel obj)
        {
            var windows = new InputWindow(obj.Value, obj.MinValue, obj.MinValue, obj.Title);
            windows.InputWindowFinishEvent += (s) => obj.Value = s;
            windows.ShowDialog();
        }

        public override void CncOnError(string message)
        {
            
        }

        public override List<CncEventData> GetCncEventData()
        {
            List<CncEventData> cncEventDatas = new List<CncEventData>();
            cncEventDatas.Add(new CncEventData()
            {
                Kind = CncEventEnum.ReadWorkpartNum,
                Para = Newtonsoft.Json.JsonConvert.SerializeObject(new ReadWorkpartNumModel() { 
                    Decompilers = new List<string>()
                    {
                        "YD",
                        "XD"
                    },
                    Readers = new List<string>()
                    {
                        
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
                           Id = "H"
                       },
                       new DecompReadMacroItemModel()
                       {
                           Id = "CleanTime"
                       },
                       new DecompReadMacroItemModel()
                       {
                           Id = "ZLimit"
                       }
                    },
                    Readers = new List<ReadMacroTypeModel>()
                    {
                        new ReadMacroTypeModel(){ Quantity=10,StartNum=813},
                        new ReadMacroTypeModel(){ Quantity=10,StartNum=837},
                        new ReadMacroTypeModel(){ Quantity=10,StartNum=838}
                    }
                })
            });
            return cncEventDatas;
        }

        public override List<object> GetResultViewModelMap()
        {
            return new List<object>()
            {
                new SingalrResultMapModel<ReadMacroResultItemModel>()
                {
                    ViewModels =new MacroManualItemViewModel(),
                    MapType = SignalrMapModelEnum.CustomAction,
                    MapAction = (node) =>
                    {
                        node.ForEach(d=>
                        {
                          var info=  AutoCutterCleanVM.CutterCleanItems.FirstOrDefault(f=>f.Id==d.Id);
                            if(info!=null)
                            {
                                info.Value=d.Value.ToString();

                            }
                        });
                    }
                },
            };
        }
    }
}
