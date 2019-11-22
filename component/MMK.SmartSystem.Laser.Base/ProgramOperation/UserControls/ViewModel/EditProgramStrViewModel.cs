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

        private string _SearchText;
        public string SearchText
        {
            get { return _SearchText; }
            set
            {
                if (_SearchText != value)
                {
                    _SearchText = value;
                    RaisePropertyChanged(() => SearchText);
                }
            }
        }

        private string _ReplaceText;
        public string ReplaceText
        {
            get { return _ReplaceText; }
            set
            {
                if (_ReplaceText != value)
                {
                    _ReplaceText = value;
                    RaisePropertyChanged(() => ReplaceText);
                }
            }
        }

        public string FillName { get; set; }

        public event Action CloseEvent;
        public event Action SaveEvent;

        public event Action LastSearchEvent;
        public event Action NextSearchEvent;
        public event Action ReplaceEvent;
        public event Action ReplaceAllEvent;

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

        public ICommand LastSearchCommand
        {
            get
            {
                return new RelayCommand(() => {
                    LastSearchEvent.Invoke();
                });
            }
        }
        public ICommand NextSearchCommand
        {
            get
            {
                return new RelayCommand(() => {
                    NextSearchEvent.Invoke();
                });
            }
        }
        public ICommand ReplaceCommand
        {
            get
            {
                return new RelayCommand(() => {
                    ReplaceEvent.Invoke();
                });
            }
        }
        public ICommand ReplaceAllCommand
        {
            get
            {
                return new RelayCommand(() => {
                    ReplaceAllEvent.Invoke();
                });
            }
        }
    }

    public class ProgramStr:ViewModelBase
    {
        public string Str { get; set; }
    }
}
