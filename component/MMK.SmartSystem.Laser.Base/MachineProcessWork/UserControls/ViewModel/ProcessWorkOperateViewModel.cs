using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MMK.SmartSystem.Laser.Base.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MMK.SmartSystem.Laser.Base.MachineProcessWork.UserControls.ViewModel
{
    public class ProcessWorkOperateViewModel: ViewModelBase
    {
        private string _PreviewText;
        public string PreviewText
        {
            get { return _PreviewText; }
            set
            {
                if (_PreviewText != value)
                {
                    _PreviewText = value;
                    RaisePropertyChanged(() => PreviewText);
                }
            }
        }

        public SwitchBtnViewModel BreakPointCutSwitchBtn { get; set; } = new SwitchBtnViewModel();
        public SwitchBtnViewModel NCutSwitchBtn { get; set; } = new SwitchBtnViewModel();
        public SwitchBtnViewModel StopSwitchBtn { get; set; } = new SwitchBtnViewModel();

        public ProcessWorkOperateViewModel()
        {
            PreviewText = "(Model: LFK LCM3015) \r\n(Date: 10 / 15 / 19 Time: 11:16:36) \r\n(RST 37 - 2 0.8) \r\n(Sheet size: 3000x1500) \r\nG92 X0.0Y0.0 \r\nG0G90X38.57Y252.28 \r\nG13  \r\n G32 L1 E002  \r\n M70";
        }
    }
}
