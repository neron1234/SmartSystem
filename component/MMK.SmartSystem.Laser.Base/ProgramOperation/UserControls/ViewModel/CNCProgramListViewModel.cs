using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MMK.SmartSystem.Laser.Base.ProgramOperation.UserControls.ViewModel
{
    public class CNCProgramListViewModel:ViewModelBase{
        private ObservableCollection<ProgramViewModel> _ProgramList;
        public ObservableCollection<ProgramViewModel> ProgramList{
            get { return _ProgramList; }
            set{
                if (_ProgramList != value){
                    _ProgramList = value;
                    RaisePropertyChanged(() => ProgramList);
                }
            }
        }

        private string _CNCPath;
        public string CNCPath
        {
            get { return _CNCPath; }
            set
            {
                if (_CNCPath != value)
                {
                    _CNCPath = value;
                    RaisePropertyChanged(() => CNCPath);
                }
            }
        }

        public ReadProgramFolderItemViewModel ProgramFolderList { get; set; }

        public ICommand CNCPathCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    new PopupWindow(new CNCPathControl(this.ProgramFolderList), 680, 240, "修改CNC路径").ShowDialog();
                });
            }
        }
    }
}
