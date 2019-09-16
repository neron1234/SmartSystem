using Abp.Dependency;
using Abp.Events.Bus.Handlers;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common;
using MMK.SmartSystem.Common.EventDatas;
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
    public class UserLanguageHandler : BaseTranslate,IEventHandler<UserLanguageEventData>, ITransientDependency
    {

        public void HandleEvent(UserLanguageEventData eventData)
        {
            if (string.IsNullOrEmpty(eventData.Culture))
            {
                return;
            }

            UserClientServiceProxy userClientService = new UserClientServiceProxy(SmartSystemCommonConsts.ApiHost, new System.Net.Http.HttpClient());
            try
            {
                userClientService.ChangeLanguageAsync(new ChangeUserLanguageDto() { LanguageName = eventData.Culture }).Wait();
                var tokenAuthClient = new TokenAuthClient(SmartSystemCommonConsts.ApiHost, new System.Net.Http.HttpClient());


                var obj2 = tokenAuthClient.GetUserConfiguraionAsync().Result;
                if (obj2.Success)
                {
                    SmartSystemCommonConsts.UserConfiguration = obj2.Result;
                    Translate();
                }
                else
                {
                    Messenger.Default.Send(new MainSystemNoticeModel
                    {
                        Tagret = eventData.Tagret,
                        Error = obj2.Error?.Message,
                        ErrorAction = eventData.ErrorAction,
                        HashCode = eventData.HashCode

                    });
                }
            }
            catch (ApiException apiExcaption)
            {
                Messenger.Default.Send(new MainSystemNoticeModel
                {
                    Tagret = eventData.Tagret,
                    Error = apiExcaption.Message,
                    ErrorAction = eventData.ErrorAction,
                    HashCode = eventData.HashCode

                });
            }
            catch (Exception ex)
            {
                Messenger.Default.Send(new MainSystemNoticeModel
                {
                    Tagret = eventData.Tagret,
                    Error = ex.Message,
                    ErrorAction = eventData.ErrorAction,
                    HashCode = eventData.HashCode

                });

            }

        }
    }
}
