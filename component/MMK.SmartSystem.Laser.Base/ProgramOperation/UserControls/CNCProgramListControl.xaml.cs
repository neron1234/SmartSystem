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
            return resultModel.Id == "getProgramList";
        }

        public void DoWork(HubReadWriterResultModel resultModel)
        {
            JArray jArray = JArray.Parse(resultModel.Result.ToString());
            ReadProgramList(jArray);
        }
    }
}
