using MMK.SmartSystem.LE.Host.AccountControl.ViewModel;
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

namespace MMK.SmartSystem.LE.Host.AccountControl
{
    /// <summary>
    /// UpdatePasswordControl.xaml 的交互逻辑
    /// </summary>
    public partial class UpdatePasswordControl : UserControl
    {
        public UpdatePasswordControlViewModel updatePasswordViewModel { get; set; }
        public UpdatePasswordControl()
        {
            InitializeComponent();
            this.DataContext = updatePasswordViewModel = new UpdatePasswordControlViewModel();
        }
    }
}
