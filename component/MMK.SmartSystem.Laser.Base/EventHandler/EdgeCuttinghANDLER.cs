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
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Laser.Base.EventHandler
{
    public class UpdateEdgeCuttingHandler : BaseEventHandler<UpdateEdgeCuttingEventData, EdgeCuttingDataDto>
    {
        public override RequestResult<EdgeCuttingDataDto> WebRequest(UpdateEdgeCuttingEventData eventData)
        {
            EdgeCuttingClientServiceProxy edgeCuttingClientServiceProxy = new EdgeCuttingClientServiceProxy(apiHost, httpClient);
            return edgeCuttingClientServiceProxy.UpdateAsync(eventData.UpdateEdgeCuttingData).Result;
        }
    }

    public class EdgeCuttingHandler : BaseEventHandler<EdgeCuttingByGroupIdEventData, List<EdgeCuttingDataDto>>
    {
        public override RequestResult<List<EdgeCuttingDataDto>> WebRequest(EdgeCuttingByGroupIdEventData eventData)
        {
            EdgeCuttingClientServiceProxy edgeCuttingClientServiceProxy = new EdgeCuttingClientServiceProxy(apiHost, httpClient);
            var res = edgeCuttingClientServiceProxy.GetAllAsync(eventData.machiningDataGroupId, 0, 50).Result;
            return new RequestResult<List<EdgeCuttingDataDto>>()
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
