using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.LE.Host.SystemControl.ViewModel
{
    public class ModuleMenuViewModel:ViewModelBase
    {
        private ObservableCollection<SystemMenuModuleViewModel> _SysModuleViews;
        public ObservableCollection<SystemMenuModuleViewModel> SysModuleViews
        {
            get { return _SysModuleViews; }
            set
            {
                if (_SysModuleViews != value)
                {
                    _SysModuleViews = value;
                    RaisePropertyChanged(() => SysModuleViews);
                }
            }
        }
        public ModuleMenuViewModel()
        {
            this.SysModuleViews = new ObservableCollection<SystemMenuModuleViewModel>();
            this.SysModuleViews = SmartSystemLEConsts.SystemModules;
        }
    }
}
