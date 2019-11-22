using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MMK.SmartSystem.LE.Host.SystemControl.ViewModel
{
    public class HeaderStatusViewModel : ViewModelBase
    {
        private string _ServoImg;
        public string ServoImg
        {
            get { return _ServoImg; }
            set
            {
                if (_ServoImg != value)
                {
                    _ServoImg = value;
                    RaisePropertyChanged(() => ServoImg);
                }
            }
        }

        private string _MachineImg;
        public string MachineImg
        {
            get { return _MachineImg; }
            set
            {
                if (_MachineImg != value)
                {
                    _MachineImg = value;
                    RaisePropertyChanged(() => MachineImg);
                }
            }
        }

        private string _LaserImg;
        public string LaserImg
        {
            get { return _LaserImg; }
            set
            {
                if (_LaserImg != value)
                {
                    _LaserImg = value;
                    RaisePropertyChanged(() => LaserImg);
                }
            }
        }

        private string _SystemTime;
        public string SystemTime
        {
            get { return _SystemTime; }
            set
            {
                if (_SystemTime != value)
                {
                    _SystemTime = value;
                    RaisePropertyChanged(() => SystemTime);
                }
            }
        }

        private System.Windows.Threading.DispatcherTimer TimeTimer { get; set; }
        public HeaderStatusViewModel()
        {
            LaserImg = "/MMK.SmartSystem.LE.Host;component/Resources/Images/Status_Green.png";
            MachineImg = "/MMK.SmartSystem.LE.Host;component/Resources/Images/Status_Blue.png";
            ServoImg = "/MMK.SmartSystem.LE.Host;component/Resources/Images/Status_Red.png";
            TimeTimer = new System.Windows.Threading.DispatcherTimer();
            TimeTimer.Tick += TimeTimer_Tick;
            TimeTimer.Interval = new TimeSpan(0, 0, 0, 1);
            TimeTimer.Start();
        }

        private void TimeTimer_Tick(object sender, EventArgs e)
        {
            this.SystemTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public ICommand ShutDownCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Environment.Exit(0);
                });
            }
        }

        public ICommand HomeCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Messenger.Default.Send(new PageChangeModel() { Url = "/", Page = PageEnum.WebPage });
                });
            }
        }
    }
}
