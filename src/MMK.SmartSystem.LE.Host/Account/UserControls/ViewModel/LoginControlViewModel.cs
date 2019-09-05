using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MMK.SmartSystem.LE.Host.Account.UserControls.ViewModel
{
    public class LoginControlViewModel:ViewModelBase
    {
        
        public bool IsLogin { get; set; }

        public string Account { get; set; }
        public string Pwd { get; set; }

        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand<LoginControlViewModel>((s) =>
                {
                    Messenger.Default.Send(s);
                });
            }
        }
    }
}
