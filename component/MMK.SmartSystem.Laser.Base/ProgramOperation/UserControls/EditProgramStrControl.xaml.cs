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
        public EditProgramStrControl(string fillName){
            InitializeComponent();
            this.DataContext = EditProgramStrVM;
            EditProgramStrVM.FillName = fillName;
            EditProgramStrVM.CloseEvent += EditProgramStrVM_CloseEvent;
            EditProgramStrVM.SaveEvent += EditProgramStrVM_SaveEvent;
            Task.Factory.StartNew(new Action(() => {
                try
                {
                    EditProgramStrVM.ProgramStr = System.IO.File.ReadAllText(fillName);
                    //this.ProgramStr = string.Join("  ", str.Take(100).ToArray());
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        this.MyTextEditor.Text = EditProgramStrVM.ProgramStr;
                    }));
                }
                catch (Exception ex)
                {
                    Messenger.Default.Send(new Common.ViewModel.NotifiactionModel()
                    {
                        Title = "操作失败",
                        Content = $"失败信息：{ex} {DateTime.Now}",
                        NotifiactionType = Common.ViewModel.EnumPromptType.Error
                    });
                }
            }));
        }

        private void EditProgramStrVM_SaveEvent()
        {
            using (FileStream fsWrite = new FileStream(EditProgramStrVM.FillName, FileMode.OpenOrCreate, FileAccess.Write))
            {
                byte[] buffer = Encoding.Default.GetBytes(this.MyTextEditor.Text);
                fsWrite.Write(buffer, 0, buffer.Length);
                fsWrite.Close();
                fsWrite.Dispose();
                Messenger.Default.Send(new PopupMsg("", false));
            }
        }

        private void EditProgramStrVM_CloseEvent(){
            Messenger.Default.Send(new PopupMsg() { IsClose = true });
        }
    }
}
