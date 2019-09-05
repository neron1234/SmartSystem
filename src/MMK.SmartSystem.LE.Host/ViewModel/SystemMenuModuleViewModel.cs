using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MMK.SmartSystem.LE.Host.ViewModel
{
    public class SystemMenuModuleViewModel:ViewModelBase
    {
        public string ModuleName { get; set; } 
        public string Icon { get; set; }
        private List<MainMenuViewModel> mainMenuViews;
        public List<MainMenuViewModel> MainMenuViews
        {
            get { return mainMenuViews; }
            set { mainMenuViews = value; RaisePropertyChanged(() => MainMenuViews); }
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

    }
}
