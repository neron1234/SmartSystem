using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common.ViewModel;
using MMK.SmartSystem.WebCommon.DeviceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MMK.SmartSystem.LE.Host.SystemControl.ViewModel
{

    public class HeaderStatusViewModel : CncResultViewModel<ReadPmcResultItemModel>
    {
        private string devecesState;

        public string mainPage_devicestate
        {
            get { return devecesState; }
            set
            {
                devecesState = value;
                MachineImg = GetStateImage(devecesState);
            }
        }

        private string servostate;

        public string mainPage_servostate
        {
            get { return servostate; }
            set
            {
                servostate = value;
                ServoImg = GetStateImage(servostate);
            }
        }


        private string laserState;

        public string mainPage_laserstate
        {
            get { return laserState; }
            set
            {
                laserState = value;
                LaserImg = GetStateImage(laserState);
            }
        }

        private string mode;

        public string mainPage_mode
        {
            get { return mode; }
            set
            {
                mode = value;
                Mode = mode;
            }
        }



        private Dictionary<string, string> modeDict = new Dictionary<string, string>()
        { { "0", "OFF" },{ "1","AUTO"},{ "2","EDIT"},{ "4","MDI"} ,{ "8","RMT"} ,{ "16","REF"} ,{ "32","JOG"} ,{ "64","HND"},{ "128","INC"} };
        private string _Mode;
        public string Mode
        {
            get
            {
                if (!string.IsNullOrEmpty(_Mode)&& modeDict.ContainsKey(_Mode))
                {
                    return modeDict[_Mode];
                }
                return "ERR";
            }
            set
            {
                if (_Mode != value)
                {
                    _Mode = value;
                    RaisePropertyChanged(() => Mode);
                }
            }
        }

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
            LaserImg = GetStateImage("0");
            MachineImg = GetStateImage("0");
            ServoImg = GetStateImage("0");
            TimeTimer = new System.Windows.Threading.DispatcherTimer();
            TimeTimer.Tick += TimeTimer_Tick;
            TimeTimer.Interval = new TimeSpan(0, 0, 0, 1);
            TimeTimer.Start();
        }

        private string GetStateImage(string state)
        {

            if (state == "1")
            {
                return "Status_Green";
            }
            if (state == "4")
            {
                return "Status_Red";
            }
            return "Status_Blue";
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
