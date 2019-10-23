using Abp.Dependency;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common.ViewModel;
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
    /// MainHomeControl.xaml 的交互逻辑
    /// </summary>
    public partial class MainHomeControl : UserControl
    {
        private MainWindowViewModel MainViewModel = new MainWindowViewModel();
        private Notifiaction notifiaction = new Notifiaction();
        private IIocManager iocManager;

        public MainHomeControl()
        {
            InitializeComponent();
            this.DataContext = MainViewModel;

        }


        public void InitMessenger(IIocManager _iocManager)
        {
            iocManager = _iocManager;         

            Messenger.Default.Register<UserControl>(this, (control) =>
            {
                MainViewModel.PopupControl = control;
            });

            Messenger.Default.Register<Common.AuthenticateResultModel>(this, (userConfig) =>
            {
                MainViewModel.MainFrame = null;
            });

            //消息通知
            Messenger.Default.Register<NotifiactionModel>(this, (nm) =>
            {
            });
        }
        public void ChangeWPFPage(PageChangeModel pageChange)
        {

            if (pageChange.Page == PageEnum.WPFPage)
            {
                try
                {
                    var page = iocManager.Resolve(pageChange.FullType);
                    MainViewModel.MainFrame = page;

                }
                catch (Exception ex)
                {
                    notifiaction.AddNotifiaction(new NotifiactionModel() { Content = ex.Message, Title = ex.Source });

                }

            }
        }

    }
}
