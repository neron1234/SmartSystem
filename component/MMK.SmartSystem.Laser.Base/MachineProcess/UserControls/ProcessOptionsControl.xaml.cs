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
            RegisterMaterial();
            Messenger.Default.Register<PagedResultDtoOfMachiningGroupDto>(this, (result) =>
            {
                processOptionsViewModel.MaterialThicknessList.Clear();
                foreach (var item in result.Items)
                {
                    processOptionsViewModel.MaterialThicknessList.Add(item);
                }
                if (processOptionsViewModel.MaterialThicknessList.Count > 0)
                {
                    processOptionsViewModel.SelectedMaterialTypeId = (int)processOptionsViewModel.MaterialThicknessList.First()?.Id;
                }
            });
        }

        private void Del_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            //d: DesignHeight = "245" d: DesignWidth = "600"
            Messenger.Default.Unregister<List<MaterialDto>>(this);

            new PopupWindow(new AddMaterialControl(),600,245,"添加工艺材料").ShowDialog();

            await RegisterMaterial();
        }

        private async Task RegisterMaterial()
        {
            Messenger.Default.Register<List<MaterialDto>>(this, (results) =>
            {
                processOptionsViewModel.MaterialTypeList.Clear();
                foreach (var item in results)
                {
                    processOptionsViewModel.MaterialTypeList.Add(item);
                }
                if (processOptionsViewModel.MaterialTypeList.Count > 0)
                {
                    processOptionsViewModel.SelectedMaterialId = (int)processOptionsViewModel.MaterialTypeList.First()?.Id;
                    processOptionsViewModel.MTypeSelectionCommand.Execute("");
                }
            });

            await EventBus.Default.TriggerAsync(new MaterialInfoEventData { IsAll = false });
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(processOptionsViewModel.SelectedMaterialId + "|" + processOptionsViewModel.SelectedMaterialTypeId);
        }
    }
}
