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

namespace MMK.SmartSystem.LE.Host.SystemControl.ViewModel
{
    public class HeaderMenuViewModel : ViewModelBase
    {
        public ICommand OpenCommand
        {
            get
            {
                return new RelayCommand<HeaderMenuViewModel>((s) =>
                {
                    Messenger.Default.Send((UserControl)new AccountControl.LoginControl());
                });
            }
        }
    }
}
