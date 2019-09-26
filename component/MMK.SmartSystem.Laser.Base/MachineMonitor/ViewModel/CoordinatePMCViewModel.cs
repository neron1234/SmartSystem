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
    public class CoordinatePMCViewModel: CncResultViewModel<ReadPmcResultItemModel>
    {
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


        public CoordinatePMCViewModel()
        {
            AbsX = "4821";
            AbsY = "34";
            AbsZ = "810";
        }
    }
}
