using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.LE.Host.ViewModel
{
    public class SystemTranslate
    {
        public SmartSystem SmartSystem { get; set; } = new SmartSystem();

    }

    public class SmartSystem : ViewModelBase
    {
        private string _Account;
        public string Account
        {
            get { return _Account; }
            set
            {
                if (_Account != value)
                {
                    _Account = value;
                    RaisePropertyChanged(() => Account);
                }
            }
        }

        private string _Pwd;
        public string Pwd
        {
            get { return _Pwd; }
            set
            {
                if (_Pwd != value)
                {
                    _Pwd = value;
                    RaisePropertyChanged(() => Pwd);
                }
            }
        }

        private string _Login;
        public string Login
        {
            get { return _Login; }
            set
            {
                if (_Login != value)
                {
                    _Login = value;
                    RaisePropertyChanged(() => Login);
                }
            }
        }

        private string _ChangeAccount;
        public string ChangeAccount
        {
            get { return _ChangeAccount; }
            set
            {
                if (_ChangeAccount != value)
                {
                    _ChangeAccount = value;
                    RaisePropertyChanged(() => ChangeAccount);
                }
            }
        }

        private string _Cancel;
        public string Cancel
        {
            get { return _Cancel; }
            set
            {
                if (_Cancel != value)
                {
                    _Cancel = value;
                    RaisePropertyChanged(() => Cancel);
                }
            }
        }

        private string _LoginSucceed;
        public string LoginSucceed
        {
            get { return _LoginSucceed; }
            set
            {
                if (_LoginSucceed != value)
                {
                    _LoginSucceed = value;
                    RaisePropertyChanged(() => LoginSucceed);
                }
            }
        }

        private string _LoginFailure;
        public string LoginFailure
        {
            get { return _LoginFailure; }
            set
            {
                if (_LoginFailure != value)
                {
                    _LoginFailure = value;
                    RaisePropertyChanged(() => LoginFailure);
                }
            }
        }


        private string _OldPwd;
        public string OldPwd
        {
            get { return _OldPwd; }
            set
            {
                if (_OldPwd != value)
                {
                    _OldPwd = value;
                    RaisePropertyChanged(() => OldPwd);
                }
            }
        }

        private string _Pwd1;
        public string Pwd1
        {
            get { return _Pwd1; }
            set
            {
                if (_Pwd1 != value)
                {
                    _Pwd1 = value;
                    RaisePropertyChanged(() => Pwd1);
                }
            }
        }

        private string _Pwd2;
        public string Pwd2
        {
            get { return _Pwd2; }
            set
            {
                if (_Pwd2 != value)
                {
                    _Pwd2 = value;
                    RaisePropertyChanged(() => Pwd2);
                }
            }
        }

        private string _UpdatePwd;
        public string UpdatePwd
        {
            get { return _UpdatePwd; }
            set
            {
                if (_UpdatePwd != value)
                {
                    _UpdatePwd = value;
                    RaisePropertyChanged(() => UpdatePwd);
                }
            }
        }

        private string _Chinese;
        public string Chinese
        {
            get { return _Chinese; }
            set
            {
                if (_Chinese != value)
                {
                    _Chinese = value;
                    RaisePropertyChanged(() => Chinese);
                }
            }
        }

        private string _English;
        public string English
        {
            get { return _English; }
            set
            {
                if (_English != value)
                {
                    _English = value;
                    RaisePropertyChanged(() => English);
                }
            }
        }
    }
}
