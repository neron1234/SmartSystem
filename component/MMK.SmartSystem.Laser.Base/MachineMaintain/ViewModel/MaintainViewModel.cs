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

        public MaintainViewModel()
        {
            ChangePage(0);
        }

        private void ChangePage(int id){
            switch (id){
                case 0:
                    this.PageControl = new MachineConfigControl();
                    break;
                case 1:
                    this.PageControl = new SoftSettingControl();
                    break;
                case 2:
                    this.PageControl = new SparePartControl();
                    break;
                case 3:
                    this.PageControl = new ProductSpecificationControl();
                    break;
                case 4:
                    this.PageControl = new SystemUserControl();
                    break;
            }
        }

        public ICommand ChangePageCommand{
            get{
                return new RelayCommand<string>((page) =>{
                    ChangePage(Convert.ToInt32(page));
                });
            }
        }
    }
}
