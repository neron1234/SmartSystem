using Abp.Dependency;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common.Base;
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
        public ProgramListPage()
        {
            InitializeComponent();
            this.DataContext = programListViewModel = new ProgramListViewModel();

            programListViewModel.CNCPath = new CNCProgramPath("//CNC_MEM/USER/PATH1/","UserControl");
            programListViewModel.ListControl = new CNCProgramListControl(programListViewModel.ProgramFolder);
            programListViewModel.InfoControl = new CNCProgramInfoControl();
            Messenger.Default.Send(programListViewModel.CNCPath);
            Messenger.Default.Register<CNCProgramPath>(this, (cncPath) => {
                if (cncPath.Page == "Page")
                {
                    programListViewModel.CNCPath = cncPath;
                    SendQurayProgramList(true);
                }
            });
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

        protected async override void SignalrProxyClient_HubReaderWriterResultEvent(HubReadWriterResultModel obj)
        {
            if (obj.Id == "getProgramList") {
                if (!obj.Success){
                    this.programListViewModel.CNCProgramViews = new List<ProgramViewModel>();
                    if (ChangePathByQurayProgramList)
                    {
                        Messenger.Default.Send(this.programListViewModel.CNCProgramViews);
                    }
                    return;
                }
                //读取CNC程序列表
                JArray jArray = JArray.Parse(obj.Result.ToString());
                this.programListViewModel.CNCProgramViews = new List<ProgramViewModel>();
                foreach (var item in jArray){
                    JObject jObject = JObject.Parse(item.ToString());
                    if (jObject != null){
                        this.programListViewModel.CNCProgramViews.Add(new ProgramViewModel{
                            Name = jObject["name"].ToString(),
                            Size = jObject["size"].ToString(),
                            CreateTime = jObject["createDateTime"].ToString(),
                            Description = jObject["description"].ToString()
                        });
                    }
                }
                Messenger.Default.Send(this.programListViewModel.CNCProgramViews);
            }else if (obj.Id == "getProgramFolder"){
                if (!obj.Success){
                    return;
                }
                //读取CNC路径
                JObject jObject = JObject.Parse(obj.Result.ToString());
                ReadProgramFolderItemViewModel readProgramFolder = new ReadProgramFolderItemViewModel();
                if (jObject != null){
                    readProgramFolder.RegNum = (int)jObject["regNum"];
                    readProgramFolder.Name = jObject["name"].ToString();
                    readProgramFolder.Folder = jObject["folder"].ToString();
                    var jArray = JArray.Parse(jObject["nodes"].ToString());

                    await Task.Run(new Action(() => {
                        ReadProgramFolderNode(jArray, readProgramFolder);
                    })); 

                    Messenger.Default.Send(readProgramFolder);
                    programListViewModel.ProgramFolder = readProgramFolder;
                }
            }else{
                if (!obj.Success){
                    return;
                }
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
                        PlateSize = jObject["plateSize"].ToString(),
                        UsedPlateSize = jObject["usedPlateSize"].ToString(),
                        CuttingDistance = Convert.ToDouble(jObject["cuttingDistance"]),
                        PiercingCount = Convert.ToInt32(jObject["piercingCount"])
                    });
                }
            }
        }

        private void ReadProgramFolderNode(JArray jArray, ReadProgramFolderItemViewModel node)
        {
            if (jArray == null) return;

            node.Nodes = new System.Collections.ObjectModel.ObservableCollection<ReadProgramFolderItemViewModel>();

            Task.Run(() => ReadProgramFolderNode(jArray, node));
            Parallel.ForEach(jArray, item => {
                var childNode = new ReadProgramFolderItemViewModel
                {
                    RegNum = (int)item["regNum"],
                    Name = item["name"].ToString(),
                    Folder = item["folder"].ToString(),
                };
                node.Nodes.Add(childNode);
                ReadProgramFolderNode(JArray.Parse(item["nodes"].ToString()), childNode);
            });
        }

        public override bool IsRequestResponse => true;

        public override List<object> GetResultViewModelMap()
        {
            return default;
        }

        public override void CncOnError(string message)
        {
        }

        private void CNCProgramListBtn_Click(object sender, RoutedEventArgs e)
        {
            programListViewModel.ListControl = new CNCProgramListControl(programListViewModel.ProgramFolder);
            programListViewModel.InfoControl = new CNCProgramInfoControl();
            Messenger.Default.Send(programListViewModel.CNCPath);
            SendQurayProgramList();
        }
    }
}
