using GalaSoft.MvvmLight;
using MMK.SmartSystem.Common.EventDatas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.LE.Host.ViewModel
{
    public class MainSystemNoticeModel : ViewModelBase
    {
        public ErrorTagretEnum Tagret { get; set; }

        public string Error { get; set; }

        public bool Success { set; get; }

        public Action SuccessAction { set; get; }

        public Action ErrorAction { set; get; }

    }
}
