using MMK.SmartSystem.Common.Model;
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

        public const string ApiHost = "http://123.207.159.105:21021";

        public static AuthenticateResultModel AuthenticateModel = new AuthenticateResultModel();

        public static AbpUserConfiguration UserConfiguration = new AbpUserConfiguration();

        public static string CurrentCulture = "en";

        public static UserInfo UserInfo = new UserInfo();
    }
}
