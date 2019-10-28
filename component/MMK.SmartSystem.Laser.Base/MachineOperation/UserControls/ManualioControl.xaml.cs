﻿using MMK.SmartSystem.Laser.Base.MachineOperation.UserControls.ViewModel;
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

namespace MMK.SmartSystem.Laser.Base.MachineOperation.UserControls
{
    /// <summary>
    /// ManualioControl.xaml 的交互逻辑
    /// </summary>
    public partial class ManualioControl : UserControl
    {
        public ManualioViewModel mioVm { get; set; }
        public ManualioControl()
        {
            InitializeComponent();
            this.DataContext = mioVm = new ManualioViewModel();
        }
    }
}
