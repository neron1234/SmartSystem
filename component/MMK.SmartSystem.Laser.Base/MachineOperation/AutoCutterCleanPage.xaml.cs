using Abp.Dependency;
using MMK.SmartSystem.Common.Base;
using MMK.SmartSystem.Common.ViewModel;
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
using MMK.SmartSystem.WebCommon.DeviceModel;
using MMK.SmartSystem.Common.SignalrProxy;
using MMK.SmartSystem.Common.Model;

namespace MMK.SmartSystem.Laser.Base.MachineOperation
{
    /// <summary>
    /// AutoCutterCleanPage.xaml 的交互逻辑
    /// </summary>
    public partial class AutoCutterCleanPage : SignalrPage
    {
        /// <summary>
        /// 割嘴自动清理
        /// </summary>
        public AutoCutterCleanPage()
        {
            InitializeComponent();
            //this.DataContext = new MainTranslateViewModel();
        }

        public override void CncOnError(string message)
        {
            throw new NotImplementedException();
        }

        public override List<CncEventData> GetCncEventData()
        {
            throw new NotImplementedException();
        }

        public override List<object> GetResultViewModelMap()
        {
            throw new NotImplementedException();
        }

        public override void PageSignlarLoaded()
        {
            throw new NotImplementedException();
        }
    }
}
