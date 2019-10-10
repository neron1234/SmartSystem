﻿using Abp.Dependency;
using MMK.SmartSystem.Laser.Base.MachineProcess.ViewModel;
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

namespace MMK.SmartSystem.Laser.Base.MachineProcess
{
    /// <summary>
    /// ProcessPage.xaml 的交互逻辑
    /// </summary>
    public partial class ProcessPage : Page, ITransientDependency
    {
        //public ProcessViewModel viewModel { get; set; }
        public ProcessPage()
        {
            InitializeComponent();
            Loaded += ProcessPage_Loaded;
        }

        private void ProcessPage_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
