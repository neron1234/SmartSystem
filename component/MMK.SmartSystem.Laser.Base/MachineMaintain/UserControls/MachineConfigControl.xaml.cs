﻿using MMK.SmartSystem.Laser.Base.MachineMaintain.UserControls.ViewModel;
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

namespace MMK.SmartSystem.Laser.Base.MachineMaintain.UserControls
{
    /// <summary>
    /// MachineConfigControl.xaml 的交互逻辑
    /// </summary>
    public partial class MachineConfigControl : UserControl
    {
        public MachineConfigViewModel machineConfigVM { get; set; }
        public MachineConfigControl()
        {
            InitializeComponent();
            this.DataContext = machineConfigVM = new MachineConfigViewModel();
        }
    }
}
