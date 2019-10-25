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
    public class UpdatePiercingDataHandler : BaseEventHandler<UpdatePiercingDataEventData, PiercingDataDto>
    {
        public override RequestResult<PiercingDataDto> WebRequest(UpdatePiercingDataEventData eventData)
        {
            PiercingDataClientServiceProxy piercingDataClientServiceProxy = new PiercingDataClientServiceProxy(apiHost, httpClient);
            return piercingDataClientServiceProxy.UpdateAsync(eventData.UpdatePiercingData).Result;
        }
    }

    public class PiercingDataHandler : BaseEventHandler<PiercingDataByGroupIdEventData, List<PiercingDataDto>>
    {
        public override RequestResult<List<PiercingDataDto>> WebRequest(PiercingDataByGroupIdEventData eventData)
        {
            PiercingDataClientServiceProxy piercingDataClientServiceProxy = new PiercingDataClientServiceProxy(apiHost, httpClient);
            var res = piercingDataClientServiceProxy.GetAllAsync(eventData.machiningDataGroupId, 0, 50).Result;
            return new RequestResult<List<PiercingDataDto>>()
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
