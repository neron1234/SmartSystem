using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.LE.Host.ViewModel
{
    public class CNCWarningMsgViewModel:ViewModelBase
    {
        public ObservableCollection<WarningMsgInfo> WarningMsgList { get; set; }
        public CNCWarningMsgViewModel()
        {
            WarningMsgList = new ObservableCollection<WarningMsgInfo> {
                new WarningMsgInfo{ Number = "0005",Type = "101", Msg = "报警0005",Img = "/MMK.SmartSystem.LE.Host;component/Resources/Images/WarningIcon.png"},
                new WarningMsgInfo{ Number = "0006",Type = "102", Msg = "报警0006",Img = "/MMK.SmartSystem.LE.Host;component/Resources/Images/WarningIcon.png"},
                new WarningMsgInfo{ Number = "0007",Type = "101", Msg = "报警0007",Img = "/MMK.SmartSystem.LE.Host;component/Resources/Images/WarningIcon.png"},
                new WarningMsgInfo{ Number = "0008",Type = "102", Msg = "报警0008",Img = "/MMK.SmartSystem.LE.Host;component/Resources/Images/WarningIcon.png"}
            };
        }
    }

    public class WarningMsgInfo
    {
        public string Type { get; set; }
        public string Number { get; set; }
        public string Img { get; set; }
        public string Msg { get; set; }
    }
}
