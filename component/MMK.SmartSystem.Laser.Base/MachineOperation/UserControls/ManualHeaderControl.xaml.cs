using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Laser.Base.MachineOperation.UserControls.ViewModel;
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

namespace MMK.SmartSystem.Laser.Base.MachineOperation.UserControls
{
    /// <summary>
    /// ManualHeaderControl.xaml 的交互逻辑
    /// </summary>
    public partial class ManualHeaderControl : UserControl
    {
        List<ManualHeaderViewModel> manualHeaders;
        public ManualHeaderControl()
        {
            InitializeComponent();
            headerItems.ItemsSource = manualHeaders = ManualHeaderViewModel.GetHeaderNodes();
        }

        public void SetHeaderActive(object type)
        {
            string name = type.GetType().Name;
            var obj = manualHeaders.FirstOrDefault(d => d.FullName == name);
            if (obj != null)
            {
                obj.IsCheck = true;
            }
        }
    }
}
