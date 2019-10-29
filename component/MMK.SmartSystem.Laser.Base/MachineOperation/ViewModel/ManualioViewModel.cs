using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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
    public class ManualioViewModel:ViewModelBase
    {
        public ObservableCollection<IoBtnInfo> IoBtnList { get; set; }


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
            ActionBtnInfo = new ActionBtnInfo
            {
                Color = "#000000",
                IsActive = false
            };

            IoBtnList = new ObservableCollection<IoBtnInfo> {
                new IoBtnInfo { Name = "交换台", Icon = "/MMK.SmartSystem.Laser.Base;component/Resources/Images/Manual_io/Switchboard.png",BtnVisibility=Visibility.Visible,BgVisibility=Visibility.Collapsed,IsActive =false},
                new IoBtnInfo { Name = "割嘴复归", Icon = "/MMK.SmartSystem.Laser.Base;component/Resources/Images/Manual_io/CutterReset.png",BtnVisibility=Visibility.Visible,BgVisibility=Visibility.Collapsed,IsActive =false},
                new IoBtnInfo { Name = "", Icon = "",BtnVisibility=Visibility.Collapsed,BgVisibility=Visibility.Visible,IsActive =false},
                new IoBtnInfo { Name = "", Icon = "",BtnVisibility=Visibility.Collapsed,BgVisibility=Visibility.Visible,IsActive =false},
                new IoBtnInfo { Name = "", Icon = "",BtnVisibility=Visibility.Collapsed,BgVisibility=Visibility.Visible,IsActive =false},
                new IoBtnInfo { Name = "手动", Icon = "/MMK.SmartSystem.Laser.Base;component/Resources/Images/Manual_io/Manual.png",BtnVisibility=Visibility.Visible,BgVisibility=Visibility.Collapsed,IsActive =false},
                new IoBtnInfo { Name = "增量", Icon = "/MMK.SmartSystem.Laser.Base;component/Resources/Images/Manual_io/Incremental.png",BtnVisibility=Visibility.Visible,BgVisibility=Visibility.Collapsed,IsActive =false},
                new IoBtnInfo { Name = "手轮", Icon = "/MMK.SmartSystem.Laser.Base;component/Resources/Images/Manual_io/HandWheel.png",BtnVisibility=Visibility.Visible,BgVisibility=Visibility.Collapsed,IsActive =false},
                new IoBtnInfo { Name = "自动", Icon = "/MMK.SmartSystem.Laser.Base;component/Resources/Images/Manual_io/Auto.png",BtnVisibility=Visibility.Visible,BgVisibility=Visibility.Collapsed,IsActive =false},
                new IoBtnInfo { Name = "编辑", Icon = "/MMK.SmartSystem.Laser.Base;component/Resources/Images/Manual_io/Edit.png",BtnVisibility=Visibility.Visible,BgVisibility=Visibility.Collapsed,IsActive =false},
                new IoBtnInfo { Name = "MDI", Icon = "/MMK.SmartSystem.Laser.Base;component/Resources/Images/Manual_io/MDI.png",BtnVisibility=Visibility.Visible,BgVisibility=Visibility.Collapsed,IsActive =false},
                new IoBtnInfo { Name = "远程", Icon = "/MMK.SmartSystem.Laser.Base;component/Resources/Images/Manual_io/Remote.png",BtnVisibility=Visibility.Visible,BgVisibility=Visibility.Collapsed,IsActive =false},
                new IoBtnInfo { Name = "参考点", Icon = "/MMK.SmartSystem.Laser.Base;component/Resources/Images/Manual_io/RPoint.png",BtnVisibility=Visibility.Visible,BgVisibility=Visibility.Collapsed,IsActive =false},
                new IoBtnInfo { Name = "", Icon = "",BtnVisibility=Visibility.Collapsed,BgVisibility=Visibility.Visible,IsActive =false},
                new IoBtnInfo { Name = "", Icon = "",BtnVisibility=Visibility.Collapsed,BgVisibility=Visibility.Visible,IsActive =false},
                new IoBtnInfo { Name = "", Icon = "",BtnVisibility=Visibility.Collapsed,BgVisibility=Visibility.Visible,IsActive =false}
            };
        }
    }

    public class IoBtnInfo : ViewModelBase
    {
        public string Name { get; set; }

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

        public ICommand IoBtnCommand
        {
            get
            {
                return new RelayCommand(() => {
                    if (!string.IsNullOrEmpty(this.Icon)){
                        if (!IsActive){
                            var url = this.Icon.Substring(0, this.Icon.LastIndexOf('.'));
                            this.Icon = url + "_Active.png";
                            this.IsActive = true;
                        }else{
                            this.Icon = this.Icon.Replace("_Active", "");
                            this.IsActive = false;
                        }
                    }
                });
            }
        }
    }

    public class ActionBtnInfo : ViewModelBase {
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
