using Abp.Events.Bus;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common;
using MMK.SmartSystem.Common.EventDatas;
using MMK.SmartSystem.Common.Model;
using MMK.SmartSystem.Common.ViewModel;
using MMK.SmartSystem.LE.Host.AccountControl.ViewModel;
using MMK.SmartSystem.LE.Host.SystemControl;
using MMK.SmartSystem.LE.Host.SystemControl.ViewModel;
using MMK.SmartSystem.LE.Host.ViewModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MMK.SmartSystem.LE.Host.AccountControl
{
    /// <summary>
    /// UserInfoControl.xaml 的交互逻辑
    /// </summary>
    public partial class UserInfoControl : UserControl
    {
        public UserInfoControlViewModel userViewModel { get; set; }
        public UserInfoControl()
        {
            InitializeComponent();
            this.DataContext = userViewModel = new UserInfoControlViewModel();
           
            Messenger.Default.Register<UserInfo>(this, (u) => {
                userViewModel.Account = $"{userViewModel.Translate.SmartSystem.Account}:{u.UserName}";
                userViewModel.Email = $"Email:{u.EmailAddress}";
                userViewModel.Id = $"Id:{u.Id}";
                userViewModel.CreateTime = $"CreateTime:{u.CreationTime}";
                userViewModel.IsLogin = true;
            });

            userViewModel.ChangeUserEvent += UserViewModel_ChangeUserEvent;

 
        }

        private void UserViewModel_ChangeUserEvent(){
            //EventBus.Default.TriggerAsync(new UserLoginEventData(){
            //    UserName = SmartSystemLEConsts.DefaultUser,
            //    Pwd = SmartSystemLEConsts.DefaultPwd,
            //    Tagret = ErrorTagretEnum.Window,
            //    HashCode = this.GetHashCode(),
            //    SuccessAction = LoginSuccess
            //});
            WebRouteClient webRouteClient = new WebRouteClient(SmartSystemCommonConsts.ApiHost, new System.Net.Http.HttpClient());
            webRouteClient.NavigateAsync("/");
            Messenger.Default.Send(new PageChangeModel() { Page = PageEnum.WebPage });
        }

        public void LoginSuccess()
        {
           // EventBus.Default.Trigger(new UserInfoEventData() { UserId = (int)SmartSystemCommonConsts.AuthenticateModel.UserId, Tagret = ErrorTagretEnum.UserControl });
        }

        private void UpdatePwdBtn_Click(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send((UserControl)new PopupWindowControl(new UpdatePasswordControl()));
        }
    }
}
