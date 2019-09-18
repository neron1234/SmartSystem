using Abp.Dependency;
using MMK.SmartSystem.Common.ViewModel;
using MMK.SmartSystem.Laser.Base.MachineOperation.ViewModel;
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
using System.Reflection;

namespace MMK.SmartSystem.Laser.Base.MachineOperation
{
    /// <summary>
    /// AutoFindSidePage.xaml 的交互逻辑
    /// </summary>
    public partial class AutoFindSidePage : Page, ITransientDependency
    {
        public AutoFindSidePageViewModel AutoFindSidePageViewModel { get; set; }
        public AutoFindSidePage()
        {
            InitializeComponent();

            var declaringType = MethodBase.GetCurrentMethod().DeclaringType;
            this.DataContext = AutoFindSidePageViewModel = new AutoFindSidePageViewModel();

            AutoFindSidePageViewModel.IsEdit = AutoFindSidePageViewModel.IsEdit.ToPermission($"{declaringType.Namespace.Substring(declaringType.Namespace.LastIndexOf('.') + 1, declaringType.Namespace.Length - declaringType.Namespace.LastIndexOf('.') - 1)}.{declaringType.Name}.Edit");
        }
    }
}
