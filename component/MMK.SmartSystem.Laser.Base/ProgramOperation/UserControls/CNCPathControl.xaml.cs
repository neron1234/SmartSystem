using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Laser.Base.ProgramOperation.UserControls.ViewModel;
using MMK.SmartSystem.Laser.Base.ProgramOperation.ViewModel;
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

namespace MMK.SmartSystem.Laser.Base.ProgramOperation.UserControls
{
    /// <summary>
    /// CNCPathControl.xaml 的交互逻辑
    /// </summary>
    public partial class CNCPathControl : UserControl
    {
        public CNCPathViewModel cncPathVM { get; set; }
        public CNCPathControl(ReadProgramFolderItemViewModel readProgramFolder)
        {
            InitializeComponent();
            this.DataContext = cncPathVM = new CNCPathViewModel(readProgramFolder);
        }

        private void SaveCNCPathBtn_Click(object sender, RoutedEventArgs e)
        {
            var folder = ((ReadProgramFolderItemViewModel)this.CNCPathCascader.SelectedValues[this.CNCPathCascader.SelectedValues.Count - 1]).Folder;
            Messenger.Default.Send(new CNCProgramPath(folder));
            Messenger.Default.Send(new PopupMsg("保存CNC路径成功", true));
        }
    }
}
