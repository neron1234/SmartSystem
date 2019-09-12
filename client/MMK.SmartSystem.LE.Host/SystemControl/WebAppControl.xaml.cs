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

namespace MMK.SmartSystem.LE.Host.SystemControl
{
    /// <summary>
    /// WebAppControl.xaml 的交互逻辑
    /// </summary>
    public partial class WebAppControl : UserControl
    {
        private bool _isLoadSuccess;
        public WebAppControl()
        {
            InitializeComponent();
            this.Loaded += WebAppControl_Loaded;
        }

        private void WebAppControl_Loaded(object sender, RoutedEventArgs e)
        {
            string path = System.IO.Path.Combine(System.Environment.CurrentDirectory, "WebApp","cncapp.exe");
            if (!_isLoadSuccess&&System.IO.File.Exists(path))
            {
                
                _isLoadSuccess = ctnTest.StartAndEmbedProcess(path);
            }
        }


    }
}
