using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.LE.Host.SystemControl.ViewModel
{
    public class HeaderTitleMenuViewModel:ViewModelBase
    {
        private string _Title;
        public string Title
        {
            get { return _Title; }
            set
            {
                if (_Title != value)
                {
                    _Title = value;
                    RaisePropertyChanged(() => Title);
                }
            }
        }

        private string _ProgramName;
        public string ProgramName
        {
            get { return _ProgramName; }
            set
            {
                if (_ProgramName != value)
                {
                    _ProgramName = value;
                    RaisePropertyChanged(() => ProgramName);
                }
            }
        }

        private string _WarnStr;
        public string WarnStr
        {
            get { return _WarnStr; }
            set
            {
                if (_WarnStr != value)
                {
                    _WarnStr = value;
                    RaisePropertyChanged(() => WarnStr);
                }
            }
        }

        public HeaderTitleMenuViewModel()
        {
            this.Title = "手动";
            this.ProgramName = "09100";
            this.WarnStr = "机器出现一般故障";
        }
    }
}
