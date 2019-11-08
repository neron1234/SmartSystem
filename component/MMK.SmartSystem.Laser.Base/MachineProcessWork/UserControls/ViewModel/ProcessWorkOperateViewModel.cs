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

        private System.Windows.Visibility _OperateVisibility;
        public System.Windows.Visibility OperateVisibility
        {
            get { return _OperateVisibility; }
            set
            {
                if (_OperateVisibility != value)
                {
                    _OperateVisibility = value;
                    if (value == System.Windows.Visibility.Visible)
                    {
                        this.ProgramVisibility = System.Windows.Visibility.Collapsed;
                    }
                    RaisePropertyChanged(() => OperateVisibility);
                }
            }
        }

        private System.Windows.Visibility _ProgramVisibility;
        public System.Windows.Visibility ProgramVisibility
        {
            get { return _ProgramVisibility; }
            set
            {
                if (_ProgramVisibility != value)
                {
                    _ProgramVisibility = value;
                    if (value == System.Windows.Visibility.Visible)
                    {
                        this.OperateVisibility = System.Windows.Visibility.Collapsed;
                    }
                    RaisePropertyChanged(() => ProgramVisibility);
                }
            }
        }

        public ICommand SwitchPageCommand{
            get{
                return new RelayCommand<string>((str) => {
                    if (str == "0"){
                        ProgramVisibility = System.Windows.Visibility.Visible;
                    }else{
                        OperateVisibility = System.Windows.Visibility.Visible;
                    }
                });
            }
        }

        public SwitchBtnViewModel BreakPointCutSwitchBtn { get; set; } = new SwitchBtnViewModel();
        public SwitchBtnViewModel NCutSwitchBtn { get; set; } = new SwitchBtnViewModel();
        public SwitchBtnViewModel StopSwitchBtn { get; set; } = new SwitchBtnViewModel();

        public ProcessWorkOperateViewModel()
        {
            OperateVisibility = System.Windows.Visibility.Visible;
            ProgramVisibility = System.Windows.Visibility.Collapsed;
            PreviewText = "(Model: LFK LCM3015) \r\n(Date: 10 / 15 / 19 Time: 11:16:36) \r\n(RST 37 - 2 0.8) \r\n(Sheet size: 3000x1500) \r\nG92 X0.0Y0.0 \r\nG0G90X38.57Y252.28 \r\nG13  \r\n G32 L1 E002  \r\n M70";
        }
    }
}
