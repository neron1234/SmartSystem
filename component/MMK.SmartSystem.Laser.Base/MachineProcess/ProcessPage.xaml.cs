using Abp.Dependency;
using Abp.Events.Bus;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common;
using MMK.SmartSystem.Common.EventDatas;
using MMK.SmartSystem.Laser.Base.MachineProcess.ViewModel;
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

namespace MMK.SmartSystem.Laser.Base.MachineProcess
{
    /// <summary>
    /// ProcessPage.xaml 的交互逻辑
    /// </summary>
    public partial class ProcessPage : Page, ITransientDependency
    {
        public ProcessViewModel processViewModel { get; set; }
        public ProcessPage()
        {
            InitializeComponent();
            this.DataContext = processViewModel = new ProcessViewModel();
            processViewModel.RefreshCuttingDataEvent += ProcessViewModel_RefreshDataEvent<CuttingDataByGroupIdEventData, CuttingDataDto>;
            processViewModel.RefreshEdgeCuttingDataEvent += ProcessViewModel_RefreshDataEvent<EdgeCuttingByGroupIdEventData, EdgeCuttingDataDto>;
            processViewModel.RefreshPiercingDataEvent += ProcessViewModel_RefreshDataEvent<PiercingDataByGroupIdEventData, PiercingDataDto>;
            processViewModel.RefreshSlopeControlDataEvent += ProcessViewModel_RefreshDataEvent<SlopeControlDataByGroupIdEventData, SlopeControlDataDto>;
            processOption.MaterialGroupChangeEvent += processViewModel.RefreshGroupData;
            processOption.MoveGridHeadEvent += processList.MoveDataGridHeader;
        }

        private void ProcessViewModel_RefreshDataEvent<T, U>(T obj) where T : BaseApiEventData<List<U>>
        {
            obj.SuccessAction = (s) => processList.RefreshGroupData(s);
            Task.Factory.StartNew(new Action(() =>
            {
                EventBus.Default.Trigger(obj);
            }));
        }
    }
}
