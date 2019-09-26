using GalaSoft.MvvmLight;
using MMK.SmartSystem.Common.ViewModel;
using MMK.SmartSystem.WebCommon.DeviceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Laser.Base.MachineMonitor.ViewModel
{
    public class CoordinateControlViewModel: CncResultViewModel<ReadPmcResultItemModel>
    {
        private string _MachineX;
        public string MachineX
        {
            get { return _MachineX; }
            set
            {
                if (_MachineX != value)
                {
                    _MachineX = value;
                    RaisePropertyChanged(() => MachineX);
                }
            }
        }

        private string _MachineY;
        public string MachineY
        {
            get { return _MachineY; }
            set
            {
                if (_MachineY != value)
                {
                    _MachineY = value;
                    RaisePropertyChanged(() => MachineY);
                }
            }
        }

        private string _MachineZ;
        public string MachineZ
        {
            get { return _MachineZ; }
            set
            {
                if (_MachineZ != value)
                {
                    _MachineZ = value;
                    RaisePropertyChanged(() => MachineZ);
                }
            }
        }

        private string _AbsX;
        public string AbsX
        {
            get { return _AbsX; }
            set
            {
                if (_AbsX != value)
                {
                    _AbsX = value;
                    RaisePropertyChanged(() => AbsX);
                }
            }
        }

        private string _AbsY;
        public string AbsY
        {
            get { return _AbsY; }
            set
            {
                if (_AbsY != value)
                {
                    _AbsY = value;
                    RaisePropertyChanged(() => AbsY);
                }
            }
        }

        private string _AbsZ;
        public string AbsZ
        {
            get { return _AbsZ; }
            set
            {
                if (_AbsZ != value)
                {
                    _AbsZ = value;
                    RaisePropertyChanged(() => AbsZ);
                }
            }
        }

        private string _ResidualMoveNumber1;
        public string ResidualMoveNumber1
        {
            get { return _ResidualMoveNumber1; }
            set
            {
                if (_ResidualMoveNumber1 != value)
                {
                    _ResidualMoveNumber1 = value;
                    RaisePropertyChanged(() => ResidualMoveNumber1);
                }
            }
        }

        private string _ResidualMoveNumber2;
        public string ResidualMoveNumber2
        {
            get { return _ResidualMoveNumber2; }
            set
            {
                if (_ResidualMoveNumber2 != value)
                {
                    _ResidualMoveNumber2 = value;
                    RaisePropertyChanged(() => ResidualMoveNumber2);
                }
            }
        }

        private string _ResidualMoveNumber3;
        public string ResidualMoveNumber3
        {
            get { return _ResidualMoveNumber3; }
            set
            {
                if (_ResidualMoveNumber3 != value)
                {
                    _ResidualMoveNumber3 = value;
                    RaisePropertyChanged(() => ResidualMoveNumber3);
                }
            }
        }

        public CoordinateControlViewModel()
        {
            ResidualMoveNumber1 = "41241";
            ResidualMoveNumber2 = "12312";
            ResidualMoveNumber3 = "64521";

            MachineX = "1412";
            MachineY = "15";
            MachineZ = "124";
            AbsX = "4821";
            AbsY = "34";
            AbsZ = "810";

        }
    }
}
