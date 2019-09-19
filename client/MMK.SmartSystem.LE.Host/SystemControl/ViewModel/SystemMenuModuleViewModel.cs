using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MMK.SmartSystem.LE.Host.SystemControl.ViewModel
{
    public class SystemMenuModuleViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string moduleName;

        public string ModuleName
        {
            get { return moduleName; }
            set
            {
                if (moduleName != value)
                {
                    moduleName = value;
                    RaisePropertyChanged(() => ModuleName);
                }
            }
        }

        public string ModuleKey { get; set; }

        private Visibility _Show;
        public Visibility Show
        {
            get { return _Show; }
            set
            {
                if (_Show != value)
                {
                    _Show = value;
                    RaisePropertyChanged(() => Show);
                }
            }
        }

        public string Icon { get; set; }
        public string BackColor { get; set; }
        public int Sort { get; set; }

        private ObservableCollection<MainMenuViewModel> mainMenuViews;
        public ObservableCollection<MainMenuViewModel> MainMenuViews
        {
            get { return mainMenuViews; }
            set
            {
                mainMenuViews = value;
                RaisePropertyChanged(() => MainMenuViews);
            }
        }

        public ICommand OpenCommand
        {
            get
            {
                return new RelayCommand<SystemMenuModuleViewModel>((s) =>
                {
                    Messenger.Default.Send(s);
                });
            }
        }

        public SystemMenuModuleViewModel()
        {
            _Show = Visibility.Collapsed;
        }

        //protected internal virtual void OnPropertyChanged(string propertyName)
        //{
        //    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //}
        //public event PropertyChangedEventHandler PropertyChanged;
    }
}
