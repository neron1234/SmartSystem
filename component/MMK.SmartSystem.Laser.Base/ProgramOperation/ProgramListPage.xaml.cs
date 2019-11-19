using Abp.Dependency;
using Abp.Events.Bus;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common.Base;
using MMK.SmartSystem.Common.EventDatas;
using MMK.SmartSystem.Common.ViewModel;
using MMK.SmartSystem.Laser.Base.CustomControl;
using MMK.SmartSystem.Laser.Base.ProgramOperation.UserControls;
using MMK.SmartSystem.Laser.Base.ProgramOperation.UserControls.ViewModel;
using MMK.SmartSystem.Laser.Base.ProgramOperation.ViewModel;
using MMK.SmartSystem.WebCommon.HubModel;
using netDxf;
using netDxf.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace MMK.SmartSystem.Laser.Base.ProgramOperation
{
    /// <summary>
    /// ProgramListPage.xaml 的交互逻辑
    /// </summary>
    public partial class ProgramListPage : SignalrPage
    {
        private ProgramListViewModel programListViewModel { get; set; }
        private ProgramViewModel currentLocalProgram;
        private ProgramDetailViewModel currentProgramDetail;
        public ProgramListPage()
        {
            InitializeComponent();
            this.DataContext = programListViewModel = new ProgramListViewModel();

            programListViewModel.CNCPath = new CNCProgramPath("//CNC_MEM/USER/PATH1/", "UserControl");
            //programListViewModel.ListControl = new CNCProgramListControl(programListViewModel.ProgramFolder);
            programListViewModel.InfoControl = new CNCProgramInfoControl();

            MyCNCProgramListControl.cpViewModel.CNCPath = programListViewModel.CNCPath.Path;
            MyCNCProgramListControl.cpViewModel.SetCNCProgramPath += CpViewModel_SetCNCProgramPath;

            MyLocalProgramListControl.lpViewModel.ProgramFolderList = programListViewModel.ProgramFolder;
            MyLocalProgramListControl.lpViewModel.ConnectId = programListViewModel.ConnectId;
            MyLocalProgramListControl.UploadEvent += MyLocalProgramListControl_UploadEvent;
            MyLocalProgramListControl.ProgramSelectEvent += MyLocalProgramListControl_ProgramSelectEvent;
        }

        private void MyLocalProgramListControl_ProgramSelectEvent(ProgramViewModel obj)
        {
            currentLocalProgram = obj;
        }

        private void CpViewModel_SetCNCProgramPath()
        {
            var cncPath = new CNCPathControl(programListViewModel.ProgramFolder);
            cncPath.SaveCNCPathEvent += CncPath_SaveCNCPathEvent;
            new PopupWindow(cncPath, 680, 220, "修改CNC路径").ShowDialog();
        }
        private void MyLocalProgramListControl_UploadEvent(Common.EventDatas.UpLoadProgramClientEventData arg1, LocalProgramListViewModel local)
        {
            Task.Factory.StartNew(new Action(() =>
            {
                EventBus.Default.TriggerAsync(arg1);
            }));
            var modal = new UpLoadLocalProgramControl(local.Path, local.ProgramFolderList);

            modal.ProgramUploadEvent += Modal_ProgramUploadEvent;
            new PopupWindow(modal, 900, 590, "上传本地程序").ShowDialog();

        }

        private void Modal_ProgramUploadEvent(ProgramDetailViewModel obj)
        {
            currentProgramDetail = obj;
            SendReaderWriter(new HubReadWriterModel()
            {
                ProxyName = "ProgramTransferInOut",
                Action = "UploadProgramToCNC",
                Id = "uploadProgramToCNC",
                Data = new object[] { currentLocalProgram?.FillName, obj.SelectedProgramFolders.Folder }
            });
        }
        private void CncPath_SaveCNCPathEvent(CNCProgramPath obj)
        {
            programListViewModel.CNCPath = obj;
            MyCNCProgramListControl.cpViewModel.CNCPath = obj.Path;
            SendQurayProgramList(true);
        }

        protected override void PageSignlarLoaded()
        {
            SendQurayProgramList();
            SendReaderWriter(new HubReadWriterModel()
            {
                ProxyName = "ProgramFolderInOut",
                Action = "Reader",
                Id = "getProgramFolder",
                Data = new object[] { "//CNC_MEM/" }
            });
            programListViewModel.ConnectId = this.CurrentConnectId;
        }

        private bool ChangePathByQurayProgramList = false;
        private void SendQurayProgramList(bool ChangePath = false)
        {
            ChangePathByQurayProgramList = ChangePath;
            SendReaderWriter(new HubReadWriterModel()
            {
                ProxyName = "ProgramListInOut",
                Action = "Reader",
                Id = "getProgramList",
                Data = new object[] { programListViewModel.CNCPath.Path }
            });
        }

        protected override void SignalrProxyClient_HubReaderWriterResultEvent(HubReadWriterResultModel obj)
        {
            if (!obj.Success)
            {
                return;
            }
            switch (obj.Id)
            {
                case "getProgramList":
                    JArray jArray = JArray.Parse(obj.Result.ToString());
                    this.MyCNCProgramListControl.ReadProgramList(jArray);
                    break;
                case "getProgramFolder":
                    JObject jObject2 = JObject.Parse(obj.Result.ToString());
                    GetProgramFolder(jObject2);
                    break;
                case "uploadProgramToCNC":
                    string name = obj.Result.ToString();
                    Task.Factory.StartNew(new Action(() =>
                    {
                        EventBus.Default.Trigger(new UpdateProgramClientEventData()
                        {
                            Data = new Common.UpdateProgramDto()
                            {

                                CuttingDistance = currentProgramDetail?.CuttingDistance,
                                CuttingTime = currentProgramDetail.CuttingTime,
                                FocalPosition = currentProgramDetail.FocalPosition,
                                FullPath = currentProgramDetail.FullPath,
                                Gas = currentProgramDetail.Gas,
                                Material = currentProgramDetail.Material,
                                Name = currentProgramDetail.Name,
                                NozzleDiameter = currentProgramDetail.NozzleDiameter,
                                NozzleKind = currentProgramDetail.NozzleKind,
                                PiercingCount = currentProgramDetail.PiercingCount,
                                Size = currentProgramDetail.Size,
                                Thickness = currentProgramDetail.Thickness,
                                
                            }
                        });

                    }));
                    break;
                default:
                    //读取上传到服务器的本地程序解析结果（CNC还未上传）
                    JObject jObject = JObject.Parse(obj.Result.ToString());
                    if (jObject != null)
                    {
                        Messenger.Default.Send(new ProgramDetailViewModel
                        {
                            Name = jObject["name"].ToString(),
                            FullPath = jObject["fullPath"].ToString(),
                            Size = Convert.ToDouble(jObject["size"].ToString()),
                            Material = jObject["material"].ToString(),
                            Thickness = Convert.ToDouble(jObject["thickness"]),
                            Gas = jObject["gas"].ToString(),
                            FocalPosition = Convert.ToDouble(jObject["focalPosition"]),
                            NozzleKind = jObject["nozzleKind"].ToString(),
                            NozzleDiameter = Convert.ToDouble(jObject["nozzleDiameter"]),
                            PlateSizeHeight = jObject["plateSizeHeight"].ToString(),
                            UsedPlateSizeHeigth = jObject["usedPlateSizeHeigth"].ToString(),
                            PlateSizeWidth = jObject["plateSizeWidth"].ToString(),
                            UsedPlateSizeWidth = jObject["usedPlateSizeWidth"].ToString(),
                            CuttingDistance = Convert.ToDouble(jObject["cuttingDistance"]),
                            PiercingCount = Convert.ToInt32(jObject["piercingCount"])
                        });
                    }
                    break;
            };
        }

        private void GetProgramFolder(JObject jObject)
        {
            ReadProgramFolderItemViewModel readProgramFolder = new ReadProgramFolderItemViewModel();
            if (jObject != null)
            {
                readProgramFolder.RegNum = (int)jObject["regNum"];
                readProgramFolder.Name = jObject["name"].ToString();
                readProgramFolder.Folder = jObject["folder"].ToString();
                var jArray = JArray.Parse(jObject["nodes"].ToString());

                ReadProgramFolderNode(jArray, readProgramFolder);

                MyLocalProgramListControl.lpViewModel.ProgramFolderList = readProgramFolder;
                programListViewModel.ProgramFolder = readProgramFolder;
            }
        }

        private void ReadProgramFolderNode(JArray jArray, ReadProgramFolderItemViewModel node)
        {
            if (jArray == null) return;

            node.Nodes = new System.Collections.ObjectModel.ObservableCollection<ReadProgramFolderItemViewModel>();
            foreach (var item in jArray)
            {
                var childNode = new ReadProgramFolderItemViewModel
                {
                    RegNum = (int)item["regNum"],
                    Name = item["name"].ToString(),
                    Folder = item["folder"].ToString(),
                };
                node.Nodes.Add(childNode);
                ReadProgramFolderNode(JArray.Parse(item["nodes"].ToString()), childNode);
            }
        }

        public override bool IsRequestResponse => true;

        public override List<object> GetResultViewModelMap()
        {
            return default;
        }

        public override void CncOnError(string message)
        {
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (TabItem)((TabControl)sender).SelectedItem;
            switch (item.Header)
            {
                case "CNC程序":
                    programListViewModel.InfoControl = new CNCProgramInfoControl();
                    break;
                case "本地程序":
                    programListViewModel.InfoControl = new LocalProgramInfoControl();
                    break;
                case "CNC信息":
                    programListViewModel.InfoControl = new UserControl();
                    break;
                default:
                    break;
            }
        }
    }
}
