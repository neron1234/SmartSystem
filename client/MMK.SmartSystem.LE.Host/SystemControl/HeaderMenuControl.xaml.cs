using Abp.Events.Bus;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common.EventDatas;
using MMK.SmartSystem.Common.Model;
using MMK.SmartSystem.LE.Host.AccountControl;
using MMK.SmartSystem.LE.Host.SystemControl.ViewModel;
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

namespace MMK.SmartSystem.LE.Host.SystemControl
{
    /// <summary>
    /// HeaderMenuControl.xaml 的交互逻辑
    /// </summary>
    public partial class HeaderMenuControl : UserControl
    {
        public HeaderMenuControl()
        {
            InitializeComponent();
            Loaded += HeaderMenuControl_Loaded;
        }
        
        public HeaderMenuViewModel headerViewModel { get; set; }
        private void HeaderMenuControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = headerViewModel = new HeaderMenuViewModel();

            Messenger.Default.Register<Common.AuthenticateResultModel> (this, (userConfig) => {
                headerViewModel.IsLogin = true;
                headerViewModel.AccountGroupVisibility = Visibility.Visible;
                headerViewModel.UserAccount = "ID:" + userConfig.UserId;
            });
        }

        private void EnBtn_Click(object sender, RoutedEventArgs e)
        {
            SetCulture("en");
        }

        private void CnBtn_Click(object sender, RoutedEventArgs e)
        {
            SetCulture("zh-Hans");
        }

        private void SetCulture(string language)
        {
            Task.Factory.StartNew(() => EventBus.Default.Trigger(new UserConfigEventData()
            {
                Culture = language
            }));

        }
    }
}
