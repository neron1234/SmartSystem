using GalaSoft.MvvmLight;
using MMK.SmartSystem.Common.ViewModel;
using MMK.SmartSystem.WebCommon.DeviceModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Laser.Base.MachineMonitor.ViewModel
{
    public class WarningListViewModel : CncResultViewModel<ReadAlarmResultModel>
    {
        public ObservableCollection<WarningInfo> WarningList = new ObservableCollection<WarningInfo>();

    }

    public class WarningInfo
    {
        public string Id { get; set; }
        public string Number { get; set; }
        public string Message { get; set; }
    }
}
