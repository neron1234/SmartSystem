using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MMK.SmartSystem.Laser.Base.MachineProcessWork.UserControls.ViewModel
{
    public class SimulationViewModel:ViewModelBase
    {
        private string _TotalTimeStr;
        public string TotalTimeStr
        {
            get { return _TotalTimeStr; }
            set
            {
                if (_TotalTimeStr != value)
                {
                    _TotalTimeStr = value;
                    RaisePropertyChanged(() => TotalTimeStr);
                }
            }
        }

        private string _ElapsedTimeStr;
        public string ElapsedTimeStr
        {
            get { return _ElapsedTimeStr; }
            set
            {
                if (_ElapsedTimeStr != value)
                {
                    _ElapsedTimeStr = value;
                    RaisePropertyChanged(() => ElapsedTimeStr);
                }
            }
        }

        private string _RemainTimeStr;
        public string RemainTimeStr
        {
            get { return _RemainTimeStr; }
            set
            {
                if (_RemainTimeStr != value)
                {
                    _RemainTimeStr = value;
                    RaisePropertyChanged(() => RemainTimeStr);
                }
            }
        }

        private string _PercentageStr;
        public string PercentageStr
        {
            get { return _PercentageStr; }
            set
            {
                if (_PercentageStr != value)
                {
                    _PercentageStr = value;
                    RaisePropertyChanged(() => PercentageStr);
                }
            }
        }

        public double TotalTime { get; set; } = 0;
        public double ElapsedTime { get; set; } = 0;
        public double RemainTime { get; set; } = 0;

        public List<ProgressItemViewModel> progressItemList { get; set; } = new List<ProgressItemViewModel>();

        private System.Windows.Threading.DispatcherTimer TimeTimer { get; set; }

        private string _IsMax;
        public string IsMax
        {
            get { return _IsMax; }
            set
            {
                if (_IsMax != value)
                {
                    _IsMax = value;
                    RaisePropertyChanged(() => IsMax);
                }
            }
        }

        private string _IsNormal;
        public string IsNormal
        {
            get { return _IsNormal; }
            set
            {
                if (_IsNormal != value)
                {
                    _IsNormal = value;
                    RaisePropertyChanged(() => IsNormal);
                }
            }
        }

        public SimulationViewModel(){
            this.IsNormal = "true";
            this.IsMax = "false";
            for (int i = 0; i < 20; i++){
                progressItemList.Add(new ProgressItemViewModel { Id = i, FillColor = "#27251f", StrokeColor = "#333330" });
            }
            this.PercentageStr = "0%";
            this.TotalTime = 35;
            this.TotalTimeStr = new DateTime().AddSeconds(TotalTime).TimeOfDay.ToString();
            this.RemainTime = this.TotalTime;
            this.RemainTimeStr = this.TotalTimeStr;
            TimeTimer = new System.Windows.Threading.DispatcherTimer();
            TimeTimer.Tick += TimeTimer_Tick; ;
            TimeTimer.Interval = new TimeSpan(0, 0, 0, 1);
            TimeTimer.Start();
        }

        private void TimeTimer_Tick(object sender, EventArgs e){
            ElapsedTime++;
            RemainTime--;
            this.ElapsedTimeStr = new DateTime().AddSeconds(ElapsedTime).TimeOfDay.ToString();
            this.RemainTimeStr = new DateTime().AddSeconds(RemainTime).TimeOfDay.ToString();
            double percentage = ElapsedTime / TotalTime * 100;
            this.PercentageStr = Math.Round(percentage, 0) + "%";

            if (percentage / 5 > 1){
                for (int i = 0; i < Math.Floor(percentage / 5); i++){
                    this.progressItemList[i].IsLoad = true;
                }
            }
            if (TotalTime <= ElapsedTime){
                TimeTimer.Stop();
            }
        }

        public ICommand SetWindowCommand{
            get{
                return new RelayCommand<string>((str) => {
                    Messenger.Default.Send(str == "0" ? MachineProcessWork.ViewModel.PageStatus.Normal : MachineProcessWork.ViewModel.PageStatus.Max);
                    if (str == "0")
                    {
                        this.IsNormal = "true";
                        this.IsMax = "false";
                    }
                    else
                    {
                        this.IsNormal = "false";
                        this.IsMax = "true";
                    }
                });
            }
        }
    }
    public class ProgressItemViewModel : ViewModelBase {
        //每5%点亮一个单位

        //1.循环出100%的未点亮单位，每秒钟去计算已经运行了总时间的百分比，每超过一个5%，点亮一个单位

        //点亮
        //#FDCD00
        //#816e1d
        //未点亮
        //#27251f
        //#333330
        public int Id { get; set; }

        private string _FillColor;
        public string FillColor
        {
            get { return _FillColor; }
            set {
                if (_FillColor != value) {
                    _FillColor = value;
                    RaisePropertyChanged(() => FillColor);
                }
            }
        }

        private string _StrokeColor;
        public string StrokeColor{
            get { return _StrokeColor; }
            set{
                if (_StrokeColor != value){
                    _StrokeColor = value;
                    RaisePropertyChanged(() => StrokeColor);
                }
            }
        }

        private bool _IsLoad;
        public bool IsLoad{
            get { return _IsLoad; }
            set { 
                _IsLoad = value;
                if (_IsLoad){
                    this.FillColor = "#FDCD00";
                    this.StrokeColor = "#816e1d";
                }else{
                    this.FillColor = "#27251f";
                    this.StrokeColor = "#333330";
                }
            }
        }
    }
}
