using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Abp.Modules;
using MMK.SmartSystem.Common;

namespace MMK.SmartSystem.LE.Host.SystemControl.ViewModel
{
    public class FunctionConfigViewModel:ViewModelBase
    {
        private ObservableCollection<SystemMenuModuleViewModel> _SysModuleViews;
        public ObservableCollection<SystemMenuModuleViewModel> SysModuleViews
        {
            get { return _SysModuleViews; }
            set
            {
                _SysModuleViews = value;
                RaisePropertyChanged(() => SysModuleViews);  
            }
        }

        private ObservableCollection<SystemMenuModuleViewModel> _CustomModuleViews;
        public ObservableCollection<SystemMenuModuleViewModel> CustomModuleViews
        {
            get { return _CustomModuleViews; }
            set
            {
                _CustomModuleViews = value;
                RaisePropertyChanged(() => CustomModuleViews);
            }
        }

        public FunctionConfigViewModel(){
            //SysModuleViews = new ObservableCollection<SystemMenuModuleViewModel>(SmartSystemLEConsts.SystemModules.CloneJson().OrderByDescending(n => n.Sort));
            SysModuleViews = SmartSystemLEConsts.SystemModules.CloneJson();
            CustomModuleViews = new ObservableCollection<SystemMenuModuleViewModel>();
        }

        public ICommand AddCommand
        {
            get
            {
                return new RelayCommand<string>((s) =>
                {
                    
                });
            }
        }
    }
}
