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
using MMK.SmartSystem.WebCommon.HubModel;
using Newtonsoft.Json.Linq;

namespace MMK.SmartSystem.Laser.Base.ProgramOperation.UserControls
{
    /// <summary>
    /// CNCInfoControl.xaml 的交互逻辑
    /// </summary>
    public partial class CNCInfoControl : UserControl, IProgramNotice
    {
        public CNCInfoControl()
        {
            InitializeComponent();
        }

        public event Action<HubReadWriterModel> RealReadWriterEvent;

        public void Init()
        {
            RealReadWriterEvent?.Invoke(new HubReadWriterModel()
            {
                ProxyName = "ProgramTransferInOut",
                Action = "ReadProgramInfo",
                Id = "readProgramInfo",
                Data = new object[] { }

            });
        }
        public bool CanWork(HubReadWriterResultModel resultModel)
        {
            return resultModel.Id == "readProgramInfo";
        }

        public void DoWork(HubReadWriterResultModel resultModel)
        {
            JObject jObject2 = JObject.Parse(resultModel.Result.ToString());

        }
    }
}
