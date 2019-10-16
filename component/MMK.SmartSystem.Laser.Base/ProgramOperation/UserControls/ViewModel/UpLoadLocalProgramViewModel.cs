using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MMK.SmartSystem.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MMK.SmartSystem.Laser.Base.ProgramOperation.UserControls.ViewModel
{
    public class UpLoadLocalProgramViewModel:ViewModelBase
    {
        private int _SelectedNozzleKindCode;
        public int SelectedNozzleKindCode
        {
            get { return _SelectedNozzleKindCode; }
            set
            {
                if (_SelectedNozzleKindCode != value)
                {
                    _SelectedNozzleKindCode = value;
                    RaisePropertyChanged(() => SelectedNozzleKindCode);
                }
            }
        }

        private ObservableCollection<NozzleKindDto> _NozzleKindList;
        public ObservableCollection<NozzleKindDto> NozzleKindList
        {
            get { return _NozzleKindList; }
            set
            {
                if (_NozzleKindList != value)
                {
                    _NozzleKindList = value;
                    RaisePropertyChanged(() => NozzleKindList);
                }
            }
        }

        public UpLoadLocalProgramViewModel(){

        }
    }
}
