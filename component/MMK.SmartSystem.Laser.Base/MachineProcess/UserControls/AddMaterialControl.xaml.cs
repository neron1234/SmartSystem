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
        public AddMaterialControl()
        {
            InitializeComponent();

            this.DataContext = addMaterialViewModel = new AddMaterialViewModel();
            addMaterialViewModel.SaveMachiningEvent += AddMaterialViewModel_SaveMachiningEvent;
            this.Loaded += AddMaterialControl_Loaded;

        }

        private async void AddMaterialViewModel_SaveMachiningEvent(CreateMachiningGroupDto obj)
        {
            Messenger.Default.Send(new MainSystemNoticeModel() { EventType = EventEnum.StartLoad });
            await Task.Run(new Action(() =>
            {
                EventBus.Default.Trigger(new AddMachiningGroupInfoEventData()
                {
                    CreateMachiningGroup = obj,
                    SuccessAction = (s) =>
                    {
                        addMaterialViewModel.MaterialThickness = "";
                        Messenger.Default.Send(new PopupMsg("保存成功", true));
                        Messenger.Default.Send(new MainSystemNoticeModel() { EventType = EventEnum.EndLoad });

                    }
                });
            }));
        }

        private async void AddMaterialControl_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Run(new Action(() =>
            {
                EventBus.Default.Trigger(new MaterialInfoEventData
                {
                    IsCheckSon = false,
                    SuccessAction = (d) =>
                    {
                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            foreach (var item in d)
                            {
                                addMaterialViewModel.MaterialTypeList.Add(item);
                            }
                            if (addMaterialViewModel.MaterialTypeList.Count > 0)
                            {
                                addMaterialViewModel.SelectedMaterialId = (int)addMaterialViewModel.MaterialTypeList.First()?.Code;
                            }
                        }));
                    }
                });
            }));
        }
    }
}
