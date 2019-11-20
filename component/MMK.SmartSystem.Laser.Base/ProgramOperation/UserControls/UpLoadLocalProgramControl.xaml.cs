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
using Newtonsoft.Json.Linq;

namespace MMK.SmartSystem.Laser.Base.ProgramOperation.UserControls
{
    /// <summary>
    /// EditProgramControl.xaml 的交互逻辑
    /// </summary>
    public partial class UpLoadLocalProgramControl : UserControl
    {
        public UpLoadLocalProgramViewModel upLoadProViewModel { get; set; }

        public event Action<ProgramDetailViewModel> ProgramUploadEvent;
        private string fileHash = "";
        public UpLoadLocalProgramControl(string programPath, string fileCode = "")
        {
            InitializeComponent();
            fileHash = fileCode;
            this.DataContext = upLoadProViewModel = new UpLoadLocalProgramViewModel();
            upLoadProViewModel.LocalProgramPath = programPath;
            upLoadProViewModel.CloseEvent += UpLoadProViewModel_CloseEvent;
            upLoadProViewModel.GetDetailModelEvent += UpLoadProViewModel_GetDetailModelEvent;
            upLoadProViewModel.InputKeyInputEvent += UpLoadProViewModel_InputKeyInputEvent;
            Loaded += UpLoadLocalProgramControl_Loaded;
            //this.CNCPathCascader.SelectedItem = upLoadProViewModel.SelectedProgramFolders;
        }

        public void SetSelectProgramDetail(ProgramDetailViewModel pds)
        {
            upLoadProViewModel.ProgramDetail = pds;

            if (!string.IsNullOrEmpty(pds.Material))
            {
                upLoadProViewModel.SelectedMaterialId = (int)upLoadProViewModel.MaterialTypeList.FirstOrDefault(n => n.Name_CN == pds.Material)?.Code;
            }
            if (!string.IsNullOrEmpty(pds.NozzleKind))
            {
                upLoadProViewModel.SelectedNozzleKindCode = (int)upLoadProViewModel.NozzleKindList.FirstOrDefault(n => n.Name_CN == pds.NozzleKind)?.Code;
            }

        }
        private void UpLoadProViewModel_InputKeyInputEvent(string obj)
        {
            InputTextBox(obj);
        }

        private void UpLoadProViewModel_GetDetailModelEvent(ProgramDetailViewModel obj)
        {
            obj.FileHashCode = fileHash;
            obj.SelectedProgramFolders = ((ReadProgramFolderItemViewModel)this.CNCPathCascader.SelectedValues[this.CNCPathCascader.SelectedValues.Count - 1]);
            ProgramUploadEvent?.Invoke(obj);
        }

        private void UpLoadProViewModel_CloseEvent()
        {
            Messenger.Default.Send(new PopupMsg() { IsClose = true });
        }

        private void UpLoadLocalProgramControl_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(new Action(() =>
            {
                EventBus.Default.TriggerAsync(new NozzleKindEventData
                {
                    SuccessAction = (nzList) =>
                    {
                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            upLoadProViewModel.NozzleKindList.Clear();
                            nzList.ForEach(d => upLoadProViewModel.NozzleKindList.Add(d));

                            if (upLoadProViewModel.NozzleKindList.Count > 0)
                            {
                                upLoadProViewModel.SelectedNozzleKindCode = (int)upLoadProViewModel.NozzleKindList.First()?.Code;
                            }
                        }));
                    }
                });
                EventBus.Default.TriggerAsync(new MaterialInfoEventData
                {
                    IsCheckSon = false,
                    SuccessAction = ((mtList) =>
                    {
                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            upLoadProViewModel.MaterialTypeList.Clear();
                            mtList.ForEach(d => upLoadProViewModel.MaterialTypeList.Add(new MaterialDto()
                            {

                                Code = d.MaterialCode,
                                Id = d.Code,
                                Name_CN = d.Name_CN,
                                Name_EN = d.Name_EN
                            }));

                            if (upLoadProViewModel.MaterialTypeList.Count > 0)
                            {
                                upLoadProViewModel.SelectedMaterialId = (int)upLoadProViewModel.MaterialTypeList.First()?.Code;
                            }
                        }));
                    })
                });
            }));


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

        private void InputTextBox(string keyCode)
        {
            if (FocusTb == null)
            {
                if (Keyboard.FocusedElement is TextBox)
                {
                    FocusTb = Keyboard.FocusedElement as TextBox;
                }
                else
                {
                    return;
                }
            }

            var number = 0;

            string name = FocusTb.Name;


            if (int.TryParse(keyCode, out number))
            {
                FocusTb.Text += number;
            }
            else
            {
                if (keyCode == "." && !FocusTb.Text.Contains("."))
                {
                    FocusTb.Text += keyCode;
                }
                else
                {
                    if (FocusTb.Text.Length > 0)
                    {
                        FocusTb.Text = FocusTb.Text.Remove(FocusTb.Text.Length - 1, 1);
                    }
                }
            }
            if (!string.IsNullOrEmpty(name))
            {
                var propName = upLoadProViewModel.ProgramDetail.GetType().GetProperty(name);
                if (propName != null)
                {
                    propName.SetValue(upLoadProViewModel.ProgramDetail, Convert.ChangeType(FocusTb.Text, propName.PropertyType));
                }
            }
        }

        public TextBox FocusTb { get; set; }
        public List<TextBox> tbList { get; set; }
        private void NextOptionBtn_Click(object sender, RoutedEventArgs e)
        {
            if (FocusTb == null)
            {
                FocusTb = tbList[0];
            }
            else if (tbList.IndexOf(FocusTb) + 1 < tbList.Count)
            {
                FocusTb = tbList[tbList.IndexOf(FocusTb) + 1];
            }
            Keyboard.Focus(FocusTb);
        }

        private void LastOptionBtn_Click(object sender, RoutedEventArgs e)
        {
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
