﻿using MMK.SmartSystem.Common.Base;
using MMK.SmartSystem.Laser.Base.MachineOperation.ViewModel;
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
    /// RemainCutPage.xaml 的交互逻辑
    /// </summary>
    public partial class RemainCutPage : SignalrPage
    {
        public RemainCutViewModel remainCutVM { get; set; }

        /// <summary>
        /// 余料切割
        /// </summary>
        public RemainCutPage()
        {
            InitializeComponent();
            manualControl.SetHeaderActive(this);
            this.DataContext = remainCutVM = new RemainCutViewModel();
        }
     

        public override List<object> GetResultViewModelMap()
        {
            return default;
        }

        public override void CncOnError(string message)
        {
        }
    }
}
