using Abp.Dependency;
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

namespace MMK.SmartSystem.CNC.Host.Pages
{
    /// <summary>
    /// HelloPage.xaml 的交互逻辑
    /// </summary>
    public partial class HelloPage : Page, ITransientDependency
    {
        int num = 0;
        public HelloPage()
        {
            InitializeComponent();
            this.Loaded += HelloPage_Loaded;
        }

        private void HelloPage_Loaded(object sender, RoutedEventArgs e)
        {
            num++;
            textBlock.Text = num.ToString();
        }
    }
}
