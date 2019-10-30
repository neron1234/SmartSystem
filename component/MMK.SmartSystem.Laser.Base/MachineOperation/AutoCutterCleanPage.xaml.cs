using Abp.Dependency;
using MMK.SmartSystem.Common.Base;
using MMK.SmartSystem.Common.ViewModel;
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
using MMK.SmartSystem.WebCommon.DeviceModel;
using MMK.SmartSystem.Common.SignalrProxy;
using MMK.SmartSystem.Common.Model;

namespace MMK.SmartSystem.Laser.Base.MachineOperation
{
    /// <summary>
    /// AutoCutterCleanPage.xaml 的交互逻辑
    /// </summary>
    public partial class AutoCutterCleanPage : SignalrPage
    {
        /// <summary>
        /// 割嘴自动清理
        /// </summary>
        public AutoCutterCleanPage()
        {
            InitializeComponent();
            //this.DataContext = new MainTranslateViewModel();
            manualControl.SetHeaderActive(this);
        }
      

        public override void CncOnError(string message)
        {
            
        }

        public override List<CncEventData> GetCncEventData()
        {
            List<CncEventData> cncEventDatas = new List<CncEventData>();
            cncEventDatas.Add(new CncEventData()
            {
                Kind = CncEventEnum.ReadWorkpartNum,
                Para = Newtonsoft.Json.JsonConvert.SerializeObject(new ReadWorkpartNumModel() { 
                    Decompilers = new List<string>()
                    {
                        "YD",
                        "XD"
                    },
                    Readers = new List<string>()
                    {
                        
                    }
                })
            });
            cncEventDatas.Add(new CncEventData()
            {
                Kind = CncEventEnum.ReadMacro,
                Para = Newtonsoft.Json.JsonConvert.SerializeObject(new ReadMacroModel()
                {
                    Decompilers = new List<DecompReadMacroItemModel>(){
                       new DecompReadMacroItemModel()
                       {
                           Id = "H"
                       },
                       new DecompReadMacroItemModel()
                       {
                           Id = "CleanTime"
                       },
                       new DecompReadMacroItemModel()
                       {
                           Id = "ZLimit"
                       }
                    },
                    Readers = new List<ReadMacroTypeModel>()
                    {
                        new ReadMacroTypeModel(){ Quantity=10,StartNum=813},
                        new ReadMacroTypeModel(){ Quantity=10,StartNum=837},
                        new ReadMacroTypeModel(){ Quantity=10,StartNum=838}
                    }
                })
            });
            return cncEventDatas;
        }

        public override List<object> GetResultViewModelMap()
        {
            return default;
        }
    }
}
