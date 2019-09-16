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
    public class UserLanguageHandler : IEventHandler<UserLanguageEventData>, ITransientDependency
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

        private void Translate()
        {
            var dict = SmartSystemCommonConsts.UserConfiguration.Localization.Values?.SmartSystem;
            var pageAuth = SmartSystemCommonConsts.UserConfiguration?.Auth?.GrantedPermissions ?? new Dictionary<string, string>();
            if (dict != null)
            {
                foreach (var item in SmartSystemLEConsts.SystemModules)
                {
                    item.ModuleName = item.ModuleKey.Translate();
                    bool isAuth = false;
                    foreach (var g in item.MainMenuViews)
                    {
                        g.Title = g.PageKey.Translate();
                        if (g.Auth)
                        {
                            if (pageAuth.ContainsKey(g.Permission))
                            {
                                g.Show = Visibility.Visible;
                                isAuth = true;
                            }
                            else
                            {
                                g.Show = Visibility.Collapsed;
                            }
                        }
                        else
                        {
                            isAuth = true;
                            g.Show = Visibility.Visible;

                        }

                    }
                    item.Show = isAuth ? Visibility.Visible : Visibility.Collapsed;
                }
            }

            //
            var smartGype = SmartSystemLEConsts.SystemTranslateModel.GetType();
            foreach (PropertyInfo item in smartGype.GetProperties())
            {
                var obj = item.GetValue(SmartSystemLEConsts.SystemTranslateModel, null);
                foreach (PropertyInfo propItem in item.PropertyType.GetProperties())
                {
                    string key = $"{item.Name}.{propItem.Name}";
                    if (dict.ContainsKey(key))
                    {
                        propItem.SetValue(obj, dict[key]);

                    }
                }
            }
        }
    }
}
