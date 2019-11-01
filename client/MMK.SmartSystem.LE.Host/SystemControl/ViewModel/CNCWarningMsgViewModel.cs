using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.LE.Host.SystemControl.ViewModel
{
    public class CNCWarningMsgViewModel:ViewModelBase
    {
        public ObservableCollection<WarningMsgInfo> WarningMsgList { get; set; }
        public CNCWarningMsgViewModel()
        {
            WarningMsgList = new ObservableCollection<WarningMsgInfo> { 
                new WarningMsgInfo{ Number = "0005",Type = "101", Msg = "报警0005"},
                new WarningMsgInfo{ Number = "0006",Type = "102", Msg = "报警0006"},
                new WarningMsgInfo{ Number = "0007",Type = "101", Msg = "报警0007"},
                new WarningMsgInfo{ Number = "0008",Type = "102", Msg = "报警0008"}
            };
        }
    }

    public class WarningMsgInfo
    {
        public string Type { get; set; }
        public string Number { get; set; }
        public string Msg { get; set; }
    }
}
