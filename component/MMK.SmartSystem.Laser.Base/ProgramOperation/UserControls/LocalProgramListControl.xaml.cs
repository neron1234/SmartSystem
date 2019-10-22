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
        public LocalProgramListControl(string connectId, ReadProgramFolderItemViewModel readProgramFolder)
        {
            InitializeComponent();
            this.DataContext = lpViewModel = new LocalProgramListViewModel();
            lpViewModel.ConnectId = connectId;
            lpViewModel.ProgramFolderInfo = readProgramFolder;
        }

        private void ProgramGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = ((DataGrid)sender).SelectedValue;
            if (selected != null && selected is ProgramViewModel)
            {
                lpViewModel.SelectedProgramViewModel = (ProgramViewModel)selected;
                //Messenger.Default.Send(lpViewModel.SelectedProgramViewModel);
                StringBuilder sb = new StringBuilder();
                using (System.IO.StreamReader reader = new System.IO.StreamReader(lpViewModel.Path + @"\" + lpViewModel.SelectedProgramViewModel.Name))
                {
                    var line = reader.ReadLine();
                    for (int i = 0; i < 28; i++)
                    {
                        if (line != null)
                        {
                            line = reader.ReadLine();
                            sb.AppendLine(line);
                        }
                    }
                    reader.Dispose();
                }
                Messenger.Default.Send(sb);
            }
        }
    }
}
