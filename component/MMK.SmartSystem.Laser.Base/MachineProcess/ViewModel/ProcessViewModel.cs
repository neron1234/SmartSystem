using Abp.Events.Bus;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common.EventDatas;
using MMK.SmartSystem.Laser.Base.MachineProcess.UserControls;
using MMK.SmartSystem.Laser.Base.MachineProcess.UserControls.ViewModel;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MMK.SmartSystem.Laser.Base.MachineProcess.ViewModel
{
    public class ProcessViewModel:ViewModelBase{
        public ProcessData SelectedData { get; set; }

        public ProcessViewModel(){
            this.commandType = 1;
            Messenger.Default.Register<int>(this, (result) => {
                this.SearchGroupId = result;
                Task.Factory.StartNew(new Action(() => {
                    SearchList();
                }));
            });
            Messenger.Default.Register<ProcessData>(this, (pd) => {
                SelectedData = pd;
            });
        }

        public int SearchGroupId = 0;
        private int commandType { get; set; }
        public async void SearchList(){
            if (SearchGroupId == 0){
                return;
            }
            switch (commandType){
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
        }

        public ICommand CuttingDataCommand{
            get{
                return new RelayCommand(() => {
                    commandType = 1;
                    Task.Factory.StartNew(new Action(() => {
                        SearchList();
                    }));
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
                });
            }
        }
        
        public ICommand UpLoadCommand{
            get{
                return new RelayCommand(() => {
                    new PopupWindow(new EditMaterialControl(SelectedData), 900, 590, "上传工艺库").ShowDialog();
                });
            }
        }

        public ICommand EditCommand
        {
            get
            {
                return new RelayCommand(() => {
                    Messenger.Default.Send("UnRegisterMaterial");
                    //UnRegisterMaterial();

                    new PopupWindow(new AddMaterialControl(), 650, 260, "添加工艺材料").ShowDialog();

                    Messenger.Default.Send("RegisterMaterial");
                    //RegisterMaterial();
                    Messenger.Default.Send("GetMaterial");
                });
            }
        }
    }
}
