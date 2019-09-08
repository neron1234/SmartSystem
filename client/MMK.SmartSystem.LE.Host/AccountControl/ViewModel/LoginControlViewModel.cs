using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MMK.SmartSystem.LE.Host.AccountControl.ViewModel
{
    public class LoginControlViewModel:ViewModelBase
    {
        private bool isLogin;
        public bool IsLogin
        {
            get { return isLogin; }
            set
            {
                isLogin = value;
                RaisePropertyChanged(() => IsLogin);
            }
        }

        private bool isError;
        public bool IsError
        {
            get { return isError; }
            set
            {
                isError = value;
                RaisePropertyChanged(() => IsError);
            }
        }

        private string accountError;
        public string AccountError
        {
            get { return accountError; }
            set
            {
                accountError = value;
                RaisePropertyChanged(() => AccountError);
            }
        }

        private string pwdError;
        public string PwdError
        {
            get { return pwdError; }
            set
            {
                pwdError = value;
                RaisePropertyChanged(() => PwdError);
            }
        }

        public string Account { get; set; }
        public string Pwd { get; set; }

        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand<LoginControlViewModel>((s) =>
                {
                    IsLogin = false;
                    Messenger.Default.Send(s);
                });
            }
        }
    }
}
