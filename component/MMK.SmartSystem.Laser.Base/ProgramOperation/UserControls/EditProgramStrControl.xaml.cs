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
        public EditProgramStrControl(string url){
            InitializeComponent();
            Loaded += EditProgramStrControl_Loaded;
            this.DataContext = EditProgramStrVM;
            EditProgramStrVM.Url = url;
            EditProgramStrVM.CloseEvent += EditProgramStrVM_CloseEvent;
        }

        private void EditProgramStrVM_CloseEvent(){
            Messenger.Default.Send(new PopupMsg() { IsClose = true });
        }

        private async void EditProgramStrControl_Loaded(object sender, RoutedEventArgs e){
            await Task.Factory.StartNew(new Action(() =>{
                try{
                    List<string> str = File.ReadAllLines(EditProgramStrVM.Url).ToList();
                    EditProgramStrVM.ProgramStr = string.Join("  ", str.Take(100).ToArray());
                }catch (Exception ex){
                    Messenger.Default.Send(new Common.ViewModel.NotifiactionModel(){
                        Title = "操作失败",
                        Content = $"失败信息：{ex} {DateTime.Now}",
                        NotifiactionType = Common.ViewModel.EnumPromptType.Error
                    });
                }
            }));
        }
    }
}
