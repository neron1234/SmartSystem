using MMK.SmartSystem.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Laser.Base.MachineOperation.ViewModel
{
    public class AutoFindSidePageViewModel : MainTranslateViewModel
    {

        private string _XD;
        public string XD
        {
            get { return _XD; }
            set
            {
                if (_XD != value)
                {
                    _XD = value;
                    RaisePropertyChanged(() => XD);
                }
            }
        }

        private string _YD;
        public string YD
        {
            get { return _YD; }
            set
            {
                if (_YD != value)
                {
                    _YD = value;
                    RaisePropertyChanged(() => YD);
                }
            }
        }

        private string _SITA;
        public string SITA
        {
            get { return _SITA; }
            set
            {
                if (_SITA != value)
                {
                    _SITA = value;
                    RaisePropertyChanged(() => SITA);
                }
            }
        }

        private string _H;
        public string H
        {
            get { return _H; }
            set
            {
                if (_H != value)
                {
                    _H = value;
                    RaisePropertyChanged(() => H);
                }
            }
        }

        private string _X;
        public string X
        {
            get { return _X; }
            set
            {
                if (_X != value)
                {
                    _X = value;
                    RaisePropertyChanged(() => X);
                }
            }
        }

        private string _Y;
        public string Y
        {
            get { return _Y; }
            set
            {
                if (_Y != value)
                {
                    _Y = value;
                    RaisePropertyChanged(() => Y);
                }
            }
        }

        private string _RH;
        public string RH
        {
            get { return _RH; }
            set
            {
                if (_RH != value)
                {
                    _RH = value;
                    RaisePropertyChanged(() => RH);
                }
            }
        }

        public AutoFindSidePageViewModel(string authKey) : base(authKey)
        {
            this.XD = "0";
            this.YD = "0"; 
            this.SITA = "0";
            this.H = "0";
            this.X = "0";
            this.Y = "0";
            this.RH = "0";
        }
    }
}
