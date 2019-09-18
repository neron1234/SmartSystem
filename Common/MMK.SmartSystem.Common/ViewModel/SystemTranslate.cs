using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Common.ViewModel
{
    public class LastTranslateLevelAttribute : Attribute
    {

    }

    public class SystemTranslate
    {
        public SmartSystem SmartSystem { get; set; } = new SmartSystem();
        public MachineOperation MachineOperation { get; set; } = new MachineOperation();
    }

    public class SmartSystem : ViewModelBase
    {
        private string _Index1;
        public string Index1
        {
            get { return _Index1; }
            set
            {
                if (_Index1 != value)
                {
                    _Index1 = value;
                    RaisePropertyChanged(() => Index1);
                }
            }
        }

        private string _Index2;
        public string Index2
        {
            get { return _Index2; }
            set
            {
                if (_Index2 != value)
                {
                    _Index2 = value;
                    RaisePropertyChanged(() => Index2);
                }
            }
        }

        private string _Account;
        public string Account
        {
            get { return _Account; }
            set
            {
                if (_Account != value)
                {
                    _Account = value;
                    RaisePropertyChanged(() => Account);
                }
            }
        }

        private string _Pwd;
        public string Pwd
        {
            get { return _Pwd; }
            set
            {
                if (_Pwd != value)
                {
                    _Pwd = value;
                    RaisePropertyChanged(() => Pwd);
                }
            }
        }

        private string _Login;
        public string Login
        {
            get { return _Login; }
            set
            {
                if (_Login != value)
                {
                    _Login = value;
                    RaisePropertyChanged(() => Login);
                }
            }
        }

        private string _ChangeAccout;
        public string ChangeAccout
        {
            get { return _ChangeAccout; }
            set
            {
                if (_ChangeAccout != value)
                {
                    _ChangeAccout = value;
                    RaisePropertyChanged(() => ChangeAccout);
                }
            }
        }

        private string _Cancel;
        public string Cancel
        {
            get { return _Cancel; }
            set
            {
                if (_Cancel != value)
                {
                    _Cancel = value;
                    RaisePropertyChanged(() => Cancel);
                }
            }
        }

        private string _LoginSucceed;
        public string LoginSucceed
        {
            get { return _LoginSucceed; }
            set
            {
                if (_LoginSucceed != value)
                {
                    _LoginSucceed = value;
                    RaisePropertyChanged(() => LoginSucceed);
                }
            }
        }

        private string _LoginFailure;
        public string LoginFailure
        {
            get { return _LoginFailure; }
            set
            {
                if (_LoginFailure != value)
                {
                    _LoginFailure = value;
                    RaisePropertyChanged(() => LoginFailure);
                }
            }
        }


        private string _OldPwd;
        public string OldPwd
        {
            get { return _OldPwd; }
            set
            {
                if (_OldPwd != value)
                {
                    _OldPwd = value;
                    RaisePropertyChanged(() => OldPwd);
                }
            }
        }

        private string _Pwd1;
        public string Pwd1
        {
            get { return _Pwd1; }
            set
            {
                if (_Pwd1 != value)
                {
                    _Pwd1 = value;
                    RaisePropertyChanged(() => Pwd1);
                }
            }
        }

        private string _Pwd2;
        public string Pwd2
        {
            get { return _Pwd2; }
            set
            {
                if (_Pwd2 != value)
                {
                    _Pwd2 = value;
                    RaisePropertyChanged(() => Pwd2);
                }
            }
        }

        private string _UpdatePwd;
        public string UpdatePwd
        {
            get { return _UpdatePwd; }
            set
            {
                if (_UpdatePwd != value)
                {
                    _UpdatePwd = value;
                    RaisePropertyChanged(() => UpdatePwd);
                }
            }
        }

        private string _Chinese;
        public string Chinese
        {
            get { return _Chinese; }
            set
            {
                if (_Chinese != value)
                {
                    _Chinese = value;
                    RaisePropertyChanged(() => Chinese);
                }
            }
        }

        private string _English;
        public string English
        {
            get { return _English; }
            set
            {
                if (_English != value)
                {
                    _English = value;
                    RaisePropertyChanged(() => English);
                }
            }
        }
    }

    public class MachineOperation
    {
        [LastTranslateLevel]
        public AutoFindSide AutoFindSide { get; set; } = new AutoFindSide();
    }

    public class AutoFindSide : ViewModelBase
    {
        private string _PlateOrigin;
        public string PlateOrigin
        {
            get { return _PlateOrigin; }
            set
            {
                if (_PlateOrigin != value)
                {
                    _PlateOrigin = value;
                    RaisePropertyChanged(() => PlateOrigin);
                }
            }
        }

        private string _PlateAngle;
        public string PlateAngle
        {
            get { return _PlateAngle; }
            set
            {
                if (_PlateAngle != value)
                {
                    _PlateAngle = value;
                    RaisePropertyChanged(() => PlateAngle);
                }
            }
        }

        private string _P12Distance;
        public string P12Distance
        {
            get { return _P12Distance; }
            set
            {
                if (_P12Distance != value)
                {
                    _P12Distance = value;
                    RaisePropertyChanged(() => P12Distance);
                }
            }
        }

        private string _OptoelectronicSwitchDistance;
        public string OptoelectronicSwitchDistance
        {
            get { return _OptoelectronicSwitchDistance; }
            set
            {
                if (_OptoelectronicSwitchDistance != value)
                {
                    _OptoelectronicSwitchDistance = value;
                    RaisePropertyChanged(() => OptoelectronicSwitchDistance);
                }
            }
        }

        private string _PlateEdgeDistance;
        public string PlateEdgeDistance
        {
            get { return _PlateEdgeDistance; }
            set
            {
                if (_PlateEdgeDistance != value)
                {
                    _PlateEdgeDistance = value;
                    RaisePropertyChanged(() => PlateEdgeDistance);
                }
            }
        }

        private string _FindXYTrack;
        public string FindXYTrack
        {
            get { return _FindXYTrack; }
            set
            {
                if (_FindXYTrack != value)
                {
                    _FindXYTrack = value;
                    RaisePropertyChanged(() => FindXYTrack);
                }
            }
        }

        private string _ManualStartFindSide;
        public string ManualStartFindSide
        {
            get { return _ManualStartFindSide; }
            set
            {
                if (_ManualStartFindSide != value)
                {
                    _ManualStartFindSide = value;
                    RaisePropertyChanged(() => ManualStartFindSide);
                }
            }
        }
    }
}
