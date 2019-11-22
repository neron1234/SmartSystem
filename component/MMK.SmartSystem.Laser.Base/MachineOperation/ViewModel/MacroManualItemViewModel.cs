using GalaSoft.MvvmLight.Command;
using MMK.SmartSystem.Common.ViewModel;
using MMK.SmartSystem.WebCommon.DeviceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MMK.SmartSystem.Laser.Base.MachineOperation.ViewModel
{
    public class MacroManualItemViewModel : CncResultViewModel<ReadMacroResultItemModel>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public int MinValue { get; set; } = 0;

        public int MaxValue { get; set; } = 100;

        private string _value;
        public string Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    RaisePropertyChanged(() => Value);
                }
            }
        }

        public event Action<MacroManualItemViewModel> InputClickEvent;
        public ICommand InputCommand
        {
            get
            {
                return new RelayCommand(() => InputClickEvent?.Invoke(this));
            }
        }

        public MacroManualItemViewModel()
        {
            Value = "0";
        }
    }
}
