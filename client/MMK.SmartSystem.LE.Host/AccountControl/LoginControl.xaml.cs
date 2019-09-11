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
            LoginModel = new LoginControlViewModel()
            {
                Account = "",
                Pwd = "",
                IsError = false,
                IsLogin = false
            };
            this.DataContext = LoginModel;

            Loaded += UserControl_Loaded;
        }  
        
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
            this.maskLayer.SetValue(MaskLayerBehavior.IsOpenProperty, true);
            Loaded -= UserControl_Loaded;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            Login();
        }

        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Login();
            }
        }

        private void Login()
        {
            var msg = string.Empty;
            try
            {
                TokenAuthClient tokenAuthClient = new TokenAuthClient(MMK.SmartSystem.Common.SmartSystemCommonConsts.ApiHost, new System.Net.Http.HttpClient());
                var ts = tokenAuthClient.AuthenticateAsync(new MMK.SmartSystem.Common.AuthenticateModel() { UserNameOrEmailAddress = LoginModel.Account, Password = LoginModel.Pwd }).Result;
                if (ts.Success)
                {
                    Common.SmartSystemCommonConsts.AuthenticateModel = ts.Result;
                    var obj2 = tokenAuthClient.GetUserConfiguraionAsync().Result;
                    if (obj2.Success)
                    {
                        Common.SmartSystemCommonConsts.UserConfiguration = obj2.Result;
                    }
                    msg = "登陆成功";
                    Close();
                }
                else
                {
                    msg = ts.Error.Details;
                }
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
            }
            MessageBox.Show(msg);

            //Messenger.Default.Send(Common.SmartSystemCommonConsts.UserConfiguration);

            Messenger.Default.Send(Common.SmartSystemCommonConsts.AuthenticateModel);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Close()
        {
            this.maskLayer.SetValue(MaskLayerBehavior.IsOpenProperty, false);
        }
    }
}
