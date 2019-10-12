using GalaSoft.MvvmLight;
using MMK.SmartSystem.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Laser.Base.MachineProcess.UserControls.ViewModel
{
    public class ProcessListViewModel:ViewModelBase
    {
        private ObservableCollection<CuttingDataDto> _CuttingDataList;
        public ObservableCollection<CuttingDataDto> CuttingDataList
        {
            get { return _CuttingDataList; }
            set     
            {
                if (_CuttingDataList != value)
                {
                    _CuttingDataList = value;
                    RaisePropertyChanged(() => CuttingDataList);
                }
            }
        }

        private ObservableCollection<EdgeCuttingDataDto> _EdgeCuttingDataList;
        public ObservableCollection<EdgeCuttingDataDto> EdgeCuttingDataList
        {
            get { return _EdgeCuttingDataList; }
            set
            {
                if (_EdgeCuttingDataList != value)
                {
                    _EdgeCuttingDataList = value;
                    RaisePropertyChanged(() => EdgeCuttingDataList);
                }
            }
        }

        private ObservableCollection<PiercingDataDto> _PiercingDataList;
        public ObservableCollection<PiercingDataDto> PiercingDataList
        {
            get { return _PiercingDataList; }
            set
            {
                if (_PiercingDataList != value)
                {
                    _PiercingDataList = value;
                    RaisePropertyChanged(() => PiercingDataList);
                }
            }
        }

        private ObservableCollection<SlopeControlDataDto> _SlopeControlDataList;
        public ObservableCollection<SlopeControlDataDto> SlopeControlDataList
        {
            get { return _SlopeControlDataList; }
            set
            {
                if (_SlopeControlDataList != value)
                {
                    _SlopeControlDataList = value;
                    RaisePropertyChanged(() => SlopeControlDataList);
                }
            }
        }
    }
}
