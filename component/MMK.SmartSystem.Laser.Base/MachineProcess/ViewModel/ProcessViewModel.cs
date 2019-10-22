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
            this.commandType = 1;
            Messenger.Default.Register<int>(this, (result) =>
            {
                this.SearchGroupId = result;
                Task.Factory.StartNew(new Action(() => {
                    SearchList();
                }));
            });
        }

        public int SearchGroupId = 0;
        //private BaseErrorEventData commandType { get; set; }
        private int commandType { get; set; }
        public async void SearchList()
        {
            if (SearchGroupId == 0){
                return;
            }
            switch (commandType)
            {
                case 1:
                    await EventBus.Default.TriggerAsync(new CuttingDataByGroupIdEventData() { machiningDataGroupId = SearchGroupId });
                    break;
                case 2:
                    await EventBus.Default.TriggerAsync(new PiercingDataByGroupIdEventData() { machiningDataGroupId = SearchGroupId });
                    break;
                case 3:
                    await EventBus.Default.TriggerAsync(new EdgeCuttingByGroupIdEventData() { machiningDataGroupId = SearchGroupId });
                    break;
                case 4:
                    await EventBus.Default.TriggerAsync(new SlopeControlDataByGroupIdEventData() { machiningDataGroupId = SearchGroupId });
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
                    Task.Factory.StartNew(new Action(() => {
                        SearchList();
                    }));
                    //commandType = new CuttingDataByGroupIdEventData() { machiningDataGroupId = this.MchiningDataGroupId };
                    //EventBus.Default.TriggerAsync(new CuttingDataByGroupIdEventData() { machiningDataGroupId = this.MchiningDataGroupId });
                });
            }
        }

        public ICommand PiercingDataCommand{
            get{
                return new RelayCommand(() => {
                    commandType = 2;
                    Task.Factory.StartNew(new Action(() => {
                        SearchList();
                    }));
                    //EventBus.Default.TriggerAsync(new PiercingDataByGroupIdEventData() { machiningDataGroupId = this.MchiningDataGroupId });
                });
            }
        }

        public ICommand EdgeCuttingCommand{
            get{
                return new RelayCommand(() => {
                    commandType = 3;
                    Task.Factory.StartNew(new Action(() => {
                        SearchList();
                    }));
                    //EventBus.Default.TriggerAsync(new EdgeCuttingByGroupIdEventData() { machiningDataGroupId = this.MchiningDataGroupId });
                });
            }
        }

        public ICommand SlopeControlDatCommand{
            get{
                return new RelayCommand(() => {
                    commandType = 4;
                    Task.Factory.StartNew(new Action(() => {
                        SearchList();
                    }));
                    //EventBus.Default.TriggerAsync(new SlopeControlDataByGroupIdEventData() { machiningDataGroupId = this.MchiningDataGroupId });
                });
            }
        }
    }
}
