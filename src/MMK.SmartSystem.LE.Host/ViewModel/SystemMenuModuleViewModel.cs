using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;
namespace MMK.SmartSystem.LE.Host.ViewModel
{
    public class SystemMenuModuleViewModel:ViewModelBase
    {
        public string ModuleName { get; set; } 
        public string Icon { get; set; }
        public ObservableCollection<MainMenuViewModel> MainMenuViews { set; get; } = new ObservableCollection<MainMenuViewModel>();
        

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
    }
}
