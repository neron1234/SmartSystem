using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Laser.Base.MachineProcess.UserControls.ViewModel
{
    public class ProcessOptionsViewModel:ViewModelBase
    {
        private ObservableCollection<string> _MaterialTypeList;
        public ObservableCollection<string> MaterialTypeList
        {
            get { return _MaterialTypeList; }
            set
            {
                if (_MaterialTypeList != value)
                {
                    _MaterialTypeList = value;
                    RaisePropertyChanged(() => MaterialTypeList);
                }
            }
        }

        private ObservableCollection<string> _MaterialThicknessList;
        public ObservableCollection<string> MaterialThicknessList
        {
            get { return _MaterialThicknessList; }
            set
            {
                if (_MaterialThicknessList != value)
                {
                    _MaterialThicknessList = value;
                    RaisePropertyChanged(() => MaterialThicknessList);
                }
            }
        }

        public ProcessOptionsViewModel()
        {
            this.MaterialTypeList = new ObservableCollection<string> { 
                "铜",
                "铁",
                "铝"
            };
            this.MaterialThicknessList = new ObservableCollection<string>
            {
                "10",
                "12",
                "15"
            };
        }
    }
}
