using Abp.Dependency;
using Abp.Events.Bus.Handlers;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common;
using MMK.SmartSystem.Common.EventDatas;
using MMK.SmartSystem.Common.Model;
using MMK.SmartSystem.Common.SerivceProxy;
using MMK.SmartSystem.LE.Host.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TokenAuthClient = MMK.SmartSystem.Common.SerivceProxy.TokenAuthClient;

namespace MMK.SmartSystem.LE.Host.EventHandler
{
    public class UserLanguageHandler : BaseEventHandler<UserLanguageEventData, AbpUserConfiguration>
    {
        public override RequestResult<AbpUserConfiguration> WebRequest(UserLanguageEventData eventData)
        {
            if (string.IsNullOrEmpty(eventData.Culture))
            {
                return null;
            }
            UserClientServiceProxy userClientService = new UserClientServiceProxy(apiHost, httpClient);
            userClientService.ChangeLanguageAsync(new ChangeUserLanguageDto() { LanguageName = eventData.Culture }).Wait();
            var tokenAuthClient = new TokenAuthClient(apiHost, httpClient);
            var obj2 = tokenAuthClient.GetUserConfiguraionAsync().Result;
            if (obj2.Success)
            {
                SmartSystemCommonConsts.UserConfiguration = obj2.Result;
                new BaseTranslate().Translate();
            }
            return obj2;
        }
    }
}
