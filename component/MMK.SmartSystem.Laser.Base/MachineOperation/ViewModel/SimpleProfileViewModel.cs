using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MMK.SmartSystem.Common.ViewModel;
using MMK.SmartSystem.WebCommon.DeviceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.Concurrent;
namespace MMK.SmartSystem.Laser.Base.MachineOperation.ViewModel
{
    public class SimpleProfileViewModel : ViewModelBase
    {
        public List<MacroManualItemViewModel> SimpleItems { get; set; } = new List<MacroManualItemViewModel>();

        public event Action<MacroManualItemViewModel> InputClickEvent;

        public SimpleProfileViewModel()
        {
            SimpleItems.Add(new MacroManualItemViewModel() { Id = "AutoFindSide_ZeroPoint_XD", Title = "板材原点D" });
            SimpleItems.Add(new MacroManualItemViewModel() { Id = "AutoFindSide_ZeroPoint_YD", Title = "板材原点D" });
            SimpleItems.Add(new MacroManualItemViewModel() { Id = "AutoFindSide_Angle_SITA", Title = "板材倾斜角度" });
            SimpleItems.Add(new MacroManualItemViewModel() { Id = "AutoFindSide_CenterP1P2_H", Title = "中间点P1、P2距离H" });
            SimpleItems.Add(new MacroManualItemViewModel() { Id = "AutoFindSide_OPOFS_X", Title = "光电开关偏置位置 X" });
            SimpleItems.Add(new MacroManualItemViewModel() { Id = "AutoFindSide_OPOFS_Y", Title = "光电开关偏置位置 Y" });
            SimpleItems.Add(new MacroManualItemViewModel() { Id = "AutoFindSide_SideRemainRH_M", Title = "板材边缘的垂直预留距离 Y" });

            SimpleItems.ForEach(d => d.InputClickEvent += D_InputClickEvent);
        }

        private void D_InputClickEvent(MacroManualItemViewModel obj)
        {
            InputClickEvent?.Invoke(obj);
        }
    }
}
