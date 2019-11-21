using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MMK.SmartSystem.Laser.Base.ProgramOperation.UserControls.ViewModel
{
    public class EditProgramStrViewModel:ViewModelBase
    {
        private string _ProgramStr;
        public string ProgramStr
        {
            get { return _ProgramStr; }
            set
            {
                if (_ProgramStr != value)
                {
                    _ProgramStr = value;
                    RaisePropertyChanged(() => ProgramStr);
                }
            }
        }
        public string Url { get; set; }

        public event Action CloseEvent;
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
    }
}
