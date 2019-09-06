using Abp.Events.Bus;
using MMK.SmartSystem.Common.EventDatas;
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
        }

        private async void ChangeUserBtn_Click(object sender, RoutedEventArgs e)
        {
            await EventBus.Default.TriggerAsync(new UserConfigEventData()
            {
                UserName = SmartSystemLEConsts.DefaultUser,
                Pwd = SmartSystemLEConsts.DefaultPwd,
                Culture = SmartSystemLEConsts.Culture,
                IsChangeUser = true
            });
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {

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
