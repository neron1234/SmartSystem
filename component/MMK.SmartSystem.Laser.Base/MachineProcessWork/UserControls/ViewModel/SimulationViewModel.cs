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

        private double _Percentage;
        public double Percentage
        {
            get { return _Percentage; }
            set
            {
                if (_Percentage != value)
                {
                    _Percentage = value;
                    RaisePropertyChanged(() => Percentage);
                }
            }
        }

       /// <summary>
       /// 进度条宽度
       /// </summary>
        private int _ProgressBarWidth;
        public int ProgressBarWidth
        {
            get { return _ProgressBarWidth; }
            set
            {
                if (_ProgressBarWidth != value)
                {
                    _ProgressBarWidth = value;
                    RaisePropertyChanged(() => ProgressBarWidth);
                }
            }
        }

        /// <summary>
        /// 间隔蒙版宽度
        /// </summary>
        private int _IntervalMaskWidth;
        public int IntervalMaskWidth
        {
            get { return _IntervalMaskWidth; }
            set
            {
                if (_IntervalMaskWidth != value)
                {
                    _IntervalMaskWidth = value;
                    RaisePropertyChanged(() => IntervalMaskWidth);
                }
            }
        }

        /// <summary>
        /// 间隔蒙版间距
        /// </summary>
        private string _IntervalMaskMargin;
        public string IntervalMaskMargin
        {
            get { return _IntervalMaskMargin; }
            set
            {
                if (_IntervalMaskMargin != value)
                {
                    _IntervalMaskMargin = value;
                    RaisePropertyChanged(() => IntervalMaskMargin);
                }
            }
        }

        public double TotalTime { get; set; } = 0;
        public double ElapsedTime { get; set; } = 0;
        public double RemainTime { get; set; } = 0;

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
            this.PercentageStr = "0%";
            this.Percentage = 0;
            this.IntervalMaskWidth = 4;
            this.IntervalMaskMargin = "36 0 0 0";
            this.ProgressBarWidth = 800;
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
            this.Percentage = Math.Round(percentage, 0);

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
                        this.ProgressBarWidth = 800;
                        this.IntervalMaskWidth = 4;
                        this.IntervalMaskMargin = "36 0 0 0";
                    }
                    else
                    {
                        this.IsNormal = "false";
                        this.IsMax = "true";
                        this.ProgressBarWidth = 1100;
                        this.IntervalMaskWidth = 4;
                        this.IntervalMaskMargin = "51 0 0 0";
                    }
                });
            }
        }
    }
}
