using Abp.Dependency;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common.Embed;
using MMK.SmartSystem.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MMK.SmartSystem.Laser.Base.MachineOperation
{
    /// <summary>
    /// ContainerPage.xaml 的交互逻辑
    /// </summary>
    public partial class ContainerPage : Page,ITransientDependency
    {
        public ContainerPage()
        {
            InitializeComponent();
            Messenger.Default.Send(new PageChangeModel() { Url = "home-zrender", Page = PageEnum.WebComponet });
        }
        private void btnGo_Click(object sender, RoutedEventArgs e)
        {

            var host = AppContainer.FindChild<WindowsFormsHost>(appEmbed);
            Task.Factory.StartNew(new Action(() =>
            {
                Thread.Sleep(2000);
                appEmbed.StartAndEmbedWindowsName("AngualrElectron-Home", host, Dispatcher);

            }));
        }
    }
}
