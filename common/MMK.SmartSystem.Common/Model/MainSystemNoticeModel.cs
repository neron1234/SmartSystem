using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Common.Model
{
    public enum ErrorTagretEnum
    {
        UserControl,
        Page,
        Window,
        MainWindow
    }

    public enum EventEnum
    {
        NavHome,
        RefreshAuth
        
    }
    public class MainSystemNoticeModel : ViewModelBase
    {
        public ErrorTagretEnum Tagret { get; set; }

        public EventEnum EventType { get; set; }
        public string Error { get; set; }

        public bool Success { set; get; }

      

    }
}
