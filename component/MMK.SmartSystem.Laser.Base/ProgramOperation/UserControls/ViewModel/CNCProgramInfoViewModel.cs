using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Laser.Base.ProgramOperation.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Laser.Base.ProgramOperation.UserControls.ViewModel
{
    public class CNCProgramInfoViewModel:ViewModelBase
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
        public CNCProgramInfoViewModel()
        {
            Messenger.Default.Register<CNCProgramPath>(this, (str => {
                this.Path = str.Path;
            }));
        }
    }
}
