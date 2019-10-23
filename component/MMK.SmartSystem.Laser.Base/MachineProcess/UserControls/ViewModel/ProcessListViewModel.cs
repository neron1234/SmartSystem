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
    public class ProcessListViewModel:ViewModelBase
    {
        private ObservableCollection<object> _PageListData;
        public ObservableCollection<object> PageListData
        {
            get { return _PageListData; }
            set
            {
                if (_PageListData != value)
                {
                    _PageListData = value;
                    RaisePropertyChanged(() => PageListData);
                }
            }
        }

        public Dictionary<string,string> ColumnArray { get; set; }

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

            ColumnArray.Add("Angle", "角度");
            ColumnArray.Add("RecoveryDistance", "返回距离");
            ColumnArray.Add("RecoveryFeedrate", "返回速度");
            ColumnArray.Add("RecoveryFrequency", "返回频率");
            ColumnArray.Add("RecoveryDuty", "返回占空比");

            //Piercing(未完成)
            //ColumnArray.Add("Frequency", "初始值频率");
            //ColumnArray.Add("Duty", "初始值占空比");
            ColumnArray.Add("StepFrequency", "增量频率");
            ColumnArray.Add("StepDuty", "增量占空比");
            ColumnArray.Add("StepTime", "步 时间");
            ColumnArray.Add("StepQuantity", "步 数");
            ColumnArray.Add("PiercingTime", "穿孔时间");

            //SlopeControl(未完成)
            ColumnArray.Add("PowerMin", "功率 最小");
            ColumnArray.Add("PwrSpZr", "功率 速度0");
            ColumnArray.Add("FreqMin", "频率 最小");
            ColumnArray.Add("FreqSpZr", "频率 速度0");
            ColumnArray.Add("DutyMin", "占空比 最小");
            ColumnArray.Add("DutySpZr", "占空比 速度0");
            ColumnArray.Add("FeedRDec", "速度变化允许量");
        }
    }

    public class DataColumn
    {
        public string Header { get; set; }
        public string BindingName { get; set; }
        public double Width { get; set; }
    }
}
