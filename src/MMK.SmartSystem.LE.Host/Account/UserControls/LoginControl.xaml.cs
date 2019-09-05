using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.LE.Host.Account.UserControls.ViewModel;
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

namespace MMK.SmartSystem.LE.Host.Account.UserControls
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
            LoginModel = new LoginControlViewModel();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = this;
            Messenger.Default.Register<LoginControlViewModel>(this, Login);
        }

        private void Login(LoginControlViewModel loginModel)
        {
            MessageBox.Show(LoginModel.Account);
        }
    }
}
