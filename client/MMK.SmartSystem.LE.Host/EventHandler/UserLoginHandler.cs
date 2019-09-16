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
using System.Text;
using System.Threading.Tasks;
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
                        EventBus.Default.Trigger(new UserInfoEventData() { UserId = (int)ts.Result.UserId, Tagret = eventData.Tagret });
                        Messenger.Default.Send(new MainSystemNoticeModel
                        {
                            Tagret = eventData.Tagret,
                            Error = "",
                            Success=true,
                            SuccessAction=eventData.SuccessAction
                        });
                        return;
                    }
                }

                Messenger.Default.Send(new MainSystemNoticeModel
                {
                    Tagret = eventData.Tagret,
                    Error = errorMessage,
                    ErrorAction=eventData.ErrorAction
                });
            }
            catch (Exception ex)
            {
                Messenger.Default.Send(new MainSystemNoticeModel
                {
                    Tagret = eventData.Tagret,
                    Error = ex.Message,
                    ErrorAction = eventData.ErrorAction

                });

            }



        }
    }
}
