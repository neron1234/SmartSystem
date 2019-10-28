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
        private ProcessOptionsViewModel processOptionsViewModel;

        public event Action<int> MaterialGroupChangeEvent;
        public event Action<bool> MoveGridHeadEvent;
        public ProcessOptionsControl()
        {
            InitializeComponent();
            DataContext = processOptionsViewModel = new ProcessOptionsViewModel();
            processOptionsViewModel.MaterialGroupChangeEvent += ProcessOptionsViewModel_MaterialGroupChangeEvent;
            Loaded += ProcessOptionsControl_Loaded;
            processOptionsViewModel.MoveGridHeadEvent += ProcessOptionsViewModel_MoveGridHeadEvent; 
        }

        private void ProcessOptionsViewModel_MoveGridHeadEvent(bool obj)
        {
            MoveGridHeadEvent?.Invoke(obj);
        }

        private void ProcessOptionsViewModel_MaterialGroupChangeEvent(int obj)
        {
            MaterialGroupChangeEvent?.Invoke(obj);
        }

        public void RefreshData()
        {
            Messenger.Default.Send(new MainSystemNoticeModel() { EventType = EventEnum.StartLoad });

            var model = new MaterialInfoEventData { IsCheckSon = true, SuccessAction = LoadData };
            Task.Factory.StartNew(new Action(() =>
            {
                EventBus.Default.Trigger(model);

            }));

        }
        private void ProcessOptionsControl_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshData();
        }

        private void LoadData(List<MeterialGroupThicknessDto> list)
        {
            Messenger.Default.Send(new MainSystemNoticeModel() { EventType = EventEnum.EndLoad });

            Dispatcher.BeginInvoke(new Action(() =>
            {
                processOptionsViewModel.InitMaterialData(list);

            }));

        }
    }
}
