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
    /// CutCenterPage.xaml 的交互逻辑
    /// 割嘴对中
    /// </summary>
    public partial class CutCenterPage : SignalrPage
    {
        public override string GetModule => "MachineOperation";
        private CutCenterPageViewModel CutCenterPageVM = new CutCenterPageViewModel();
        public CutCenterPage()
        {
            InitializeComponent();
            CutCenterPageVM.InputClickEvent += CutCenterPageVM_InputClickEvent;
            manualControl.SetHeaderActive(this);
        }

        private void CutCenterPageVM_InputClickEvent(MacroManualItemViewModel obj)
        {
            var windows = new InputWindow(obj.Value, obj.MinValue, obj.MinValue, obj.Title);
            windows.InputWindowFinishEvent += (s) => obj.Value = s;
            windows.ShowDialog();
        }

        public override void CncOnError(string message)
        {

        }

        public override List<object> GetResultViewModelMap(){
            return new List<object>(){
                new SingalrResultMapModel<ReadMacroResultItemModel>(){
                    ViewModels =new MacroManualItemViewModel(),
                    MapType = SignalrMapModelEnum.CustomAction,
                    MapAction = (node) =>{
                        node.ForEach(d=>{
                          var info =  CutCenterPageVM.CutCenterItemList.FirstOrDefault(f=>f.Id==d.Id);
                            if(info != null){
                                info.Value=d.Value.ToString();
                            }
                        });
                    }
                },
            };
        }
    }
}
