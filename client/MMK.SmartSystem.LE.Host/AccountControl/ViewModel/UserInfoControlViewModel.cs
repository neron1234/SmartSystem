using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.LE.Host.SystemControl.ViewModel;
using MMK.SmartSystem.LE.Host.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MMK.SmartSystem.Common.ViewModel;

namespace MMK.SmartSystem.LE.Host.AccountControl.ViewModel
{
    public class UserInfoControlViewModel:MainTranslateViewModel
    {
        public UserInfoControlViewModel()
        {
            IsLogin = false;
            LoginBtnVisibility = Visibility.Visible;
            ChangeAccountBtnVisibility = Visibility.Collapsed;
            UpdateBtnVisibility = Visibility.Collapsed;
        }

        private bool _IsLogin;
        public bool IsLogin
        {
            get { return _IsLogin; }
            set
            {
                if (_IsLogin != value)
                {
                    _IsLogin = value;
                    if (_IsLogin)
                    {
                        LoginBtnVisibility = Visibility.Collapsed;
                        ChangeAccountBtnVisibility = Visibility.Visible;
                        UpdateBtnVisibility = Visibility.Visible;
                    }
                    else
                    {
                        LoginBtnVisibility = Visibility.Visible;
                        ChangeAccountBtnVisibility = Visibility.Collapsed;
                        UpdateBtnVisibility = Visibility.Collapsed;
                    }
                    RaisePropertyChanged(() => IsLogin);
                }
            }
        }

        private Visibility _LoginBtnVisibility;
        public Visibility LoginBtnVisibility
        {
            get { return _LoginBtnVisibility; }
            set
            {
                if (_LoginBtnVisibility != value)
                {
                    _LoginBtnVisibility = value;
                    RaisePropertyChanged(() => LoginBtnVisibility);
                }
            }
        }

        private Visibility _ChangeAccountBtnVisibility;
        public Visibility ChangeAccountBtnVisibility
        {
            get { return _ChangeAccountBtnVisibility; }
            set
            {
                if (_ChangeAccountBtnVisibility != value)
                {
                    _ChangeAccountBtnVisibility = value;
                    RaisePropertyChanged(() => ChangeAccountBtnVisibility);
                }
            }
        }

        private Visibility _UpdateBtnVisibility;
        public Visibility UpdateBtnVisibility
        {
            get { return _UpdateBtnVisibility; }
            set
            {
                if (_UpdateBtnVisibility != value)
                {
                    _UpdateBtnVisibility = value;
                    RaisePropertyChanged(() => UpdateBtnVisibility);
                }
            }
        }


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

        private string _Id;
        public string Id
        {
            get { return _Id; }
            set
            {
                if (_Id != value)
                {
                    _Id = value;
                    RaisePropertyChanged(() => Id);
                }
            }
        }

        private string _Email;
        public string Email
        {
            get { return _Email; }
            set
            {
                if (_Email != value)
                {
                    _Email = value;
                    RaisePropertyChanged(() => Email);
                }
            }
        }

        private string _CreateTime;
        public string CreateTime
        {
            get { return _CreateTime; }
            set
            {
                if (_CreateTime != value)
                {
                    _CreateTime = value;
                    RaisePropertyChanged(() => CreateTime);
                }
            }
        }

        public event Action ChangeUserEvent;
        public ICommand OpenCommand
        {
            get
            {
                return new RelayCommand<HeaderMenuViewModel>((s) =>
                {
                    if (!IsLogin)
                    {
                        Messenger.Default.Send((UserControl)new LoginControl());
                    }
                    else
                    {
                        ChangeUserEvent?.Invoke();
                        IsLogin = false;
                    }
                });
            }
        }
    }
}
