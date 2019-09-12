using Abp.Dependency;
using Abp.Events.Bus.Handlers;
using MMK.SmartSystem.Common;
using MMK.SmartSystem.Common.EventDatas;
using MMK.SmartSystem.Common.SerivceProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TokenAuthClient = MMK.SmartSystem.Common.SerivceProxy.TokenAuthClient;
using System.Reflection;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common.Model;

namespace MMK.SmartSystem.LE.Host.EventHandler
{
    public class UserConfigHandler : IEventHandler<UserConfigEventData>, ITransientDependency
    {
        public void HandleEvent(UserConfigEventData eventData)
        {
            if (!string.IsNullOrEmpty(eventData.Culture))
            {
                SmartSystemCommonConsts.CurrentCulture = eventData.Culture;
            }
            TokenAuthClient tokenAuthClient = new TokenAuthClient(SmartSystemCommonConsts.ApiHost, new System.Net.Http.HttpClient());

            if (eventData.IsChangeUser)
            {
                var ts = tokenAuthClient.AuthenticateAsync(new AuthenticateModel() { UserNameOrEmailAddress = eventData.UserName, Password = eventData.Pwd }).Result;
                if (ts.Success)
                {
                    SmartSystemCommonConsts.AuthenticateModel = ts.Result;
                }
            }
            UserClientServiceProxy userClientService = new UserClientServiceProxy(SmartSystemCommonConsts.ApiHost, new System.Net.Http.HttpClient());

            Task.Factory.StartNew(() => {
                var rs = userClientService.GetAsync(SmartSystemCommonConsts.AuthenticateModel.UserId).Result;
                if (rs.Success)
                {
                    SmartSystemCommonConsts.UserInfo = rs.Result;
                    Messenger.Default.Send(SmartSystemCommonConsts.UserInfo);
                }
            });

            if (eventData.IsChangeLanguage)
            {
                userClientService.ChangeLanguageAsync(new ChangeUserLanguageDto() { LanguageName = eventData.Culture }).Wait();
            }

            OperationLogClientServiceProxy operationLogClientService = new OperationLogClientServiceProxy(SmartSystemCommonConsts.ApiHost, new System.Net.Http.HttpClient());

            tokenAuthClient = new TokenAuthClient(SmartSystemCommonConsts.ApiHost, new System.Net.Http.HttpClient());
            operationLogClientService.CreateAsync(new OperationLogDto {
                ExecutionTime = DateTime.Now, ExecutionDuration = 0,
                UserId = (int)SmartSystemCommonConsts.AuthenticateModel.UserId,
                ServiceName = SmartSystemCommonConsts.ApiHost,PageName= MethodBase.GetCurrentMethod().Name,
                MethodName = MethodBase.GetCurrentMethod().DeclaringType.Namespace
            });

            var obj2 = tokenAuthClient.GetUserConfiguraionAsync().Result;
            if (obj2.Success)
            {
                SmartSystemCommonConsts.UserConfiguration = obj2.Result;
                Translate();
            }
            else
            {
                //Messenger.Default.Send(new LoginMessage {
                //    Success
                //});

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

        }
    }
}
