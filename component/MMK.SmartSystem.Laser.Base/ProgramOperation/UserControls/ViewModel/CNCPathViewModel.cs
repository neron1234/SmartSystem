using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MMK.SmartSystem.Laser.Base.ProgramOperation.UserControls.ViewModel
{
    public class CNCPathViewModel:ViewModelBase
    {
        private ReadProgramFolderItemViewModel _ProgramFolders;
        public ReadProgramFolderItemViewModel ProgramFolders
        {
            get { return _ProgramFolders; }
            set
            {
                if (_ProgramFolders != value)
                {
                    _ProgramFolders = value;
                    RaisePropertyChanged(() => ProgramFolders);
                }
            }
        }

        private ReadProgramFolderItemViewModel _SelectedProgramFolders;
        public ReadProgramFolderItemViewModel SelectedProgramFolders
        {
            get { return _SelectedProgramFolders; }
            set
            {
                if (_SelectedProgramFolders != value)
                {
                    _SelectedProgramFolders = value;
                    RaisePropertyChanged(() => SelectedProgramFolders);
                }
            }
        }

        public CNCPathViewModel(ReadProgramFolderItemViewModel programFolderInfo){
            ProgramFolders = programFolderInfo;
            SelectedProgramFolders = programFolderInfo;
        }

        public ICommand SaveCNCPathCommand
        {
            get
            {
                return new RelayCommand(() => {
                    Messenger.Default.Send(new PopupMsg(SelectedProgramFolders.Folder, false));
                });
            }
        }

        public ICommand CancelCommand{
            get
            {
                return new RelayCommand(() => {
                    Messenger.Default.Send(new PopupMsg("", true));
                });
            }
        }
    }
}
