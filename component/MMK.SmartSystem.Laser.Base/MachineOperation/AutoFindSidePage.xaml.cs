using Abp.Dependency;
using MMK.SmartSystem.Common.ViewModel;
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
using System.Reflection;
using MMK.SmartSystem.Common.Base;
using MMK.SmartSystem.Common.Model;
using MMK.SmartSystem.WebCommon.DeviceModel;

namespace MMK.SmartSystem.Laser.Base.MachineOperation
{
    /// <summary>
    /// AutoFindSidePage.xaml 的交互逻辑
    ///  自动寻边
    /// </summary>
    public partial class AutoFindSidePage : SignalrPage
    {
        public override string GetModule => "MachineOperation";
        private AutoFindSidePageViewModel AutoFindSidePageViewModel = new AutoFindSidePageViewModel();
        public AutoFindSidePage()
        {
            InitializeComponent();
            AutoFindSidePageViewModel.InputClickEvent += AutoFindSidePageViewModel_InputClickEvent;
            manualControl.SetHeaderActive(this);
            PageItemControl.ItemsSource = AutoFindSidePageViewModel.AutoFindSideItemList;
        }

        private void AutoFindSidePageViewModel_InputClickEvent(MacroManualItemViewModel obj)
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
                          var info =  AutoFindSidePageViewModel.AutoFindSideItemList.FirstOrDefault(f=>f.Id==d.Id);
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
