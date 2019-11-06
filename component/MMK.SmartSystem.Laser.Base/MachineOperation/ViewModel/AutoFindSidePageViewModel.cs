using MMK.SmartSystem.Common.ViewModel;
using MMK.SmartSystem.WebCommon.DeviceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Laser.Base.MachineOperation.ViewModel
{
    public class AutoFindSidePageViewModel : MainTranslateViewModel{
        public List<MacroManualItemViewModel> AutoFindSideItemList { get; set; } = new List<MacroManualItemViewModel>();

        public event Action<MacroManualItemViewModel> InputClickEvent;

        public AutoFindSidePageViewModel(){
            AutoFindSideItemList.Add(new MacroManualItemViewModel() { Id = "simpleprofile_h", Title = "引线H" });
            AutoFindSideItemList.Add(new MacroManualItemViewModel() { Id = "simpleprofile_i", Title = "长度I" });
            AutoFindSideItemList.Add(new MacroManualItemViewModel() { Id = "simpleprofile_j", Title = "宽度J" });
            AutoFindSideItemList.Add(new MacroManualItemViewModel() { Id = "simpleprofile_d", Title = "直径D" });
            AutoFindSideItemList.Add(new MacroManualItemViewModel() { Id = "simpleprofile_r", Title = "半径R" });
            AutoFindSideItemList.Add(new MacroManualItemViewModel() { Id = "simpleprofile_e", Title = "切割E" });
            AutoFindSideItemList.ForEach(d => d.InputClickEvent += D_InputClickEvent);
        }

        private void D_InputClickEvent(MacroManualItemViewModel obj)
        {
            InputClickEvent?.Invoke(obj);
        }
    }
}
