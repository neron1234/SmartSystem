using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MMK.SmartSystem.Laser.Base.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MMK.SmartSystem.Laser.Base.MachineMaintain.UserControls.ViewModel
{
    public class SoftSettingViewModel:ViewModelBase
    {
        public SwitchBtnViewModel SwitchBtn { get; set; } = new SwitchBtnViewModel();

        public SoftSettingViewModel(){
           
        }
    }
}
