using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Laser.Base.ProgramOperation.UserControls;
using MMK.SmartSystem.Laser.Base.ProgramOperation.UserControls.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace MMK.SmartSystem.Laser.Base.ProgramOperation.ViewModel
{
    public class ProgramListViewModel:ViewModelBase
    {
        private ProgramViewModel _SelectedProgram;
        public ProgramViewModel SelectedProgram
        {
            get { return _SelectedProgram; }
            set
            {
                if (_SelectedProgram != value)
                {
                    _SelectedProgram = value;
                    RaisePropertyChanged(() => SelectedProgram);
                }
            }
        }

        private UserControl _ControlInfo;
        public UserControl ControlInfo
        {
            get { return _ControlInfo; }
            set
            {
                if (_ControlInfo != value)
                {
                    _ControlInfo = value;
                    RaisePropertyChanged(() => ControlInfo);
                }
            }
        }

        private string _Path;
        public string Path
        {
            get { return _Path; }
            set
            {
                if (_Path != value)
                {
                    _Path = value;
                    RaisePropertyChanged(() => Path);
                }
            }
        }

        public string ConnectId { get; set; }
        public ProgramListViewModel()
        {
            this.ControlInfo = new CNCProgramListControl();
            Messenger.Default.Register<ProgramPath>(this, (str => {
                this.Path = str.Path;
            }));
        }

        public ICommand LoadFileCommand{
            get{
                return new RelayCommand(() => {
                    
                });
            }
        }

        public ICommand CNCListCommand{
            get{
                return new RelayCommand(() => {
                    this.ControlInfo = new CNCProgramListControl();
                });
            }
        }

        public ICommand LocalListCommand{
            get{
                return new RelayCommand(() => {
                    this.ControlInfo = new LocalProgramListControl(this.ConnectId);
                });
            }
        }

        public ICommand CNCInfoCommand{
            get{
                return new RelayCommand(() => {
                    this.ControlInfo = new CNCInfoControl();
                });
            }
        }

    }
}
