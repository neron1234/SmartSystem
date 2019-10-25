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
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.LE.Host.EventHandler
{
    public class UserInfoHandler : BaseEventHandler<UserInfoEventData, UserInfo>
    {
        public override RequestResult<UserInfo> WebRequest(UserInfoEventData eventData)
        {
            UserClientServiceProxy userClientService = new UserClientServiceProxy(apiHost, httpClient);
            var res = userClientService.GetAsync(eventData.UserId).Result;
            if (res.Success)
            {
                SmartSystemCommonConsts.UserInfo = res.Result;
                Messenger.Default.Send(SmartSystemCommonConsts.UserInfo);
            }
            return res;
        }
    }
}
