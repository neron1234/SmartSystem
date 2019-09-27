using Abp.Dependency;
using MMK.SmartSystem.Common.ViewModel;
using MMK.SmartSystem.Laser.Base.MachineOperation.ViewModel;
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
using System.Reflection;
using MMK.SmartSystem.Common.Base;

namespace MMK.SmartSystem.Laser.Base.MachineOperation
{
    /// <summary>
    /// AutoFindSidePage.xaml 的交互逻辑
    /// </summary>
    public partial class AutoFindSidePage : AutoRefreshPage
    {
        public AutoFindSidePageViewModel AutoFindSidePageViewModel { get; set; }
        /// <summary>
        /// 自动寻边
        /// </summary>
        public AutoFindSidePage()
        {
            InitializeComponent();
            this.DataContext = AutoFindSidePageViewModel = new AutoFindSidePageViewModel("MachineOperation.AutoFindSidePage");
            this.Unloaded += AutoFindSidePage_Unloaded;
            this.RefreshAuth += AutoFindSidePage_RefreshAuth;
        }

        private void AutoFindSidePage_RefreshAuth()
        {
            AutoFindSidePageViewModel.RefreshAuth();
        }

        private void AutoFindSidePage_Unloaded(object sender, RoutedEventArgs e)
        {
            this.RefreshAuth -= AutoFindSidePage_RefreshAuth;
            ClearRegister();
        }
    }
}
