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
                Messenger.Default.Send((ProgramViewModel)selected);
            }
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

            RealReadWriterEvent?.Invoke(new HubReadWriterModel()
            {
                ProxyName = "ProgramFolderInOut",
                Action = "Reader",
                Id = "getProgramFolder",
                Data = new object[] { "//CNC_MEM/" }
            });

        }
        private void ReadProgramList(JArray array)
        {
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
            cpViewModel.DataPaging();
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
                ProgramConfigConsts.CurrentReadProgramFolder = readProgramFolder;               
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
        public bool CanWork(HubReadWriterResultModel resultModel)
        {
            return resultModel.Id == "getProgramList" || resultModel.Id == "getProgramFolder";
        }

        public void DoWork(HubReadWriterResultModel resultModel)
        {
            if (resultModel.Id == "getProgramList")
            {
                JArray jArray = JArray.Parse(resultModel.Result.ToString());
                ReadProgramList(jArray);
                return;
            }
            JObject jObject2 = JObject.Parse(resultModel.Result.ToString());
            GetProgramFolder(jObject2);
        }
    }
}
