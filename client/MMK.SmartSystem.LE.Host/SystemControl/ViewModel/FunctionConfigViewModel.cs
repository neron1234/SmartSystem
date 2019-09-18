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



        public FunctionConfigViewModel()
        {
            SysModuleViews = SmartSystemLEConsts.SystemModules;
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
