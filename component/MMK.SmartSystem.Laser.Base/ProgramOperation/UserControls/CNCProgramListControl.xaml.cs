using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Laser.Base.ProgramOperation.UserControls.ViewModel;
using MMK.SmartSystem.Laser.Base.ProgramOperation.ViewModel;
using MMK.SmartSystem.WebCommon.HubModel;
using Newtonsoft.Json.Linq;
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
    public partial class CNCProgramListControl : UserControl, IProgramNotice
    {
        CNCProgramListViewModel cpViewModel;
        public event Action<HubReadWriterModel> RealReadWriterEvent;

        public CNCProgramListControl()
        {

            InitializeComponent();
            this.DataContext = cpViewModel = new CNCProgramListViewModel();
            cpViewModel.CNCPath = ProgramConfigConsts.CNCPath;
            cpViewModel.SetCNCProgramPath += CpViewModel_SetCNCProgramPath;
            cpViewModel.MainCommandEvent += CpViewModel_MainCommandEvent;
            cpViewModel.DeleteProgramEvent += CpViewModel_DeleteProgramEvent;
            cpViewModel.DownProgramEvent += CpViewModel_DownProgramEvent;
            cpViewModel.PagePagingEvent += CpViewModel_PagePagingEvent;
        }

        private void CpViewModel_PagePagingEvent()
        {
            if (cpViewModel.LocalProgramList.Count > 0)
            {

                ProgramGrid.SelectedIndex = 0;
            }
        }

        private void CpViewModel_DownProgramEvent()
        {

            System.Windows.Forms.FolderBrowserDialog folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var path = folderDialog.SelectedPath.Trim();
                string savaFull = System.IO.Path.Combine(path, cpViewModel.CurrentSelectModel.Name);
                RealReadWriterEvent?.Invoke(new HubReadWriterModel()
                {
                    ProxyName = "ProgramTransferInOut",
                    Action = "DownloadProgram",
                    Id = "downloadProgram",
                    SuccessTip = $"成功下载【{cpViewModel.CNCPath}】目录【{cpViewModel.CurrentSelectModel.Name}】 程序到本地目录【{savaFull}】！",
                    Data = new object[] { $"{cpViewModel.CNCPath}{cpViewModel.CurrentSelectModel.Name}", $"{savaFull}" }
                });
            }
           

        }


        private void CpViewModel_DeleteProgramEvent()
        {
            if (cpViewModel.CurrentSelectModel != null)
            {
                string message = $"确定删除 【{cpViewModel.CNCPath}】目录下的【{cpViewModel.CurrentSelectModel.Name}】 程序吗？";
                var confirm = new ConfirmControl(message);
                var popup = new PopupWindow(confirm, 480, 180, "删除CNC程序");
                confirm.ConfirmOkEvent += () =>
                {
                    RealReadWriterEvent?.Invoke(new HubReadWriterModel()
                    {
                        ProxyName = "ProgramTransferInOut",
                        Action = "DeleteProgram",
                        Id = "deleteProgram",
                        SuccessTip = $"成功删除 【{cpViewModel.CNCPath}】目录【{cpViewModel.CurrentSelectModel.Name}】 程序！",
                        Data = new object[] { $"{cpViewModel.CNCPath}{cpViewModel.CurrentSelectModel.Name}" }
                    });
                    popup.Close();
                };
                confirm.ConfirmCancelEvent += () => popup.Close();
                popup.ShowDialog();

            }
        }

        private void CpViewModel_MainCommandEvent()
        {
            if (cpViewModel.CurrentSelectModel != null)
            {
                string message = $"确定设置 【{cpViewModel.CNCPath}】目录下的【{cpViewModel.CurrentSelectModel.Name}】 为主程序吗？";
                var confirm = new ConfirmControl(message);
                var popup = new PopupWindow(confirm, 480, 180, "设置主程序");
                confirm.ConfirmOkEvent += () =>
                {
                    RealReadWriterEvent?.Invoke(new HubReadWriterModel()
                    {
                        ProxyName = "ProgramTransferInOut",
                        Action = "MainProgramToCNC",
                        Id = "mainProgramToCNC",
                        SuccessTip = $"成功设置 【{cpViewModel.CNCPath}】目录【{cpViewModel.CurrentSelectModel.Name}】 程序为当前CNC主程序！",

                        Data = new object[] { $"{cpViewModel.CNCPath}{cpViewModel.CurrentSelectModel.Name}" }
                    });
                    popup.Close();
                };
                confirm.ConfirmCancelEvent += () => popup.Close();
                popup.ShowDialog();
            }
        }

        private void CpViewModel_SetCNCProgramPath()
        {
            var cncPath = new CNCPathControl(ProgramConfigConsts.CurrentReadProgramFolder);
            cncPath.SaveCNCPathEvent += CncPath_SaveCNCPathEvent;
            new PopupWindow(cncPath, 680, 220, "修改CNC路径").ShowDialog();
        }

        private void CncPath_SaveCNCPathEvent(CNCProgramPath obj)
        {
            cpViewModel.CNCPath = obj.Path;
            cpViewModel.Clear();
            Messenger.Default.Send(new ProgramViewModel());

            RealReadWriterEvent?.Invoke(new HubReadWriterModel()
            {
                ProxyName = "ProgramListInOut",
                Action = "Reader",
                Id = "getProgramList",
                Data = new object[] { cpViewModel.CNCPath }
            });
        }

        private void ProgramGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = ((DataGrid)sender).SelectedValue;
            if (selected != null && selected is ProgramViewModel)
            {
                cpViewModel.CurrentSelectModel = (ProgramViewModel)selected;
                Messenger.Default.Send(cpViewModel.CurrentSelectModel);
            }
            e.Handled = true;
        }
        public void Init()
        {
            RealReadWriterEvent?.Invoke(new HubReadWriterModel()
            {
                ProxyName = "ProgramListInOut",
                Action = "Reader",
                Id = "getProgramList",
                Data = new object[] { cpViewModel.CNCPath }
            });

        }
        private void ReadProgramList(JArray array)
        {
            cpViewModel.CurrentSelectModel = null;
            this.cpViewModel.LocalProgramList = new List<ProgramViewModel>();
            foreach (var item in array)
            {
                JObject jObject = JObject.Parse(item.ToString());
                if (jObject != null)
                {
                    this.cpViewModel.LocalProgramList.Add(new ProgramViewModel
                    {
                        Name = jObject["name"].ToString(),
                        Size = jObject["size"].ToString(),
                        CreateTime = Convert.ToDateTime(jObject["createDateTime"]).ToString("yyyy-MM-dd HH:mm:ss"),
                        Description = jObject["description"].ToString()
                    });
                }
            }
            cpViewModel.LocalProgramList.ForEach(d => d.SetCommentDto(f => f.Name == d.Name && f.FullPath == cpViewModel.CNCPath));
            cpViewModel.DataPaging();

        }

        public bool CanWork(HubReadWriterResultModel resultModel)
        {
            string[] arr = { "deleteProgram", "getProgramList", "mainProgramToCNC", "downloadProgram" };
            return arr.Contains(resultModel.Id);
        }

        public void DoWork(HubReadWriterResultModel resultModel)
        {
            if (resultModel.Id == "getProgramList")
            {
                JArray jArray = JArray.Parse(resultModel.Result.ToString());
                ReadProgramList(jArray);

                return;
            }
            Messenger.Default.Send(new Common.ViewModel.NotifiactionModel()
            {
                Title = "操作成功",
                Content = resultModel.SuccessTip,
                NotifiactionType = Common.ViewModel.EnumPromptType.Success
            });
        }
    }
}
