using Abp.Events.Bus;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common;
using MMK.SmartSystem.Common.EventDatas;
using MMK.SmartSystem.Common.Model;
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
    /// ProcessOptionsControl.xaml 的交互逻辑
    /// </summary>
    public partial class ProcessOptionsControl : UserControl
    {
        private ProcessOptionsViewModel processOptionsViewModel { get; set; }
        public ProcessOptionsControl()
        {
            InitializeComponent();
            this.DataContext = processOptionsViewModel = new ProcessOptionsViewModel();
            
            Messenger.Default.Register<PagedResultDtoOfMaterialDto>(this, (results) =>
            {
                processOptionsViewModel.MaterialTypeList.Clear();
                foreach (var item in results.Items)
                {
                    processOptionsViewModel.MaterialTypeList.Add(item.Name_CN);
                }
            });

            Loaded += ProcessOptionsControl_Loaded;
        }

        private async void ProcessOptionsControl_Loaded(object sender, RoutedEventArgs e)
        {
            await EventBus.Default.TriggerAsync(new MaterialInfoEventData()) ;
        }

        private void Del_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
