using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using Abp.Events.Bus;
using MMK.SmartSystem.Common.EventDatas;
using MMK.SmartSystem.LE.Host.ViewModel;

namespace MMK.SmartSystem.LE.Host.SystemControl.ViewModel
{
    public class HeaderMenuViewModel : MainTranslateViewModel
    {
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
                    }
                    else
                    {
                        LoginBtnVisibility = Visibility.Visible;
                        ChangeAccountBtnVisibility = Visibility.Collapsed;
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

        private string _UserAccount;
        public string UserAccount
        {
            get { return _UserAccount; }
            set
            {
                if (_UserAccount != value)
                {
                    _UserAccount = value;
                    RaisePropertyChanged(() => UserAccount);
                }
            }
        }

        public HeaderMenuViewModel()
        {
            IsLogin = false;
            LoginBtnVisibility = Visibility.Visible;
            ChangeAccountBtnVisibility = Visibility.Collapsed;
        }

        public ICommand OpenCommand
        {
            get
            {
                return new RelayCommand<HeaderMenuViewModel>((s) =>
                {
                    if (!IsLogin)
                    {
                        Messenger.Default.Send((UserControl)new AccountControl.LoginControl());
                    }
                    else
                    {
                        EventBus.Default.TriggerAsync(new UserConfigEventData()
                        {
                            UserName = SmartSystemLEConsts.DefaultUser,
                            Pwd = SmartSystemLEConsts.DefaultPwd,
                            Culture = SmartSystemLEConsts.Culture,
                            IsChangeUser = true
                        });
                        IsLogin = false;
                    }
                });
            }
        }
    }
}
