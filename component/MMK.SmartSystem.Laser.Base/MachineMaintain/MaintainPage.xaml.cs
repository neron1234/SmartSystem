using Abp.Dependency;
using Abp.Events.Bus;
using MMK.SmartSystem.Common.EventDatas;
using MMK.SmartSystem.Laser.Base.MachineMaintain.ViewModel;
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

namespace MMK.SmartSystem.Laser.Base.MachineMaintain
{
    /// <summary>
    /// MaintainPage.xaml 的交互逻辑
    /// </summary>
    public partial class MaintainPage : Page, ITransientDependency
    {
        public MaintainViewModel mVM { get; set; }
        public MaintainPage()
        {
            InitializeComponent();
            this.DataContext = mVM = new MaintainViewModel();
        }

        private void ProcessViewModel_RefreshDataEvent<T, U>(T obj) where T : BaseApiEventData<List<U>>
        {
            //obj.SuccessAction = (s) => processList.RefreshGroupData(s);
            Task.Factory.StartNew(new Action(() =>
            {
                EventBus.Default.Trigger(obj);
            }));
        }
    }
}
