using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.LE.Host.SystemControl.ViewModel
{
    public class BottomWarningLogViewModel:ViewModelBase
    {
        private string _WarningLogStr;
        public string WarningLogStr
        {
            get { return _WarningLogStr; }
            set
            {
                if (_WarningLogStr != value)
                {
                    _WarningLogStr = value;
                    RaisePropertyChanged(() => WarningLogStr);
                }
            }
        }
        public BottomWarningLogViewModel()
        {

        }
    }
}
