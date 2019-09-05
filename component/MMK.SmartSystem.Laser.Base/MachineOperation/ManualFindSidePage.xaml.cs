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

namespace MMK.SmartSystem.Laser.Base.MachineOperation
{
    /// <summary>
    /// ManualFindSidePage.xaml 的交互逻辑
    /// </summary>
    public partial class ManualFindSidePage : Page, ITransientDependency
    {
        public ManualFindSidePage()
        {
            InitializeComponent();

            SmartSystem.Common.UserClient userClient = new Common.UserClient("http://localhost:21021", new System.Net.Http.HttpClient());
          
            var res = userClient.GetRolesAsync().Result;
            SmartSystem.Common.DepartmentClient client = new Common.DepartmentClient("http://localhost:21021", new System.Net.Http.HttpClient());
            var alls = client.GetAllAsync("", "", 0, 20).Result;

        }
    }
}
