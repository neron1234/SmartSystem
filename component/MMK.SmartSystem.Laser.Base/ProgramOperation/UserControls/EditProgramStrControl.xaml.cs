using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Laser.Base.ProgramOperation.UserControls.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// EditProgramStrControl.xaml 的交互逻辑
    /// </summary>
    public partial class EditProgramStrControl : UserControl
    {
        public EditProgramStrViewModel EditProgramStrVM { get; set; } = new EditProgramStrViewModel();
        public EditProgramStrControl(string url)
        {
            InitializeComponent();
            Loaded += EditProgramStrControl_Loaded;
            this.DataContext = EditProgramStrVM;
            EditProgramStrVM.Url = url;
            EditProgramStrVM.CloseEvent += EditProgramStrVM_CloseEvent;
        }

        private void EditProgramStrVM_CloseEvent()
        {
            Messenger.Default.Send(new PopupMsg() { IsClose = true });
        }

        private async void EditProgramStrControl_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Factory.StartNew(new Action(() =>
            {
                StringBuilder sb = new StringBuilder();
                using (StreamReader reader = new StreamReader(EditProgramStrVM.Url))
                {
                    var line = reader.ReadLine();
                    while (line != null)
                    {
                        sb.AppendLine(line);
                        line = reader.ReadLine();
                    }
                    reader.Dispose();
                }
                EditProgramStrVM.ProgramStr = sb.ToString();
            }));
        }
    }
}
