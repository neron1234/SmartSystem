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
    /// CNCListControl.xaml 的交互逻辑
    /// </summary>
    public partial class CNCProgramListControl : UserControl
    {
        public CNCProgramListViewModel cpViewModel { get; set; }
        public CNCProgramListControl(ReadProgramFolderItemViewModel readProgramFolder, List<ProgramViewModel> programList)
        {
            InitializeComponent();
            this.DataContext = cpViewModel = new CNCProgramListViewModel(readProgramFolder);
            cpViewModel.LocalProgramList = programList;
            cpViewModel.DataPaging(1);
        }

        private void ProgramGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = ((DataGrid)sender).SelectedValue;
            if (selected != null && selected is ProgramViewModel){
                Messenger.Default.Send((ProgramViewModel)selected);
            }
        }
    }
}
