using Abp.Events.Bus;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common.EventDatas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace MMK.SmartSystem.Laser.Base.MachineProcess.ViewModel
{
    public class ProcessViewModel:ViewModelBase
    {
        public ProcessViewModel()
        {
            Messenger.Default.Register<int>(this, (result) =>
            {
                SearchGroupId = result;
                SearchList();
            });
            CuttingDataCommand.Execute("");
        }
        public int SearchGroupId = 0;
        //private BaseErrorEventData commandType { get; set; }
        private int commandType { get; set; }
        private void SearchList()
        {
            if (SearchGroupId == 0)
            {
                return;
            }
            switch (commandType)
            {
                case 1:
                    EventBus.Default.TriggerAsync(new CuttingDataByGroupIdEventData() { machiningDataGroupId = SearchGroupId });
                    break;
                case 2:
                    EventBus.Default.TriggerAsync(new PiercingDataByGroupIdEventData() { machiningDataGroupId = SearchGroupId });
                    break;
                case 3:
                    EventBus.Default.TriggerAsync(new EdgeCuttingByGroupIdEventData() { machiningDataGroupId = SearchGroupId });
                    break;
                case 4:
                    EventBus.Default.TriggerAsync(new SlopeControlDataByGroupIdEventData() { machiningDataGroupId = SearchGroupId });
                    break;
                default:
                    break;
            }
            //EventBus.Default.TriggerAsync(Convert.ChangeType(commandType, commandType.GetType()));
        }

        public ICommand CuttingDataCommand{
            get{
                return new RelayCommand(() => {
                    commandType = 1;
                    SearchList();
                    //commandType = new CuttingDataByGroupIdEventData() { machiningDataGroupId = this.MchiningDataGroupId };
                    //EventBus.Default.TriggerAsync(new CuttingDataByGroupIdEventData() { machiningDataGroupId = this.MchiningDataGroupId });
                });
            }
        }

        public ICommand PiercingDataCommand{
            get{
                return new RelayCommand(() => {
                    commandType = 2;
                    SearchList();
                    //EventBus.Default.TriggerAsync(new PiercingDataByGroupIdEventData() { machiningDataGroupId = this.MchiningDataGroupId });
                });
            }
        }
        public ICommand EdgeCuttingCommand{
            get{
                return new RelayCommand(() => {
                    commandType = 3;
                    SearchList();
                    //EventBus.Default.TriggerAsync(new EdgeCuttingByGroupIdEventData() { machiningDataGroupId = this.MchiningDataGroupId });
                });
            }
        }
        public ICommand SlopeControlDatCommand{
            get{
                return new RelayCommand(() => {
                    commandType = 4;
                    SearchList();
                    //EventBus.Default.TriggerAsync(new SlopeControlDataByGroupIdEventData() { machiningDataGroupId = this.MchiningDataGroupId });
                });
            }
        }
    }
}
