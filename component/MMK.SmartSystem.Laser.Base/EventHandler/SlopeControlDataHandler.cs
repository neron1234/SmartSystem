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
    public class UpdateSlopeControlDataHandler : BaseEventHandler<UpdateSlopeControlDataEventData, SlopeControlDataDto>
    {
        public override RequestResult<SlopeControlDataDto> WebRequest(UpdateSlopeControlDataEventData eventData)
        {
            SlopeControlDataClientServiceProxy slopeControlDataClientServiceProxy = new SlopeControlDataClientServiceProxy(apiHost, httpClient);
            return slopeControlDataClientServiceProxy.UpdateAsync(eventData.UpdateSlopeControlData).Result;
        }
    }

    public class SlopeControlDataHandler : BaseEventHandler<SlopeControlDataByGroupIdEventData, List<SlopeControlDataDto>>
    {
        public override RequestResult<List<SlopeControlDataDto>> WebRequest(SlopeControlDataByGroupIdEventData eventData)
        {
            SlopeControlDataClientServiceProxy slopeControlDataClientServiceProxy = new SlopeControlDataClientServiceProxy(apiHost, httpClient);
            var res = slopeControlDataClientServiceProxy.GetAllAsync(eventData.machiningDataGroupId, 0, 50).Result;
            return new RequestResult<List<SlopeControlDataDto>>()
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
