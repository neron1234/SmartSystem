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
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.LE.Host.EventHandler
{
    public class UserInfoHandler : IEventHandler<UserInfoEventData>, ITransientDependency
    {
        public void HandleEvent(UserInfoEventData eventData)
        {
            UserClientServiceProxy userClientService = new UserClientServiceProxy(SmartSystemCommonConsts.ApiHost, new System.Net.Http.HttpClient());
            string errorMessage = "";
            try
            {
                var rs = userClientService.GetAsync(eventData.UserId).Result;
                errorMessage = rs.Error?.Details;

                if (rs.Success)
                {
                    SmartSystemCommonConsts.UserInfo = rs.Result;
                    Messenger.Default.Send(SmartSystemCommonConsts.UserInfo);
                    return;
                }
                Messenger.Default.Send(new MainSystemNoticeModel
                {
                    Tagret = eventData.Tagret,
                    Error = errorMessage,
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
