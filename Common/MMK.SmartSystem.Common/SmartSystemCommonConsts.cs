using MMK.SmartSystem.Common.Model;
using MMK.SmartSystem.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Common
{
    public class SmartSystemCommonConsts
    {
        public const string ModulePageKey = "WPF.Page";

        public const string ModuleQueryParmKey = "SignalrQueryParm";

        //public const string ApiHost = "http://192.168.21.175:21021";
        public const string ApiHost = "http://localhost:21021";

        public static AuthenticateResultModel AuthenticateModel = new AuthenticateResultModel();

        public static AbpUserConfiguration UserConfiguration = new AbpUserConfiguration();

        public static string CurrentCulture = "en";

        public static UserInfo UserInfo = new UserInfo();

        public static SystemTranslate SystemTranslateModel = new SystemTranslate();

        public static List<SignalrQueryParmModel> SignalrQueryParmModels = new List<SignalrQueryParmModel>();

    }
}
