using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MMK.SmartSystem.Laser.Base.MachineOperation.ViewModel
{
    public class ManualFindSidePageViewModel : ViewModelBase
    {
        private string _P1_X;
        public string P1_X
        {
            get { return _P1_X; }
            set
            {
                if (_P1_X != value)
                {
                    _P1_X = value;
                    RaisePropertyChanged(() => P1_X);
                }
            }
        }

        private string _P1_Y;
        public string P1_Y
        {
            get { return _P1_Y; }
            set
            {
                if (_P1_Y != value)
                {
                    _P1_Y = value;
                    RaisePropertyChanged(() => P1_Y);
                }
            }
        }

        private string _P2_X;
        public string P2_X
        {
            get { return _P2_X; }
            set
            {
                if (_P2_X != value)
                {
                    _P2_X = value;
                    RaisePropertyChanged(() => P2_X);
                }
            }
        }

        private string _P2_Y;
        public string P2_Y
        {
            get { return _P2_Y; }
            set
            {
                if (_P2_Y != value)
                {
                    _P2_Y = value;
                    RaisePropertyChanged(() => P2_Y);
                }
            }
        }

        private string _P3_X;
        public string P3_X
        {
            get { return _P3_X; }
            set
            {
                if (_P3_X != value)
                {
                    _P3_X = value;
                    RaisePropertyChanged(() => P3_X);
                }
            }
        }

        private string _P3_Y;
        public string P3_Y
        {
            get { return _P3_Y; }
            set
            {
                if (_P3_Y != value)
                {
                    _P3_Y = value;
                    RaisePropertyChanged(() => P3_Y);
                }
            }
        }

        private string _WpZeroX;
        public string WpZeroX
        {
            get { return _WpZeroX; }
            set
            {
                if (_WpZeroX != value)
                {
                    _WpZeroX = value;
                    RaisePropertyChanged(() => WpZeroX);
                }
            }
        }

        private string _WpZeroY;
        public string WpZeroY
        {
            get { return _WpZeroY; }
            set
            {
                if (_WpZeroY != value)
                {
                    _WpZeroY = value;
                    RaisePropertyChanged(() => WpZeroY);
                }
            }
        }

        private string _WpAngle;
        public string WpAngle
        {
            get { return _WpAngle; }
            set
            {
                if (_WpAngle != value)
                {
                    _WpAngle = value;
                    RaisePropertyChanged(() => WpAngle);
                }
            }
        }

        public ICommand GetP1Command
        {
            get
            {
                return new RelayCommand(() =>
                {
                });
            }
        }
        public ICommand GetP2Command
        {
            get
            {
                return new RelayCommand(() =>
                {
                });
            }
        }
        public ICommand GetP3Command
        {
            get
            {
                return new RelayCommand(() =>
                {
                });
            }
        }
        public ICommand ResetCmd
        {
            get
            {
                return new RelayCommand(() =>
                {
                });
            }
        }
        public ICommand LoadCmd
        {
            get
            {
                return new RelayCommand(() =>
                {
                });
            }
        }
        public ICommand TestCmd
        {
            get
            {
                return new RelayCommand(() =>
                {
                });
            }
        }

        public ManualFindSidePageViewModel()
        {
            this.P1_X = "0";
            this.P1_Y = "0";
            this.P2_X = "0";
            this.P2_Y = "0";
            this.P3_X = "0";
            this.P3_Y = "0";
            this.WpZeroX = "0";
            this.WpZeroY = "0";
            this.WpAngle = "0";
        }
    }
}
