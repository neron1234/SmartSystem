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
    public class SystemMenuModuleViewModel : ViewModelBase
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
            get
            {

                return MainMenuViews?.Count() > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
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

        public ObservableCollection<MainMenuViewModel> MainMenuViews { set; get; }

        public void MenuItemClick(MainMenuViewModel item)
        {
            MainMenuViews.ToList().ForEach(d=>d.MenuClearActive());
            item.MenuActive();
            
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

    }
}
