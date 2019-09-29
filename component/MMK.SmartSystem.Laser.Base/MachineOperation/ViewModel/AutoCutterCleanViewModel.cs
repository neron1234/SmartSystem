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
    public class AutoCutterCleanViewModel: CncResultViewModel<ReadMacroResultItemModel>
    {
        private string _XD;
        public string XD
        {
            get { return _XD; }
            set
            {
                if (_XD != value)
                {
                    _XD = value;
                    RaisePropertyChanged(() => XD);
                }
            }
        }

        private string _YD;
        public string YD
        {
            get { return _YD; }
            set
            {
                if (_YD != value)
                {
                    _YD = value;
                    RaisePropertyChanged(() => YD);
                }
            }
        }

        private string _H;
        public string H
        {
            get { return _H; }
            set
            {
                if (_H != value)
                {
                    _H = value;
                    RaisePropertyChanged(() => H);
                }
            }
        }

        private string  _CleanTime;
        public string  CleanTime
        {
            get { return _CleanTime; }
            set
            {
                if (_CleanTime != value)
                {
                    _CleanTime = value;
                    RaisePropertyChanged(() => CleanTime);
                }
            }
        }

        private string _ZLimit;
        public string ZLimit
        {
            get { return _ZLimit; }
            set
            {
                if (_ZLimit != value)
                {
                    _ZLimit = value;
                    RaisePropertyChanged(() => ZLimit);
                }
            }
        }
        public AutoCutterCleanViewModel()
        {
            
        }
    }
}
