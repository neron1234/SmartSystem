using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common;
using MMK.SmartSystem.Laser.Base.CustomControl;
using MMK.SmartSystem.Laser.Base.ProgramOperation.UserControls.ViewModel;
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
using System.Collections.ObjectModel;
using Abp.Events.Bus;
using MMK.SmartSystem.Common.EventDatas;

namespace MMK.SmartSystem.Laser.Base.ProgramOperation.UserControls
{
    /// <summary>
    /// EditProgramControl.xaml 的交互逻辑
    /// </summary>
    public partial class UpLoadLocalProgramControl : UserControl
    {
        public UpLoadLocalProgramViewModel upLoadProViewModel { get; set; }
        public UpLoadLocalProgramControl()
        {
            InitializeComponent();
            this.DataContext = upLoadProViewModel = new UpLoadLocalProgramViewModel();

            Messenger.Default.Register<PagedResultDtoOfNozzleKindDto>(this, (results) =>
            {
                upLoadProViewModel.NozzleKindList = new ObservableCollection<NozzleKindDto>();
                foreach (var item in results.Items)
                {
                    upLoadProViewModel.NozzleKindList.Add(item);
                }
                if (upLoadProViewModel.NozzleKindList.Count > 0)
                {
                    upLoadProViewModel.SelectedNozzleKindCode = (int)upLoadProViewModel.NozzleKindList.First()?.Code;
                }
            });

            Task.Factory.StartNew(new Action(() =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    EventBus.Default.TriggerAsync(new NozzleKindEventData());
                });
            }));
        }
    }
}
