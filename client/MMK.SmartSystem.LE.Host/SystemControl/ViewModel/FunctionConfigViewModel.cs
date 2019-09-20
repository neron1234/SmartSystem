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
            SysModuleViews = SmartSystemLEConsts.SystemModules.CloneJson();
            CustomModuleViews = SmartSystemLEConsts.CustemModules.CloneJson();
            foreach (var item in CustomModuleViews)
            {
                //初始化自定义功能项和系统功能项的交集
                foreach (var sysItem in SysModuleViews)
                {
                    //foreach(var inItem in sysItem.MainMenuViews.Intersect(item.MainMenuViews))
                    foreach(var sysMenu in sysItem.MainMenuViews)
                    {
                        if (item.MainMenuViews.Any(n => n.Id == sysMenu.Id))
                        {
                            sysMenu.Show = System.Windows.Visibility.Collapsed;
                        }
                    }
                }
            }
            CheckMenuSetShowCommand.Execute(null);
        }

        public ICommand CheckMenuSetShowCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    foreach (var item in SysModuleViews)
                    {
                        if (item.MainMenuViews.Count(n => n.Show == System.Windows.Visibility.Visible) > 0)
                        {
                            item.Show = System.Windows.Visibility.Visible;
                        }
                        else
                        {
                            item.Show = System.Windows.Visibility.Collapsed;
                        }
                    }
                });
            }
        }

        public ICommand SysMenuSetHiddenCommand{
            get
            {
                return new RelayCommand(() => {
                    foreach (var item in SysModuleViews)
                    {
                        if (item.Show == System.Windows.Visibility.Collapsed)
                        {
                            foreach (var menu in item.MainMenuViews)
                            {
                                menu.Show = System.Windows.Visibility.Collapsed;
                            }
                        }
                    }
                });
            }
        }
    }
}
