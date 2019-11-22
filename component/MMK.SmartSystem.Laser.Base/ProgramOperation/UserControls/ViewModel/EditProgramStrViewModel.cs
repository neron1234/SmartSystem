using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Laser.Base.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MMK.SmartSystem.Laser.Base.ProgramOperation.UserControls.ViewModel
{
    public class EditProgramStrViewModel:ViewModelBase
    {
        public EditProgramStrViewModel()
        {
            ProgramStr = "";
        }

        public string ProgramStr { get;set; }

        public string FillName { get; set; }

        public event Action CloseEvent;
        public event Action SaveEvent;
        //public event Action GetNewProgramStr;
        public ICommand CancelCommand
        {
            get
            {
                return new RelayCommand<string>((str) =>
                {
                    CloseEvent?.Invoke();
                });
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                return new RelayCommand(() => {
                    SaveEvent?.Invoke();
                });
            }
        }
    }

    public class ProgramStr:ViewModelBase
    {
        public string Str { get; set; }
    }
}
