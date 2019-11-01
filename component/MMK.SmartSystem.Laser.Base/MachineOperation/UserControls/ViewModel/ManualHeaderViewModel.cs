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

namespace MMK.SmartSystem.Laser.Base.MachineOperation.UserControls.ViewModel
{
    public class ManualHeaderViewModel : ViewModelBase
    {
        public string Title { get; set; }

        public string FullName { get; set; }
        private bool _isCheck;
        public bool IsCheck
        {
            get { return _isCheck; }
            set
            {
                if (_isCheck != value)
                {
                    _isCheck = value;
                    RaisePropertyChanged(() => IsCheck);
                }
            }
        }

        public ICommand ChangePageCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Messenger.Default.Send(new PageChangeModel()
                    {
                        FullType = Type.GetType($"MMK.SmartSystem.Laser.Base.MachineOperation.{FullName}"),
                        Page = PageEnum.WPFPage,
                        Title = "手动"
                    });
                });
            }
        }

        public static List<ManualHeaderViewModel> GetHeaderNodes()
        {
            return new List<ManualHeaderViewModel>()
            {
                new ManualHeaderViewModel(){ Title="I/O",FullName="MaualioPage"},
                new ManualHeaderViewModel(){ Title="简易轮廓",FullName="SimpleProfilePage"},
                new ManualHeaderViewModel(){ Title="自动巡边",FullName="AutoFindSidePage"},
                new ManualHeaderViewModel(){ Title="手动寻边",FullName="ManualFindSidePage"},
                 new ManualHeaderViewModel(){ Title="割嘴清洁",FullName="AutoCutterCleanPage"},
                   new ManualHeaderViewModel(){ Title="余料切割",FullName="RemainCutPage"},
                    new ManualHeaderViewModel(){ Title="割嘴复归",FullName="CutterResetCheckPage"},
                       new ManualHeaderViewModel(){ Title="割嘴对中",FullName="CutCenterPage"},
                    new ManualHeaderViewModel(){ Title="简易套料",FullName=""},
                       new ManualHeaderViewModel(){ Title="辅助气体",FullName="AuxGasCheckPage"}
            };
        }
    }
}
