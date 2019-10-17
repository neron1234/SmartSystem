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
        private int _SelectedMaterialId;
        public int SelectedMaterialId
        {
            get { return _SelectedMaterialId; }
            set
            {
                if (_SelectedMaterialId != value)
                {
                    _SelectedMaterialId = value;
                    RaisePropertyChanged(() => SelectedMaterialId);
                }
            }
        }

        private ObservableCollection<MaterialDto> _MaterialTypeList;
        public ObservableCollection<MaterialDto> MaterialTypeList
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

        private string _LocalProgramPath;
        public string LocalProgramPath
        {
            get { return _LocalProgramPath; }
            set
            {
                if (_LocalProgramPath != value)
                {
                    _LocalProgramPath = value;
                    RaisePropertyChanged(() => LocalProgramPath);
                }
            }
        }

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
