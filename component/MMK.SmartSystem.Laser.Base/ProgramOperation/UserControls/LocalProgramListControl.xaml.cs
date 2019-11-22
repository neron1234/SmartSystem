using Abp.Events.Bus;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common.EventDatas;
using MMK.SmartSystem.Laser.Base.ProgramOperation.UserControls.ViewModel;
using MMK.SmartSystem.Laser.Base.ProgramOperation.ViewModel;
using MMK.SmartSystem.WebCommon.HubModel;
using Newtonsoft.Json.Linq;
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
    /// LocalProgramListControl.xaml 的交互逻辑
    /// </summary>
    public partial class LocalProgramListControl : UserControl, IProgramNotice
    {
        public LocalProgramListViewModel lpViewModel { get; set; }
        public event Action<HubReadWriterModel> RealReadWriterEvent;

        private ProgramDetailViewModel currentProgramDetail;

        private UpLoadLocalProgramControl modalControl;
        public LocalProgramListControl()
        {
            InitializeComponent();
            this.DataContext = lpViewModel = new LocalProgramListViewModel();
            lpViewModel.UploadClickEvent += LpViewModel_UploadClickEvent;
            lpViewModel.CheckedProgramEvent += CheckedLocalProgram;
            lpViewModel.PagePagingEvent += LpViewModel_PagePagingEvent;
            lpViewModel.DeleteProgramEvent += LpViewModel_DeleteProgramEvent;
            lpViewModel.EditProgramEvent += LpViewModel_EditProgramEvent;
            Loaded += LocalProgramListControl_Loaded;
            lpViewModel.Init();
        }

        private void LpViewModel_EditProgramEvent()
        {
            if (lpViewModel.SelectedProgramViewModel == null)
            {
                return;
            }
            //System.Diagnostics.Process.Start(@"Notepad.exe", System.IO.Path.Combine(this.Path, this.SelectedProgramViewModel.Name));
            var ep = new EditProgramStrControl(System.IO.Path.Combine(lpViewModel.Path, lpViewModel.SelectedProgramViewModel.Name));
            var popup = new PopupWindow(ep, 1000, 600, "编辑程序");
            popup.UserControlFinishEvent += Popup_UserControlFinishEvent;
            popup.ShowDialog();
        }
        private void Popup_UserControlFinishEvent()
        {
            Messenger.Default.Send(new Common.ViewModel.NotifiactionModel()
            {
                Title = "操作成功",
                Content = $"编辑本地程序【{lpViewModel.SelectedProgramViewModel.Name}】成功",
                NotifiactionType = Common.ViewModel.EnumPromptType.Success
            });
            ReadFileText();
        }

        private void LpViewModel_DeleteProgramEvent(string arg1, ProgramViewModel arg2)
        {

            string message = $"确定删除 【{arg1}】本地目录下的【{arg2.Name}】 程序吗？";
            var confirm = new ConfirmControl(message);
            var popup = new PopupWindow(confirm, 480, 180, "删除本地程序");
            confirm.ConfirmOkEvent += () =>
            {
                if (File.Exists(System.IO.Path.Combine(arg1, arg2.Name)))
                {
                    try
                    {
                        File.Delete(System.IO.Path.Combine(arg1, arg2.Name));
                        var obj = lpViewModel.LocalProgramList.FirstOrDefault(d=>d.FillName==arg2.FillName)??new ProgramViewModel();
                        lpViewModel.LocalProgramList.Remove(obj);
                        Messenger.Default.Send(new Common.ViewModel.NotifiactionModel()
                        {
                            Title = "删除本地程序",
                            Content = $"成功删除本地程序【{arg2.Name}】！",
                            NotifiactionType = Common.ViewModel.EnumPromptType.Success
                        });
                    }
                    catch (Exception)
                    {

                        Messenger.Default.Send(new Common.ViewModel.NotifiactionModel()
                        {
                            Title = "删除本地程序",
                            Content = $"删除本地程序【{arg2.Name}】出错,请稍后重试！",
                            NotifiactionType = Common.ViewModel.EnumPromptType.Error
                        });
                    }
                }
                popup.Close();
            };
            confirm.ConfirmCancelEvent += () => popup.Close();
            popup.ShowDialog();
            lpViewModel.RefreshPage();

        }

        private void LpViewModel_PagePagingEvent()
        {
            if (lpViewModel.LocalProgramList.Count > 0)
            {
                ProgramGrid.SelectedIndex = 0;
            }
        }
        private void LocalProgramListControl_Loaded(object sender, RoutedEventArgs e)
        {
            var tempTotal = lpViewModel.CurrentPage;
            lpViewModel.CurrentPage = 0;
            lpViewModel.CurrentPage = tempTotal;
        }

        public void CheckedLocalProgram()
        {

            foreach (var program in lpViewModel.ProgramList)
            {
                program.SetCommentDto(d => d.FileHash == program.FileHash);
            }
        }

        private void LpViewModel_UploadClickEvent(LocalProgramListViewModel local, ProgramViewModel obj)
        {
            Stream stream = default;
            using (var fileStream = new FileStream(System.IO.Path.Combine(local.Path, obj.Name), FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                byte[] bytes = new byte[fileStream.Length];
                fileStream.Read(bytes, 0, bytes.Length);
                fileStream.Close();
                stream = new MemoryStream(bytes);
            }

            Task.Factory.StartNew(new Action(() =>
            {
                EventBus.Default.TriggerAsync(new UpLoadProgramClientEventData
                {
                    FileParameter = new Common.FileParameter(stream, obj.Name),
                    ConnectId = local.ConnectId,
                    FileHashCode = obj.FileHash
                });
            }));
            modalControl = new UpLoadLocalProgramControl(local.Path, obj.FileHash);
            modalControl.ProgramUploadEvent += Modal_ProgramUploadEvent;
            new PopupWindow(modalControl, 900, 590, "上传本地程序").ShowDialog();
            modalControl = null;
        }

        private void Modal_ProgramUploadEvent(ProgramDetailViewModel obj)
        {
            currentProgramDetail = obj;
            RealReadWriterEvent?.Invoke(new HubReadWriterModel()
            {
                ProxyName = "ProgramTransferInOut",
                Action = "UploadProgramToCNC",
                Id = "uploadProgramToCNC",
                Data = new object[] { lpViewModel.SelectedProgramViewModel?.FillName, obj.SelectedProgramFolders.Folder }
            });
        }

        private void ProgramGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
            var selected = ((DataGrid)sender).SelectedValue;
            if (selected != null && selected is ProgramViewModel)
            {
                lpViewModel.SelectedProgramViewModel = (ProgramViewModel)selected;
                ReadFileText();
                #region 本地图形显示测试
                //Messenger.Default.Send(lpViewModel.SelectedProgramViewModel);

                //if (lpViewModel.SelectedProgramViewModel.Name.Split('.').Count() > 1)
                //{
                //    if (lpViewModel.SelectedProgramViewModel.Name.Split('.')[1] == "dxf")
                //    {
                //    }
                //    else if (lpViewModel.SelectedProgramViewModel.Name.Split('.')[1] == "csv")
                //    {
                //        System.IO.StreamReader reader = new System.IO.StreamReader(lpViewModel.Path + @"\" + lpViewModel.SelectedProgramViewModel.Name);
                //        string line = "";
                //        List<System.Windows.Point> pointList = new List<System.Windows.Point>();
                //        //List<string[]> pointList = new List<string[]>();
                //        StringBuilder sb1 = new StringBuilder();
                //        line = reader.ReadLine();
                //        while (line != null)
                //        {
                //            //pointList.Add(new System.Windows.Point(Convert.ToDouble(line.Split(',')[0]), Convert.ToDouble(line.Split(',')[1])));

                //            sb1.AppendLine("{'X': '" + Convert.ToDouble(line.Split(',')[0]) + "','Y': '" + Convert.ToDouble(line.Split(',')[1]) + "'},");
                //            //pointList.Add(line.Split(','));
                //            line = reader.ReadLine();
                //        }
                //    }
                //}
                #endregion
            }
        }

        private void ReadFileText()
        {
            StringBuilder sb = new StringBuilder();
            using (StreamReader reader = new StreamReader(lpViewModel.Path + @"\" + lpViewModel.SelectedProgramViewModel.Name))
            {
                var line = reader.ReadLine();
                for (int i = 0; i < 22; i++)
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

        public bool CanWork(HubReadWriterResultModel resultModel)
        {
            return resultModel.Id == "uploadProgramToCNC" || resultModel.Id == "ReadProgramEvent";
        }

        public void DoWork(HubReadWriterResultModel resultModel)
        {
            if (resultModel.Id == "ReadProgramEvent")
            {
                JObject jObject = JObject.Parse(resultModel.Result.ToString());
                if (jObject != null && modalControl != null)
                {
                    var obj = new ProgramDetailViewModel
                    {
                        Name = jObject["name"]?.ToString(),
                        FullPath = jObject["fullPath"]?.ToString(),
                        Size = Convert.ToDouble(jObject["size"]?.ToString()),
                        Material = jObject["material"]?.ToString(),
                        Thickness = Convert.ToDouble(jObject["thickness"] ?? "0"),
                        Gas = jObject["gas"]?.ToString(),
                        FocalPosition = Convert.ToDouble(jObject["focalPosition"] ?? "0"),
                        NozzleKind = jObject["nozzleKind"]?.ToString(),
                        NozzleDiameter = Convert.ToDouble(jObject["nozzleDiameter"] ?? "0"),
                        PlateSizeHeight = Convert.ToDouble(jObject["plateSize_H"] ?? "0"),
                        UsedPlateSizeHeigth = Convert.ToDouble(jObject["usedPlateSize_H"] ?? "0"),
                        PlateSizeWidth = Convert.ToDouble(jObject["plateSize_W"] ?? "0"),
                        UsedPlateSizeWidth = Convert.ToDouble(jObject["usedPlateSize_W"] ?? "0"),
                        CuttingDistance = Convert.ToDouble(jObject["cuttingDistance"] ?? "0"),
                        PiercingCount = Convert.ToInt32(jObject["piercingCount"] ?? "0"),
                        Max_X = Convert.ToInt32(jObject["max_X"] ?? "0"),
                        Max_Y = Convert.ToInt32(jObject["max_Y"] ?? "0"),
                        Min_X = Convert.ToInt32(jObject["min_X"] ?? "0"),
                        Min_Y = Convert.ToInt32(jObject["min_Y"] ?? "0"),
                        CuttingTime = Convert.ToInt32(jObject["cuttingTime"] ?? "0"),
                        ThumbnaiType = Convert.ToInt32(jObject["thumbnaiType"] ?? "0"),
                        ThumbnaiInfo = jObject["thumbnaiInfo"]?.ToString()
                    };
                    modalControl.SetSelectProgramDetail(obj);
                }
                return;
            }
            string name = resultModel.Result.ToString();
            Task.Factory.StartNew(new Action(() =>
            {
                EventBus.Default.Trigger(new UpdateProgramClientEventData()
                {
                    Data = new Common.UpdateProgramDto()
                    {
                        FileHash = currentProgramDetail?.FileHashCode,
                        CuttingDistance = Convert.ToDouble(currentProgramDetail?.CuttingDistance),
                        CuttingTime = Convert.ToDouble(currentProgramDetail?.CuttingTime),
                        FocalPosition = Convert.ToDouble(currentProgramDetail?.FocalPosition),
                        FullPath = currentProgramDetail?.SelectedProgramFolders.Folder,
                        Gas = currentProgramDetail.Gas,
                        Material = currentProgramDetail.Material,
                        Name = name,
                        NozzleDiameter = currentProgramDetail.NozzleDiameter,
                        NozzleKind = currentProgramDetail.NozzleKind,
                        PiercingCount = currentProgramDetail.PiercingCount,
                        Size = currentProgramDetail.Size,
                        Thickness = currentProgramDetail.Thickness,
                        UpdateTime = DateTime.Now,
                        Max_X = currentProgramDetail.Max_X,
                        Max_Y = currentProgramDetail.Max_Y,
                        Min_X = currentProgramDetail.Min_X,
                        Min_Y = currentProgramDetail.Min_Y,
                        PlateSize_H = currentProgramDetail.PlateSizeHeight,
                        PlateSize_W = currentProgramDetail.PlateSizeWidth,
                        UsedPlateSize_H = currentProgramDetail.UsedPlateSizeHeigth,
                        UsedPlateSize_W = currentProgramDetail.UsedPlateSizeWidth,
                        ThumbnaiInfo = currentProgramDetail.ThumbnaiInfo,
                        ThumbnaiType = currentProgramDetail.ThumbnaiType
                    }
                });
            }));
            Messenger.Default.Send(new PopupMsg
            {
                IsClose = true
            });
            this.lpViewModel.LocalProgramList.FirstOrDefault(n => n.FileHash == currentProgramDetail?.FileHashCode)?.SetProgramLoad(name);
            Messenger.Default.Send(new Common.ViewModel.NotifiactionModel()
            {
                Title = "上传成功",
                Content = $"程序{name}上传成功！" + DateTime.Now,
                NotifiactionType = Common.ViewModel.EnumPromptType.Success
            });
        }
    }
}
