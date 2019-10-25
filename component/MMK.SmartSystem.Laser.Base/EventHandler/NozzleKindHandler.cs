using Abp.Dependency;
using Abp.Events.Bus.Handlers;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common;
using MMK.SmartSystem.Common.EventDatas;
using MMK.SmartSystem.Common.Model;
using MMK.SmartSystem.Common.SerivceProxy;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MMK.SmartSystem.Laser.Base.EventHandler
{
    public class NozzleKindHandler : BaseEventHandler<NozzleKindEventData, List<NozzleKindDto>>
    {
        public override RequestResult<List<NozzleKindDto>> WebRequest(NozzleKindEventData eventData)
        {
            NozzleKindClientServiceProxy nozzleKindClient = new NozzleKindClientServiceProxy(apiHost, httpClient);
            var res = nozzleKindClient.GetAllAsync(0, 50).Result;
            return new RequestResult<List<NozzleKindDto>>()
            {
                Result = res.Result.Items.ToList(),
                Error = res.Error,
                Success = res.Success,
                TargetUrl = res.TargetUrl,
                UnAuthorizedRequest = res.UnAuthorizedRequest
            };
        }
    }
}
