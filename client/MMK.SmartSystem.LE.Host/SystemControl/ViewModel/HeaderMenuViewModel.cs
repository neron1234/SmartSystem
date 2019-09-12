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
                        LoginBtnTxt = "切换账号";
                    }
                    else
                    {
                        LoginBtnTxt = "登陆";
                    }
                    RaisePropertyChanged(() => IsLogin);
                }
            }
        }

        private string _LoginBtnTxt;
        public string LoginBtnTxt
        {
            get { return _LoginBtnTxt; }
            set
            {
                if (_LoginBtnTxt != value)
                {
                    _LoginBtnTxt = value;
                    RaisePropertyChanged(() => LoginBtnTxt);
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

        private Visibility _AccountGroupVisibility;
        public Visibility AccountGroupVisibility
        {
            get { return _AccountGroupVisibility; }
            set
            {
                if (_AccountGroupVisibility != value)
                {
                    _AccountGroupVisibility = value;
                    RaisePropertyChanged(() => AccountGroupVisibility);
                }
            }
        }

        public HeaderMenuViewModel()
        {
            IsLogin = false;
            AccountGroupVisibility = Visibility.Collapsed;
            LoginBtnTxt = "登陆";
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
                        AccountGroupVisibility = Visibility.Collapsed;
                        IsLogin = false;
                    }
                });
            }
        }
    }
}
