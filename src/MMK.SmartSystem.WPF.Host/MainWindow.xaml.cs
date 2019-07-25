using Abp.Dependency;
using GalaSoft.MvvmLight.Messaging;
using MMK.CNC.Application.Managements;
using MMK.SmartSystem.WPF.Host.ViewModel;
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

namespace MMK.SmartSystem.WPF.Host
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, ISingletonDependency
    {
        IDepartmentAppService userAppService;
        public List<MainMenuViewModel> mainMenuViews { set; get; }
        IIocManager iocManager;
        public MainWindow(IDepartmentAppService userAppService, IIocManager iocManager)
        {
            this.userAppService = userAppService;
            this.iocManager = iocManager;
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            this.Unloaded += MainWindow_Unloaded;
        }

        private void MainWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Unregister<MainMenuViewModel>(this);
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            mainMenuViews = SmartSystemWPFConsts.SystemMeuns;
            this.DataContext = this;
            Messenger.Default.Register<MainMenuViewModel>(this, Navigation);
            if (mainMenuViews.Count > 0)
            {
                Navigation(mainMenuViews[0]);
            }
            await ApplicationDataTest();

        }

        private void Navigation(MainMenuViewModel model)
        {
            if (model.IsLoad)
            {
                var page = iocManager.Resolve(model.PageType);
                frame.Content = page;

            }

        }

        private async Task ApplicationDataTest()
        {
            try
            {
                await userAppService.Create(new CNC.Application.Managements.Dto.CreateDeppartmentDto()
                {

                    Code = DateTime.Now.ToString(),
                    Icon = "Icon",
                    Level = "parent",
                    Name = "Wpf",
                    ParentId = 0,
                    Sort = 1

                });
                var res = await userAppService.GetAll(new CNC.Application.Managements.Dto.PagedDepartmentResultRequestDto() { SkipCount = 0, MaxResultCount = 10 });

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
