using Abp.Events.Bus;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common.EventDatas;
using MMK.SmartSystem.Common.SerivceProxy;
using MMK.SmartSystem.LE.Host.AccountControl.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MMK.SmartSystem.LE.Host.AccountControl
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class LoginControl : UserControl
    {
        public LoginControlViewModel LoginModel { get; set; }
        public LoginControl()
        {
            InitializeComponent();
            Loaded += UserControl_Loaded;
            Unloaded += LoginControl_Unloaded;
        }

        private void LoginControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Unregister<string>(this, Login);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoginModel = new LoginControlViewModel()
            {
                Account = "",
                Pwd = "",
                IsLogin = true,
                IsError = false
            };
            this.DataContext = LoginModel;
            Messenger.Default.Register<string>(this, Login);
        }

        private void Login(string msg)
        {
            MessageBox.Show(msg);
        }
    }
}
