using Abp.Events.Bus;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common;
using MMK.SmartSystem.Common.EventDatas;
using MMK.SmartSystem.Common.Model;
using MMK.SmartSystem.Laser.Base.MachineProcess.UserControls.ViewModel;
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

namespace MMK.SmartSystem.Laser.Base.MachineProcess.UserControls
{
    /// <summary>
    /// AddMaterialControl.xaml 的交互逻辑
    /// </summary>
    public partial class AddMaterialControl : UserControl
    {
        private AddMaterialViewModel addMaterialViewModel { get; set; }
        private string Error { get; set; }
        public AddMaterialControl()
        {
            InitializeComponent();

            this.DataContext = addMaterialViewModel = new AddMaterialViewModel();

            Messenger.Default.Register<PagedResultDtoOfMeterialGroupThicknessDto>(this, (results) =>
            {
                addMaterialViewModel.MaterialTypeList = new ObservableCollection<MeterialGroupThicknessDto>();
                foreach (var item in results.Items)
                {
                    addMaterialViewModel.MaterialTypeList.Add(item);
                }
                if (addMaterialViewModel.MaterialTypeList.Count > 0)
                {
                    addMaterialViewModel.SelectedMaterialId = (int)addMaterialViewModel.MaterialTypeList.First()?.MaterialCode;
                }

                Messenger.Default.Unregister<PagedResultDtoOfMeterialGroupThicknessDto>(this);
            });


            Task.Factory.StartNew(new Action(() =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    EventBus.Default.TriggerAsync(new MaterialInfoEventData { IsCheckSon = false });
                });
            }));
        }
    }
}
