using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MMK.SmartSystem.Laser.Base.MachineMaintain.ViewModel
{
    public class MaintainViewModel:ViewModelBase
    {
        public string Title { get; set; }

        public string FullName { get; set; }


        public ICommand ChangePageCommand
        {
            get
            {
                return new RelayCommand(() =>
                {

                });
            }
        }

        public static List<MaintainViewModel> GetHeaderNodes()
        {
            return new List<MaintainViewModel>()
            {
                new MaintainViewModel(){ Title="机床配置",FullName="MaualioPage"},
                new MaintainViewModel(){ Title="软件设定",FullName="SimpleProfilePage"},
                new MaintainViewModel(){ Title="备件一览",FullName="AutoFindSidePage"},
                new MaintainViewModel(){ Title="产品说明",FullName="ManualFindSidePage"},
                new MaintainViewModel(){ Title="诚信系统",FullName="AutoCutterCleanPage"}
            };
        }
    }
}
