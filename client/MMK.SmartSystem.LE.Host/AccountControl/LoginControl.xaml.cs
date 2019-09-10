using Abp.Events.Bus;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common.EventDatas;
using MMK.SmartSystem.Common.SerivceProxy;
using MMK.SmartSystem.LE.Host.AccountControl.ViewModel;
using MMK.SmartSystem.LE.Host.SystemControl;
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
        private bool IsClose = false;
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
            if (!IsClose)
            {
                this.maskLayer.SetValue(MaskLayerBehavior.IsOpenProperty, true);
            }
        }

        private void Login(string msg)
        {
            MessageBox.Show(msg);
            Close();
        }

        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LoginModel.LoginCommand.Execute("");
                Close();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Close()
        {
            IsClose = true;
            this.maskLayer.SetValue(MaskLayerBehavior.IsOpenProperty, false);
        }
    }
}
