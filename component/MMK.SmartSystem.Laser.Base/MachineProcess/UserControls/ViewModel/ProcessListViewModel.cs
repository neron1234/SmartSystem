using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Laser.Base.MachineProcess.UserControls.ViewModel
{
    public class ProcessListViewModel : ViewModelBase
    {
        public ObservableCollection<object> PageListData { get; set; } = new ObservableCollection<object>();

        public Dictionary<string, string> ColumnArray { get; set; }

        public ProcessListViewModel()
        {
            ColumnArray = new Dictionary<string, string>();
            ColumnArray.Add("ENo", "E编号");
            ColumnArray.Add("MachiningKindName", "加工类型");
            ColumnArray.Add("MaterialName", "材料类型");
            ColumnArray.Add("MaterialThickness", "材料厚度");
            ColumnArray.Add("FocalPosition", "焦点位置");
            ColumnArray.Add("BeamSpot", "焦斑直径");
            ColumnArray.Add("LiftDistance", "蛙跳高度");
            ColumnArray.Add("NozzleKindName", "割嘴类型");
            ColumnArray.Add("NozzleDiameter", "割嘴内径");
            ColumnArray.Add("Feedrate", "速度");
            ColumnArray.Add("Power", "功率");
            ColumnArray.Add("Frequency", "频率");
            ColumnArray.Add("Duty", "占空比");
            ColumnArray.Add("GasPressure", "辅助气体压力");
            ColumnArray.Add("GasName", "辅助气体种类");
            ColumnArray.Add("GasSettingTime", "辅助气体时间");
            ColumnArray.Add("StandardDisplacement", "基准偏移量");
            ColumnArray.Add("Supple", "补偿量");
            ColumnArray.Add("EdgeSlt", "尖角");
            ColumnArray.Add("ApprSlt", "起始");
            ColumnArray.Add("PwrCtrl", "功率控制");
            ColumnArray.Add("Angle", "判定角度");
            ColumnArray.Add("RecoveryDistance", "复归距离");
            ColumnArray.Add("RecoveryFeedrate", "复归速度");
            ColumnArray.Add("RecoveryFrequency", "复归频率");
            ColumnArray.Add("RecoveryDuty", "复归占空比");
            ColumnArray.Add("StepFrequency", "步进频率");
            ColumnArray.Add("StepDuty", "步进占空比");
            ColumnArray.Add("StepTime", "步进时间");
            ColumnArray.Add("StepQuantity", "步进次数");
            ColumnArray.Add("PiercingTime", "穿孔时间");
            ColumnArray.Add("StandardDisplacement2", "基准偏移量2");
            ColumnArray.Add("GapAxis", "间隙轴");
            ColumnArray.Add("PbPower", "谷底功率");
            ColumnArray.Add("Gap", "间隙");
        }
    }

    public class DataColumn
    {
        public string Header { get; set; }
        public string BindingName { get; set; }
        public double Width { get; set; }
    }

    public class ProcessData
    {
        public Object Data { get; set; }
        public string Type { get; set; }
    }
}
