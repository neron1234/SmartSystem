using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common.Model;
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
            Loaded += UserInfoControl_Loaded;
        }

        private void UserInfoControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = userViewModel = new UserInfoControlViewModel();
            Messenger.Default.Register<UserInfo>(this, (u) => {
                userViewModel.Account = "Account:" + u.UserName;
                userViewModel.Email = "Email:" + u.EmailAddress;
                userViewModel.Id = "Id:" + u.Id;
                userViewModel.CreateTime = "CreateTime:" + u.CreationTime;
            });
        }

        private void UpdatePwdBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
