using Abp.Events.Bus;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common;
using MMK.SmartSystem.Common.EventDatas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace MMK.SmartSystem.LE.Host.ViewModel
{
    public class MainWindowViewModel:ViewModelBase
    {
        private object _mainFrame;
        public object MainFrame
        {
            get { return _mainFrame; }
            set
            {
                if (_mainFrame != value)
                {
                    _mainFrame = value;
                    RaisePropertyChanged(() => MainFrame);
                }
            }
        }

        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    MainFrame = new AccountControl.LoginControl();
                });
            }
        }

        public ICommand CnCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    SmartSystemCommonConsts.CurrentCulture = "zh-Hans";
                    EventBus.Default.Trigger(new UserConfigEventData()
                    {
                        Culture = SmartSystemCommonConsts.CurrentCulture
                    });
                });
            }
        }

        public ICommand EnCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    SmartSystemCommonConsts.CurrentCulture = "en-Hans";
                    EventBus.Default.Trigger(new UserConfigEventData()
                    {
                        Culture = SmartSystemCommonConsts.CurrentCulture
                    });
                });
            }
        }

        public ICommand ChangeUserCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    EventBus.Default.Trigger(new UserConfigEventData()
                    {
                        UserName = SmartSystemLEConsts.DefaultUser,
                        Pwd = SmartSystemLEConsts.DefaultPwd,
                        Culture = SmartSystemLEConsts.Culture,
                        IsChangeUser = true
                    });
                });
            }
        }
    }
}
