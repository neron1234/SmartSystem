using MMK.SmartSystem.Common;
using MMK.SmartSystem.Laser.Base.MachineProcess.UserControls.ViewModel;
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

namespace MMK.SmartSystem.Laser.Base.MachineProcess.UserControls
{
    /// <summary>
    /// EditMaterialControl.xaml 的交互逻辑
    /// </summary>
    public partial class EditMaterialControl : UserControl
    {
        public EditMaterialViewModel EditVM { get; set; }
        public EditMaterialControl(ProcessData mData)
        {
            InitializeComponent();
            this.DataContext = EditVM = new EditMaterialViewModel();
            switch (mData.Type)
            {
                case "CuttingDataDto":
                    break;
                case "EdgeCuttingDataDto":
                    break;
                case "PiercingDataDto":
                    break;
                case "SlopeControlDataDto":
                    break;
                default:
                    break;
            }
        }
    }
}
