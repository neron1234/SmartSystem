using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Laser.Base.MachineOperation.ViewModel
{
    public class CutCenterPageViewModel:ViewModelBase
    {
        public List<MacroManualItemViewModel> CutCenterItemList { get; set; } = new List<MacroManualItemViewModel>();

        public event Action<MacroManualItemViewModel> InputClickEvent;

        public CutCenterPageViewModel()
        {
            CutCenterItemList.Add(new MacroManualItemViewModel() { Id = "CutCenterPage_Power", Title = "功率(W)" });
            CutCenterItemList.Add(new MacroManualItemViewModel() { Id = "CutCenterPage_Freq", Title = "频率(Hz)" });
            CutCenterItemList.Add(new MacroManualItemViewModel() { Id = "CutCenterPage_Dutycycle", Title = "占空比(%)" });
            CutCenterItemList.Add(new MacroManualItemViewModel() { Id = "CutCenterPage_Time", Title = "时间(sec)" });
            CutCenterItemList.ForEach(d => d.InputClickEvent += D_InputClickEvent);
        }

        private void D_InputClickEvent(MacroManualItemViewModel obj)
        {
            InputClickEvent?.Invoke(obj);
        }
    }
}
