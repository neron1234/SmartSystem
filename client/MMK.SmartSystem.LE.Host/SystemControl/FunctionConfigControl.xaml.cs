using MMK.SmartSystem.LE.Host.SystemControl.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// FunctionConfigControl.xaml 的交互逻辑
    /// </summary>
    public partial class FunctionConfigControl : UserControl
    {
        public FunctionConfigViewModel FunctionConfigViewModel { get; set; }

        public FunctionConfigControl()
        {
            InitializeComponent();
            this.DataContext = FunctionConfigViewModel = new FunctionConfigViewModel();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            var name = ((Button)sender).Tag.ToString();

            SmartSystemLEConsts.SystemModules.FirstOrDefault(n => n.ModuleName == name).MainMenuViews.Add(new MainMenuViewModel
            {
                Show = Visibility.Visible,
                BackColor = SmartSystemLEConsts.SystemModules.FirstOrDefault(n => n.ModuleName == name).BackColor,
                Title = "Test",
                IsLoad = true
            });
            FunctionConfigViewModel.SysModuleViews = SmartSystemLEConsts.SystemModules;
        }

        private void UpMoveBtn_Click(object sender, RoutedEventArgs e)
        {
            var name = ((Button)sender).Tag.ToString();
            //SmartSystemLEConsts.SystemModules.OrderBy(n => n.Sort)
        }
    }
}
