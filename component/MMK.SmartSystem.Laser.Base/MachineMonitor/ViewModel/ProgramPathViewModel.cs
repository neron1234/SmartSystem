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
    public class ProgramPathViewModel:  CncResultViewModel<ReadProgramStrResultModel>
    {
        private string _Text;
        public string Text
        {
            get { return _Text; }
            set
            {
                if (_Text != value)
                {
                    _Text = value;
                    RaisePropertyChanged(() => Text);
                }
            }
        }



      

    }


    public class ProgramNameViewModel : CncResultViewModel<ReadProgramNameResultModel>
    {
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
    }
}
