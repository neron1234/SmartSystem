﻿using MMK.SmartSystem.Common.Base;
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
    /// SimpleProfilePage.xaml 的交互逻辑
    /// </summary>
    public partial class SimpleProfilePage : SignalrPage
    {
        public SimpleProfilePage()
        {
            InitializeComponent();
            manualControl.SetHeaderActive(this);
        }

        public override void CncOnError(string message)
        {

        }

        public override List<object> GetResultViewModelMap()
        {
            return default;
        }

        private void TextBlock_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var windows = new InputWindow("15", 0, 100, "引线H");
            windows.InputWindowFinishEvent += (s) => textBlock.Text = s;
            windows.ShowDialog();
        }
    }
}
