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
            CutCenterItemList.Add(new MacroManualItemViewModel() { Id = "CutCenterPage_Power", Title = "X机械坐标" });
            CutCenterItemList.Add(new MacroManualItemViewModel() { Id = "CutCenterPage_Freq", Title = "Y机械坐标" });
            CutCenterItemList.Add(new MacroManualItemViewModel() { Id = "CutCenterPage_Dutycycle", Title = "Z轴坐标下限" });
        }
    }
}
