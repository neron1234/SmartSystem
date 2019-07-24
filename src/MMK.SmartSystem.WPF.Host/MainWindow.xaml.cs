using Abp.Dependency;
using MMK.CNC.Application.Managements;
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
        public MainWindow(IDepartmentAppService userAppService)
        {
            this.userAppService = userAppService;

            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
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
