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

            Messenger.Default.Register<PagedResultDtoOfMaterialDto>(this, (results) =>
            {
                addMaterialViewModel.MaterialTypeList.Clear();
                foreach (var item in results.Items)
                {
                    addMaterialViewModel.MaterialTypeList.Add(item);
                }
            });
            Loaded += AddMaterialControl_Loaded; ;
        }

        private async void AddMaterialControl_Loaded(object sender, RoutedEventArgs e)
        {
            await EventBus.Default.TriggerAsync(new MaterialInfoEventData { IsAll = true });
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Register<MainSystemNoticeModel>(this, (ms) =>{
                if (ms.Success){
                    ms.SuccessAction?.Invoke();
                }else{
                    Error = ms.Error;
                    ms.ErrorAction?.Invoke();
                }
            });

            await EventBus.Default.TriggerAsync(new AddMaterialEventData
            {
                MaterialThickness = Convert.ToDouble(addMaterialViewModel.MaterialThickness),
                MaterialId = addMaterialViewModel.SelectedMaterialId,
                SuccessAction = SaveSuccessAction,
                ErrorAction = SaveErrorAction
            });

        }
        private void SaveSuccessAction()
        {
            Messenger.Default.Unregister<MainSystemNoticeModel>(this);
            Messenger.Default.Unregister<PagedResultDtoOfMaterialDto>(this);
            Messenger.Default.Send("保存成功");
        }
        private void SaveErrorAction()
        {
            Messenger.Default.Unregister<MainSystemNoticeModel>(this);
        }
    }
}
