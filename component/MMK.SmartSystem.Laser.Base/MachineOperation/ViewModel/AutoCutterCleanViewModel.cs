using GalaSoft.MvvmLight;
using MMK.SmartSystem.Common.ViewModel;
using MMK.SmartSystem.WebCommon.DeviceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Laser.Base.MachineOperation.ViewModel
{
    public class AutoCutterCleanViewModel: ViewModelBase
    {
        public List<MacroManualItemViewModel> CutterCleanItems { get; set; } = new List<MacroManualItemViewModel>();

        public event Action<MacroManualItemViewModel> InputClickEvent;

        public AutoCutterCleanViewModel()
        {
            CutterCleanItems.Add(new MacroManualItemViewModel() { Id = "AutoCutterClean_Center_XD", Title = "铜刷中心位置" });
            CutterCleanItems.Add(new MacroManualItemViewModel() { Id = "AutoCutterClean_Center_YD", Title = "铜刷中心位置" });
            CutterCleanItems.Add(new MacroManualItemViewModel() { Id = "AutoCutterClean_ZOFS_H", Title = "Z轴机械坐标下限" });
            CutterCleanItems.Add(new MacroManualItemViewModel() { Id = "AutoCutterClean_RoundTrips", Title = "割嘴高度偏置" });
            CutterCleanItems.Add(new MacroManualItemViewModel() { Id = "AutoCutterClean_Zlimit", Title = "割嘴清洁往返次数" });

            CutterCleanItems.ForEach(d => d.InputClickEvent += D_InputClickEvent);
        }

        private void D_InputClickEvent(MacroManualItemViewModel obj)
        {
            InputClickEvent?.Invoke(obj);
        }
    }
}
