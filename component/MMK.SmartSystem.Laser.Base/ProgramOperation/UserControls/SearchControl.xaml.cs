using MMK.SmartSystem.Laser.Base.ProgramOperation.UserControls.ViewModel;
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

namespace MMK.SmartSystem.Laser.Base.ProgramOperation.UserControls
{
    /// <summary>
    /// SearchControl.xaml 的交互逻辑
    /// </summary>
    public partial class SearchControl : UserControl
    {
        public SearchViewModel sVM { get; set; }
        public SearchControl()
        {
            InitializeComponent();
            this.DataContext = sVM = new SearchViewModel();
        }
    }
}
