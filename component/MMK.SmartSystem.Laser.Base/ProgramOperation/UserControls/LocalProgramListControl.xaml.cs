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
    /// LocalProgramListControl.xaml 的交互逻辑
    /// </summary>
    public partial class LocalProgramListControl : UserControl
    {
        public LocalProgramListViewModel lpViewModel { get; set; }
        public LocalProgramListControl(string connectId,ReadProgramFolderItemViewModel readProgramFolder)
        {
            InitializeComponent();
            this.DataContext = lpViewModel = new LocalProgramListViewModel();
            lpViewModel.ProgramFolderInfo = readProgramFolder;
            lpViewModel.ConnectId = connectId;
        }

        private void ProgramGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = ((DataGrid)sender).SelectedValue;
            if (selected != null && selected is ProgramViewModel)
            {
                lpViewModel.SelectedProgramViewModel = (ProgramViewModel)selected;
                Messenger.Default.Send(lpViewModel.SelectedProgramViewModel);

                System.IO.StreamReader reader = new System.IO.StreamReader(lpViewModel.Path + @"\" + lpViewModel.SelectedProgramViewModel.Name);
                var line = reader.ReadLine();
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < 30; i++)
                {
                    if (line != null)
                    {
                        line = reader.ReadLine();
                        sb.AppendLine(line);
                    }
                }
                Messenger.Default.Send(sb);
            }
        }
    }
}
