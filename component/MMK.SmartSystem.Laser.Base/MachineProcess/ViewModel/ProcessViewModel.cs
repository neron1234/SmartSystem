using Abp.Events.Bus;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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
        public int MchiningDataGroupId { get; set; }

        public ProcessViewModel()
        {

        }

        public ICommand CuttingDataCommand{
            get{
                return new RelayCommand(() => {
                    EventBus.Default.TriggerAsync(new CuttingDataByGroupIdEventData() { machiningDataGroupId = this.MchiningDataGroupId });
                });
            }
        }

        public ICommand PiercingDataCommand{
            get{
                return new RelayCommand(() => {
                    EventBus.Default.TriggerAsync(new PiercingDataByGroupIdEventData() { machiningDataGroupId = this.MchiningDataGroupId });
                });
            }
        }
        public ICommand EdgeCuttingCommand{
            get{
                return new RelayCommand(() => {
                    EventBus.Default.TriggerAsync(new EdgeCuttingByGroupIdEventData() { machiningDataGroupId = this.MchiningDataGroupId });
                });
            }
        }
        public ICommand SlopeControlDatCommand{
            get{
                return new RelayCommand(() => {
                    EventBus.Default.TriggerAsync(new SlopeControlDataByGroupIdEventData() { machiningDataGroupId = this.MchiningDataGroupId });
                });
            }
        }
    }
}
