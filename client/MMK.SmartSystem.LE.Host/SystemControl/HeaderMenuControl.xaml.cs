using Abp.Events.Bus;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common;
using MMK.SmartSystem.Common.EventDatas;
using MMK.SmartSystem.Common.Model;
using MMK.SmartSystem.Common.ViewModel;
using MMK.SmartSystem.LE.Host.AccountControl;
using MMK.SmartSystem.LE.Host.CustomControl;
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

namespace MMK.SmartSystem.LE.Host.SystemControl
{
    /// <summary>
    /// HeaderMenuControl.xaml 的交互逻辑
    /// </summary>
    public partial class HeaderMenuControl : UserControl
    {
        public HeaderMenuViewModel headerViewModel { get; set; }
        public HeaderMenuControl()
        {
            InitializeComponent();
            this.DataContext = headerViewModel = new HeaderMenuViewModel();
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
            Task.Factory.StartNew(() => EventBus.Default.Trigger(new UserLanguageEventData()
            {
                Culture = language,
                HashCode = this.GetHashCode(),
                Tagret = ErrorTagretEnum.UserControl
            }));
        }

        private void CnHomeBtn_Click(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send(new PageChangeModel() { Url = "/", Page = PageEnum.WebPage });
        
        }

        private void CnHomeBtn2_Click(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send(new PageChangeModel() { Url = "/home", Page = PageEnum.WebPage });

          

        }

        private void FunctionConfigBtn_Click(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send((UserControl)new PopupWindowControl(new FunctionConfigControl(),900,600));
        }

        private void FunctionConfig2Btn_Click(object sender, RoutedEventArgs e)
        {
            //Messenger.Default.Send(new BottomWarningLogViewModel()
            //{
            //    WarningLogStr = "测试出现了BUG！------" + DateTime.Now
            //}); 

            Messenger.Default.Send(new NotifiactionModel()
            {
                Title = "异常通知",
                Content = "测试出现了异常 ----- " + DateTime.Now,
                NotifiactionType = EnumPromptType.Error
            });
        }
    }
}
