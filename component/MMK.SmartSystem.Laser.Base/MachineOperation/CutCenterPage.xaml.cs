using Abp.Dependency;
using MMK.SmartSystem.Common.Base;
using MMK.SmartSystem.WebCommon.DeviceModel;
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

namespace MMK.SmartSystem.Laser.Base.MachineOperation
{
    /// <summary>
    /// CutCenterPage.xaml 的交互逻辑
    /// </summary>
    public partial class CutCenterPage : SignalrPage
    {
        /// <summary>
        /// 割嘴对中
        /// </summary>
        public CutCenterPage()
        {
            InitializeComponent();
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
