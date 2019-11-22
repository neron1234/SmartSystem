using Abp.Dependency;
using Abp.Events.Bus;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common.Base;
using MMK.SmartSystem.Common.EventDatas;
using MMK.SmartSystem.Common.Model;
using MMK.SmartSystem.Common.SignalrProxy;
using MMK.SmartSystem.Common.ViewModel;
using MMK.SmartSystem.LE.Host.CustomControl;
using MMK.SmartSystem.LE.Host.SystemControl.ViewModel;
using MMK.SmartSystem.LE.Host.ViewModel;
using MMK.SmartSystem.WebCommon.DeviceModel;
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

namespace MMK.SmartSystem.LE.Host.MainHome
{
    /// <summary>
    /// MainPage.xaml 的交互逻辑
    /// </summary>
    public partial class MainPage : SignalrPage
    {
        public override string GetModule => "MainHome";

        private MainWindowViewModel MainViewModel = new MainWindowViewModel();
        private Notifiaction notifiaction = new Notifiaction();
        SignalrRouteProxyClient signalrRouteProxyClient;

        public event Action<Visibility, Visibility> WebOrHostDisplayEvent;
        private IIocManager iocManager;

        public MainPage(IIocManager iocManager)
        {
            InitializeComponent();
            this.iocManager = iocManager;
            DataContext = MainViewModel;
            signalrRouteProxyClient = new SignalrRouteProxyClient();
            signalrRouteProxyClient.RouteErrorEvent += SignalrRouteProxyClient_RouteErrorEvent;
            signalrRouteProxyClient.GetHomeEvent += SignalrRouteProxyClient_GetHomeEvent;
            InitMessenger();
        }
        protected override void PageSignlarLoaded()
        {
            Task.Factory.StartNew(async () => await signalrRouteProxyClient.Start());

        }
        protected async override void SignalrPage_Unloaded(object sender, RoutedEventArgs e)
        {
            base.SignalrPage_Unloaded(sender, e);
            await signalrRouteProxyClient.Close();

        }
        private void SignalrRouteProxyClient_GetHomeEvent(string obj)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                WebOrHostDisplayEvent?.Invoke(Visibility.Collapsed, Visibility.Visible);

                if (mainFrame.Content == null)
                {
                    SmartSystemLEConsts.SystemModules.First()?.MainMenuViews.First()?.OpenCommand.Execute(SmartSystemLEConsts.SystemModules.First()?.MainMenuViews.First());
                }
            }));
        }

        private void SignalrRouteProxyClient_RouteErrorEvent(string obj)
        {

        }

        private void InitMessenger()
        {
            Messenger.Default.Register<PageChangeModel>(this, pageChange);


            //消息通知
            Messenger.Default.Register<NotifiactionModel>(this, (nm) =>
            {
                notifiaction.AddNotifiaction(nm);
            });

            Messenger.Default.Register<WaringMsgPopup>(this, (pv) =>
            {
                new PopupWindow().ShowDialog();
            });
        }

        private void pageChange(PageChangeModel changeModel)
        {
            if (changeModel.Page == PageEnum.WPFPage)
            {
                WebOrHostDisplayEvent?.Invoke(Visibility.Hidden, Visibility.Visible);

                ResolveWPFPage(changeModel);
                return;
            }
            if (changeModel.Page == PageEnum.WebPage)
            {
                WebOrHostDisplayEvent?.Invoke(Visibility.Visible, Visibility.Hidden);
                Task.Factory.StartNew(() => EventBus.Default.Trigger(new NavigateEventData()
                {
                    Url = changeModel.Url,
                    NavigateType = NavigateEnum.Url
                }));
                return;
            }
            if (changeModel.Page == PageEnum.WebComponet)
            {
                Task.Factory.StartNew(() => EventBus.Default.Trigger(new NavigateEventData()
                {
                    NavigateType = NavigateEnum.Component,
                    ComponentDto = changeModel.ComponentDto
                }));
            }
        }

        private void ResolveWPFPage(PageChangeModel pageChange)
        {
            if (pageChange.Page == PageEnum.WPFPage && pageChange.FullType != null)
            {
                try
                {
                    headTitle.UpdateTitle(pageChange.Title);
                    var page = iocManager.Resolve(pageChange.FullType);
                    MainViewModel.MainFrame = page;

                }
                catch (Exception ex)
                {
                    notifiaction.AddNotifiaction(new NotifiactionModel() { Content = ex.Message, Title = ex.Source });

                }
            }
        }
        public override List<object> GetResultViewModelMap()
        {
            return new List<object>()
            {
                new SingalrResultMapModel<ReadPmcResultItemModel>()
                {
                    ViewModels=headerStatusControl.headerVM,
                    MapType=SignalrMapModelEnum.AutoPropMap,
                    AutoPropMapAction=(n,s)=>n.FirstOrDefault(d=>d.Id.Equals(s,StringComparison.OrdinalIgnoreCase))?.Value
                },
                new SingalrResultMapModel<ReadProgramNameResultModel>()
                {
                     ViewModels=new HeaderTitleProgram(),
                     MapType=SignalrMapModelEnum.CustomAction,
                     MapAction=(d)=>headTitle.SetProgram(d.FirstOrDefault()?.Value.Name)
                },
                new SingalrResultMapModel<ReadAlarmResultModel>()
                {
                    ViewModels=new HeaderTitleAlarm(),
                    MapType=SignalrMapModelEnum.CustomAction,
                    MapAction=(d)=>headTitle.SetAlarm(d)
                }
            };
        }

        public override void CncOnError(string message)
        {

        }
    }
}
