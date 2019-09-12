using MMK.SmartSystem.LE.Host.SystemControl.ViewModel;
using MMK.SmartSystem.LE.Host.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.LE.Host
{
    public class SmartSystemLEConsts
    {
        public const string DefaultUser = "user";

        public const string DefaultPwd = "123qwe";

        public const string Culture = "en";

        public static SystemTranslate SystemTranslateModel = new SystemTranslate();
        public static ObservableCollection<SystemMenuModuleViewModel> SystemModules = new ObservableCollection<SystemMenuModuleViewModel>();


    }
}
