using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common.SerivceProxy;
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
            Loaded += UserControl_Loaded;
            Unloaded += LoginControl_Unloaded;
        }

        private void LoginControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Unregister<LoginControlViewModel>(this, Login);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoginModel = new LoginControlViewModel()
            {
                Account = "admin",
                Pwd = "123qwe",
                IsLogin = true
            };
            this.DataContext = this;
            Messenger.Default.Register<LoginControlViewModel>(this, Login);
        }

        private void Login(LoginControlViewModel model)
        {
            TokenAuthClient tokenAuthClient = new TokenAuthClient(MMK.SmartSystem.Common.SmartSystemCommonConsts.ApiHost, new System.Net.Http.HttpClient());
            var ts = tokenAuthClient.AuthenticateAsync(new MMK.SmartSystem.Common.AuthenticateModel() { UserNameOrEmailAddress = LoginModel.Account, Password = LoginModel.Pwd }).Result;
            if (ts.Success)
            {
                Common.SmartSystemCommonConsts.AuthenticateModel = ts.Result;
                MessageBox.Show("登陆成功");
            }
            else
            {
                MessageBox.Show(ts.Error.Details);
            }
            LoginModel.IsLogin = true;
        }
    }
}
