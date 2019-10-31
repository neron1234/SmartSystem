using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MMK.SmartSystem.Laser.Base.MachineMaintain.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace MMK.SmartSystem.Laser.Base.MachineMaintain.ViewModel
{
    public class MaintainViewModel:ViewModelBase
    {
        private UserControl _PageControl;
        public UserControl PageControl
        {
            get { return _PageControl; }
            set
            {
                if (_PageControl != value)
                {
                    _PageControl = value;
                    RaisePropertyChanged(() => PageControl);
                }
            }
        }

        public string Title { get; set; }

        public string FullName { get; set; }


        public ICommand ChangePageCommand{
            get{
                return new RelayCommand<string>((name) =>{
                    switch (name)
                    {
                        case "MachineConfigControl":
                            this.PageControl = new MachineConfigControl();
                            break;
                        case "SoftSettingControl":
                            this.PageControl = new SoftSettingControl();
                            break;
                        case "SparePartControl":
                            this.PageControl = new SparePartControl();
                            break;
                        case "ProductSpecificationControl":
                            this.PageControl = new ProductSpecificationControl();
                            break;
                        case "SystemUserControl":
                            this.PageControl = new SystemUserControl();
                            break;
                    }
                });
            }
        }

        public static List<MaintainViewModel> GetHeaderNodes()
        {
            return new List<MaintainViewModel>()
            {
                new MaintainViewModel(){ Title="机床配置",FullName="MachineConfigControl"},
                new MaintainViewModel(){ Title="软件设定",FullName="SoftSettingControl"},
                new MaintainViewModel(){ Title="备件一览",FullName="SparePartControl"},
                new MaintainViewModel(){ Title="产品说明",FullName="ProductSpecificationControl"},
                new MaintainViewModel(){ Title="诚信系统",FullName="SystemUserControl"}
            };
        }
    }
}
