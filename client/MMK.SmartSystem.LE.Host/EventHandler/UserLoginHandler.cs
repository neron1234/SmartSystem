using Abp.Dependency;
using Abp.Events.Bus;
using Abp.Events.Bus.Handlers;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common;
using MMK.SmartSystem.Common.EventDatas;
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
    public class UserLoginHandler : IEventHandler<UserLoginEventData>, ITransientDependency
    {
        public void HandleEvent(UserLoginEventData eventData)
        {
            TokenAuthClient tokenAuthClient = new TokenAuthClient(SmartSystemCommonConsts.ApiHost, new System.Net.Http.HttpClient());
            try
            {
                var ts = tokenAuthClient.AuthenticateAsync(new AuthenticateModel() { UserNameOrEmailAddress = eventData.UserName, Password = eventData.Pwd }).Result;
                string errorMessage = ts.Error?.Details;
                if (ts.Success)
                {
                    SmartSystemCommonConsts.AuthenticateModel = ts.Result;
                    var obj2 = tokenAuthClient.GetUserConfiguraionAsync().Result;
                    errorMessage = obj2.Error?.Details;
                    if (obj2.Success)
                    {
                        SmartSystemCommonConsts.UserConfiguration = obj2.Result;
                        Translate();
                        Messenger.Default.Send(new MainSystemNoticeModel
                        {
                            Tagret = eventData.Tagret,
                            Error = "",
                            Success=true,
                            SuccessAction=eventData.SuccessAction,
                            HashCode = eventData.HashCode
                        });
                        return;
                    }
                }
                Messenger.Default.Send(new MainSystemNoticeModel
                {
                    Tagret = eventData.Tagret,
                    Error = errorMessage,
                    ErrorAction=eventData.ErrorAction,
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
