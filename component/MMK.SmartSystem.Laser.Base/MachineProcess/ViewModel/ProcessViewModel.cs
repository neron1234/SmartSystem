using Abp.Events.Bus;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common;
using MMK.SmartSystem.Common.EventDatas;
using MMK.SmartSystem.Laser.Base.MachineProcess.UserControls;
using MMK.SmartSystem.Laser.Base.MachineProcess.UserControls.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MMK.SmartSystem.Laser.Base.MachineProcess.ViewModel
{
    public class ProcessViewModel : ViewModelBase
    {
        public ProcessData SelectedData { get; set; }

        public ProcessViewModel()
        {

            //Messenger.Default.Register<ProcessData>(this, (pd) =>
            //{
            //    SelectedData = pd;
            //});
        }

        public int SearchGroupId = 0;

        private int currentType = 1;

        public void RefreshGroupData(int groupId)
        {
            SearchGroupId = groupId;
            InitGroup(currentType);

        }
        private void InitGroup(int type)
        {
            if (SearchGroupId == 0)
            {
                return;
            }
            currentType = type;
            if (type == 1)
            {
                RefreshCuttingDataEvent?.Invoke(new CuttingDataByGroupIdEventData() { machiningDataGroupId = SearchGroupId });
                return;
            }
            if (type == 2)
            {
                RefreshPiercingDataEvent?.Invoke(new PiercingDataByGroupIdEventData() { machiningDataGroupId = SearchGroupId });
                return;
            }
            if (type == 3)
            {
                RefreshEdgeCuttingDataEvent?.Invoke(new EdgeCuttingByGroupIdEventData() { machiningDataGroupId = SearchGroupId });
                return;
            }
            if (type == 4)
            {
                RefreshSlopeControlDataEvent?.Invoke(new SlopeControlDataByGroupIdEventData() { machiningDataGroupId = SearchGroupId });
                return;
            }
        }
        public event Action<CuttingDataByGroupIdEventData> RefreshCuttingDataEvent;
        public event Action<PiercingDataByGroupIdEventData> RefreshPiercingDataEvent;
        public event Action<EdgeCuttingByGroupIdEventData> RefreshEdgeCuttingDataEvent;
        public event Action<SlopeControlDataByGroupIdEventData> RefreshSlopeControlDataEvent;

        public event Action AddMaterialEvent;
        public ICommand DataChangeCommand
        {
            get
            {
                return new RelayCommand<string>((s) => InitGroup(Convert.ToInt32(s)));
            }
        }


        public ICommand UpLoadCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    new PopupWindow(new EditMaterialControl(SelectedData), 900, 590, "上传工艺库").ShowDialog();
                });
            }
        }

        public ICommand EditCommand
        {
            get
            {
                return new RelayCommand(() =>
                {

                    AddMaterialEvent?.Invoke();

                });
            }
        }
    }
}
