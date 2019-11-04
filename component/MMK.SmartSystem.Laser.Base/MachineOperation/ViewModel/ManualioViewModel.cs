using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MMK.SmartSystem.Common.ViewModel;
using MMK.SmartSystem.WebCommon.DeviceModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MMK.SmartSystem.Laser.Base.MachineOperation.ViewModel
{
    public class ManualioViewModel : ViewModelBase
    {
        public ObservableCollection<IoBtnInfo> IoBtnList { get; set; }

        public OperaBtnViewModel OperaModel { get; set; }

        public event Action<IoBtnInfo> IoBtnClickEvent;
        private ActionBtnInfo _ActionBtnInfo;
        public ActionBtnInfo ActionBtnInfo
        {
            get { return _ActionBtnInfo; }
            set
            {
                if (_ActionBtnInfo != value)
                {
                    _ActionBtnInfo = value;
                    RaisePropertyChanged(() => ActionBtnInfo);
                }
            }
        }


        public ManualioViewModel()
        {
            OperaModel = new OperaBtnViewModel();
            ActionBtnInfo = new ActionBtnInfo
            {
                Color = "#000000",
                IsActive = false
            };

            IoBtnList = new ObservableCollection<IoBtnInfo> {
                new IoBtnInfo { Id="maualio_exchangetable",Name = "交换台",AdrType=5,Adr=1001,Bit=0,
                    Icon = "/MMK.SmartSystem.Laser.Base;component/Resources/Images/Manual_io/Switchboard.png",BtnVisibility=Visibility.Visible,BgVisibility=Visibility.Collapsed,IsActive =false},
                new IoBtnInfo { Id="maualio_nozzlereset", Name = "割嘴复归",AdrType=5,Adr=1001,Bit=1,  Icon = "/MMK.SmartSystem.Laser.Base;component/Resources/Images/Manual_io/CutterReset.png",BtnVisibility=Visibility.Visible,BgVisibility=Visibility.Collapsed,IsActive =false},
                new IoBtnInfo { Name = "", Icon = "",BtnVisibility=Visibility.Collapsed,BgVisibility=Visibility.Visible,IsActive =false},
                new IoBtnInfo { Name = "", Icon = "",BtnVisibility=Visibility.Collapsed,BgVisibility=Visibility.Visible,IsActive =false},
                new IoBtnInfo { Name = "", Icon = "",BtnVisibility=Visibility.Collapsed,BgVisibility=Visibility.Visible,IsActive =false},
                new IoBtnInfo {Id="maualio_jog", Name = "手动", AdrType=5,Adr=1000,Bit=5,Icon = "/MMK.SmartSystem.Laser.Base;component/Resources/Images/Manual_io/Manual.png",BtnVisibility=Visibility.Visible,BgVisibility=Visibility.Collapsed,IsActive =false},
                new IoBtnInfo {Id="maualio_inc", Name = "增量",  AdrType=5,Adr=1000,Bit=7,Icon = "/MMK.SmartSystem.Laser.Base;component/Resources/Images/Manual_io/Incremental.png",BtnVisibility=Visibility.Visible,BgVisibility=Visibility.Collapsed,IsActive =false},
                new IoBtnInfo { Id="maualio_hnd",Name = "手轮", AdrType=5,Adr=1000,Bit=6, Icon = "/MMK.SmartSystem.Laser.Base;component/Resources/Images/Manual_io/HandWheel.png",BtnVisibility=Visibility.Visible,BgVisibility=Visibility.Collapsed,IsActive =false},
                new IoBtnInfo { Id="maualio_auto",Name = "自动", AdrType=5,Adr=1000,Bit=0, Icon = "/MMK.SmartSystem.Laser.Base;component/Resources/Images/Manual_io/Auto.png",BtnVisibility=Visibility.Visible,BgVisibility=Visibility.Collapsed,IsActive =false},
                new IoBtnInfo { Id="maualio_edit", Name = "编辑", AdrType=5,Adr=1000,Bit=1, Icon = "/MMK.SmartSystem.Laser.Base;component/Resources/Images/Manual_io/Edit.png",BtnVisibility=Visibility.Visible,BgVisibility=Visibility.Collapsed,IsActive =false},
                new IoBtnInfo { Id="maualio_mdi",Name = "MDI",  AdrType=5,Adr=1000,Bit=2,Icon = "/MMK.SmartSystem.Laser.Base;component/Resources/Images/Manual_io/MDI.png",BtnVisibility=Visibility.Visible,BgVisibility=Visibility.Collapsed,IsActive =false},
                new IoBtnInfo { Id="maualio_rmt",Name = "远程", AdrType=5,Adr=1000,Bit=3, Icon = "/MMK.SmartSystem.Laser.Base;component/Resources/Images/Manual_io/Remote.png",BtnVisibility=Visibility.Visible,BgVisibility=Visibility.Collapsed,IsActive =false},
                new IoBtnInfo { Id="maualio_ref",Name = "参考点",  AdrType=5,Adr=1000,Bit=4,Icon = "/MMK.SmartSystem.Laser.Base;component/Resources/Images/Manual_io/RPoint.png",BtnVisibility=Visibility.Visible,BgVisibility=Visibility.Collapsed,IsActive =false},
                new IoBtnInfo { Name = "", Icon = "",BtnVisibility=Visibility.Collapsed,BgVisibility=Visibility.Visible,IsActive =false},
                new IoBtnInfo { Name = "", Icon = "",BtnVisibility=Visibility.Collapsed,BgVisibility=Visibility.Visible,IsActive =false},
                new IoBtnInfo { Name = "", Icon = "",BtnVisibility=Visibility.Collapsed,BgVisibility=Visibility.Visible,IsActive =false}
            };
            IoBtnList.ToList().ForEach(d => d.IoBtnClickEvent += D_IoBtnClickEvent);
        }

        private void D_IoBtnClickEvent(IoBtnInfo obj)
        {
            IoBtnClickEvent?.Invoke(obj);
        }
    }
    public class OperaBtnViewModel : CncResultViewModel<ReadPmcResultItemModel>
    {

        private string _maualio_pcval;
        public string Maualio_Pcval
        {
            get { return _maualio_pcval + "%"; }
            set
            {
                if (_maualio_pcval != value)
                {
                    _maualio_pcval = value;
                    RaisePropertyChanged(() => Maualio_Pcval);
                }
            }
        }
        private string _maualio_frval;
        public string Maualio_Frval
        {
            get { return _maualio_frval + "%"; }
            set
            {
                if (_maualio_frval != value)
                {
                    _maualio_frval = value;
                    RaisePropertyChanged(() => Maualio_Frval);
                }
            }
        }
        private string _maualio_duval;
        public string Maualio_Duval
        {
            get { return _maualio_duval + "%"; }
            set
            {
                if (_maualio_duval != value)
                {
                    _maualio_duval = value;
                    RaisePropertyChanged(() => Maualio_Duval);
                }
            }
        }

        private string _maualio_xplus;
        public string Maualio_Xplus
        {
            get { return _maualio_xplus; }
            set
            {
                if (_maualio_xplus != value)
                {
                    _maualio_xplus = value;
                    RaisePropertyChanged(() => Maualio_Xplus);
                }
            }
        }

        private string _maualio_xminus;
        public string Maualio_Xminus
        {
            get { return _maualio_xminus; }
            set
            {
                if (_maualio_xminus != value)
                {
                    _maualio_xminus = value;
                    RaisePropertyChanged(() => Maualio_Xminus);
                }
            }
        }

        private string _maualio_yplus;
        public string Maualio_Yplus
        {
            get { return _maualio_yplus; }
            set
            {
                if (_maualio_yplus != value)
                {
                    _maualio_yplus = value;
                    RaisePropertyChanged(() => Maualio_Yplus);
                }
            }
        }

        private string _maualio_yminus;
        public string Maualio_Yminus
        {
            get { return _maualio_yminus; }
            set
            {
                if (_maualio_yminus != value)
                {
                    _maualio_yminus = value;
                    RaisePropertyChanged(() => Maualio_Yminus);
                }
            }
        }

        private string _maualio_zplus;
        public string Maualio_Zplus
        {
            get { return _maualio_zplus; }
            set
            {
                if (_maualio_zplus != value)
                {
                    _maualio_zplus = value;
                    RaisePropertyChanged(() => Maualio_Zplus);
                }
            }
        }

        private string _maualio_zminus;
        public string Maualio_Zminus
        {
            get { return _maualio_zminus; }
            set
            {
                if (_maualio_zminus != value)
                {
                    _maualio_zminus = value;
                    RaisePropertyChanged(() => Maualio_Zminus);
                }
            }
        }
        public OperaBtnViewModel()
        {
            Maualio_Frval = "0";
            Maualio_Duval = "0";
            Maualio_Pcval = "0";
        }
    }


    public class IoBtnInfo : CncResultViewModel<ReadPmcResultItemModel>
    {
        public string Name { get; set; }

        public string Id { get; set; }
        private string _Icon;
        public string Icon
        {
            get { return _Icon; }
            set
            {
                if (_Icon != value)
                {
                    _Icon = value;
                    RaisePropertyChanged(() => Icon);
                }
            }
        }
        public short AdrType { get; set; }

        public ushort Adr { get; set; }
        public ushort Bit { get; set; }

        public bool IsActive { get; set; }

        private Visibility _BtnVisibility;
        public Visibility BtnVisibility
        {
            get { return _BtnVisibility; }
            set
            {
                if (_BtnVisibility != value)
                {
                    _BtnVisibility = value;
                    RaisePropertyChanged(() => BtnVisibility);
                }
            }
        }

        private Visibility _BgVisibility;
        public Visibility BgVisibility
        {
            get { return _BgVisibility; }
            set
            {
                if (_BgVisibility != value)
                {
                    _BgVisibility = value;
                    RaisePropertyChanged(() => BgVisibility);
                }
            }
        }

        public void ClearActive()
        {
            if (IsActive)
            {
                this.Icon = this.Icon.Replace("_Active", "");
                this.IsActive = false;

            }


        }
        public void StartActive()
        {
            if (!IsActive)
            {
                var url = this.Icon.Substring(0, this.Icon.LastIndexOf('.'));
                this.Icon = url + "_Active.png";
                this.IsActive = true;
            }

        }

        public event Action<IoBtnInfo> IoBtnClickEvent;
        public ICommand IoBtnCommand
        {
            get
            {
                return new RelayCommand(() => IoBtnClickEvent?.Invoke(this));
            }
        }
    }

    public class ActionBtnInfo : ViewModelBase
    {
        private string _Color;
        public string Color
        {
            get { return _Color; }
            set
            {
                if (_Color != value)
                {
                    _Color = value;
                    RaisePropertyChanged(() => Color);
                }
            }
        }

        public string OldColor { get; set; }

        public bool IsActive { get; set; }

        public ICommand ActionBtnCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (!string.IsNullOrEmpty(this.Color))
                    {
                        if (!IsActive)
                        {
                            this.Color = OldColor;
                        }
                        else
                        {
                            this.Color = "#fdcd00";
                        }
                    }
                });
            }
        }
    }
}
