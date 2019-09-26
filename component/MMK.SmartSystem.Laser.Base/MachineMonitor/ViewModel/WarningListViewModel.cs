using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Laser.Base.MachineMonitor.ViewModel
{
    public class WarningListViewModel:ViewModelBase
    {
        private ObservableCollection<WarningInfo> _WarningList;
        public ObservableCollection<WarningInfo> WarningList
        {
            get { return _WarningList; }
            set {
                if (_WarningList != value){
                    _WarningList = value;
                    RaisePropertyChanged(() => WarningList);
                }
            }
        }
        public WarningListViewModel()
        {
            WarningList = new ObservableCollection<WarningInfo>() {
                new WarningInfo {
                    Id = "15",
                    Number = "1001",
                    Message = "THIS IS A ALARM NO 1 CURRENT"
                },new WarningInfo {
                    Id = "15",
                    Number = "1002",
                    Message = "THIS IS A ALARM NO 2 CURRENT"
                },new WarningInfo {
                    Id = "15",
                    Number = "1003",
                    Message = "THIS IS A ALARM NO 3 CURRENT"
                },new WarningInfo {
                    Id = "16",
                    Number = "1004",
                    Message = "THIS IS A ALARM NO 4 CURRENT"
                },new WarningInfo {
                    Id = "16",
                    Number = "1005",
                    Message = "THIS IS A ALARM NO 5 CURRENT"
                }
            };
        }
    }

    public class WarningInfo
    {
        public string Id { get; set; }
        public string Number { get; set; }
        public string Message { get; set; }
    }
}
