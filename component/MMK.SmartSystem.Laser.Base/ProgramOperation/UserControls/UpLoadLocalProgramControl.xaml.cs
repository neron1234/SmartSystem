using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common;
using MMK.SmartSystem.Laser.Base.CustomControl;
using MMK.SmartSystem.Laser.Base.ProgramOperation.UserControls.ViewModel;
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
using System.Collections.ObjectModel;
using Abp.Events.Bus;
using MMK.SmartSystem.Common.EventDatas;

namespace MMK.SmartSystem.Laser.Base.ProgramOperation.UserControls
{
    /// <summary>
    /// EditProgramControl.xaml 的交互逻辑
    /// </summary>
    public partial class UpLoadLocalProgramControl : UserControl
    {
        public UpLoadLocalProgramViewModel upLoadProViewModel { get; set; }
        public UpLoadLocalProgramControl(string programPath, ReadProgramFolderItemViewModel programFolderInfo)
        {
            InitializeComponent();
            this.DataContext = upLoadProViewModel = new UpLoadLocalProgramViewModel(programFolderInfo);
            upLoadProViewModel.LocalProgramPath = programPath;
            Messenger.Default.Register<PagedResultDtoOfNozzleKindDto>(this, (results) =>
            {
                upLoadProViewModel.NozzleKindList = new ObservableCollection<NozzleKindDto>();
                foreach (var item in results.Items)
                {
                    upLoadProViewModel.NozzleKindList.Add(item);
                }
                if (upLoadProViewModel.NozzleKindList.Count > 0)
                {
                    upLoadProViewModel.SelectedNozzleKindCode = (int)upLoadProViewModel.NozzleKindList.First()?.Code;
                }
            });
            Messenger.Default.Register<PagedResultDtoOfMaterialDto>(this, (results) =>
            {
                upLoadProViewModel.MaterialTypeList = new ObservableCollection<MaterialDto>();
                foreach (var item in results.Items)
                {
                    upLoadProViewModel.MaterialTypeList.Add(item);
                }
                if (upLoadProViewModel.MaterialTypeList.Count > 0)
                {
                    upLoadProViewModel.SelectedMaterialId = (int)upLoadProViewModel.MaterialTypeList.First()?.Code;
                }
            });
            Messenger.Default.Register<ProgramDetailViewModel>(this, (pds) =>
            {
                upLoadProViewModel.ProgramDetail = pds;
                if (!string.IsNullOrEmpty(pds.Material)){
                    upLoadProViewModel.SelectedMaterialId = (int)upLoadProViewModel.MaterialTypeList.FirstOrDefault(n => n.Name_CN == pds.Material)?.Code;
                }
                if (!string.IsNullOrEmpty(pds.NozzleKind))
                {
                    upLoadProViewModel.SelectedNozzleKindCode = (int)upLoadProViewModel.NozzleKindList.FirstOrDefault(n => n.Name_CN == pds.NozzleKind)?.Code;
                }
            });

            Messenger.Default.Register<KeyCode>(this, (kCode) => InputTextBox(kCode));

            Task.Factory.StartNew(new Action(() =>
            {
                EventBus.Default.TriggerAsync(new NozzleKindEventData { SuccessAction = (nzList) => {
                    this.Dispatcher.Invoke(() =>
                    {

                    });
                } });
                EventBus.Default.TriggerAsync(new MaterialInfoEventData { IsCheckSon = false,SuccessAction = ((mtList) => {
                    this.Dispatcher.Invoke(() =>
                    {

                    });
                })});
            }));

            Loaded += UpLoadLocalProgramControl_Loaded;
            //this.CNCPathCascader.SelectedItem = upLoadProViewModel.SelectedProgramFolders;
        }

        private void UpLoadLocalProgramControl_Loaded(object sender, RoutedEventArgs e)
        {
            tbList = new List<TextBox>();
            foreach (var item in TbPanel1.Children)
            {
                if (item is TextBox)
                {
                    tbList.Add(item as TextBox);
                }
            }
            foreach (var item in TbPanel2.Children)
            {
                if (item is TextBox)
                {
                    tbList.Add(item as TextBox);
                }
            }
            Loaded -= UpLoadLocalProgramControl_Loaded;
        }

        private void InputTextBox(KeyCode keyCode)
        {
            if (FocusTb == null){
                if(Keyboard.FocusedElement is TextBox){
                    FocusTb = Keyboard.FocusedElement as TextBox;
                }else{
                    return;
                }
            }

            var number = 0;
            if (int.TryParse(keyCode.Code, out number)){
                FocusTb.Text += number;
            }else{
                if (keyCode.Code == "." && !FocusTb.Text.Contains(".")){
                    FocusTb.Text += keyCode.Code;
                }else{
                    if (FocusTb.Text.Length > 0){
                        FocusTb.Text = FocusTb.Text.Remove(FocusTb.Text.Length - 1, 1);
                    }
                }
            }
        }

        public TextBox FocusTb { get; set; }
        public List<TextBox> tbList { get; set; }
        private void NextOptionBtn_Click(object sender, RoutedEventArgs e)
        {
            if (FocusTb == null){
                FocusTb = tbList[0];
            }else if (tbList.IndexOf(FocusTb) + 1 < tbList.Count){
                FocusTb = tbList[tbList.IndexOf(FocusTb) + 1];
            }
            Keyboard.Focus(FocusTb);
        }

        private void LastOptionBtn_Click(object sender, RoutedEventArgs e){
            if (FocusTb == null)
            {
                FocusTb = tbList[0];
            }
            else if (tbList.IndexOf(FocusTb) - 1 >= 0)
            {
                FocusTb = tbList[tbList.IndexOf(FocusTb) - 1];
            }
            Keyboard.Focus(FocusTb);
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            FocusTb = sender as TextBox;
        }
    }
}
