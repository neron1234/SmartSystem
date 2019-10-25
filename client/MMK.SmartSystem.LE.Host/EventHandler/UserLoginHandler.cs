using Abp.Dependency;
using Abp.Events.Bus;
using Abp.Events.Bus.Handlers;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common;
using MMK.SmartSystem.Common.EventDatas;
using MMK.SmartSystem.Common.Model;
using MMK.SmartSystem.Common.ViewModel;
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

    public class BaseTranslate
    {
        public void Translate()
        {
            var dict = SmartSystemCommonConsts.UserConfiguration.Localization.Values?.SmartSystem;
            if (dict != null)
            {
                //导航栏翻译以及权限
                foreach (var item in SmartSystemLEConsts.SystemModules)
                {
                    item.ModuleName = item.ModuleKey.Translate();
                    bool isAuth = false;
                    foreach (var g in item.MainMenuViews)
                    {
                        g.Title = g.PageKey.Translate();
                        if (g.Auth)
                        {
                            if (g.Permission.IsGrantedPermission())
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

            //文字翻译
            var smartGype = SmartSystemCommonConsts.SystemTranslateModel.GetType();
            foreach (PropertyInfo item in smartGype.GetProperties())
            {
                var obj = item.GetValue(SmartSystemCommonConsts.SystemTranslateModel, null);
                foreach (PropertyInfo propItem in item.PropertyType.GetProperties())
                {
                    string key = $"{item.Name}.{propItem.Name}";
                    if (dict.ContainsKey(key))
                    {
                        propItem.SetValue(obj, dict[key]);
                    }
                    var attr = propItem.GetCustomAttribute<LastTranslateLevelAttribute>();
                    if (attr != null)
                    {
                        var Pobj = propItem.GetValue(obj, null);

                        foreach (var sonPropItem in propItem.PropertyType.GetProperties())
                        {
                            string sonkey = $"{item.Name}.{propItem.Name}.{sonPropItem.Name}";
                            if (dict.ContainsKey(sonkey))
                            {
                                sonPropItem.SetValue(Pobj, dict[sonkey]);
                            }
                        }
                    }
                }
            }
        }

    }

    public class UserLoginHandler : BaseEventHandler<UserLoginEventData, AuthenticateResultModel>
    {
        public override RequestResult<AuthenticateResultModel> WebRequest(UserLoginEventData eventData)
        {
            TokenAuthClient tokenAuthClient = new TokenAuthClient(apiHost, httpClient);
            return tokenAuthClient.AuthenticateAsync(new AuthenticateModel() { UserNameOrEmailAddress = eventData.UserName, Password = eventData.Pwd }).Result;
            // 后续增加获取用户数据的 successAction
            //var obj2 = tokenAuthClient.GetUserConfiguraionAsync().Result;
            //errorMessage = obj2.Error?.Details;
            //if (obj2.Success)
            //{
            //    SmartSystemCommonConsts.UserConfiguration = obj2.Result;
            //    Translate();
            //    Messenger.Default.Send(new MainSystemNoticeModel
            //    {
            //        Tagret = eventData.Tagret,
            //        Error = "",
            //        Success = true,
            //        SuccessAction = eventData.SuccessAction,
            //        HashCode = eventData.HashCode
            //    });
            //    return;
            //}
        }
    }


}
