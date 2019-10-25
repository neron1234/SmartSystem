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
    public class UpdateCuttingDataHandler : BaseEventHandler<UpdateCuttingDataEventData, CuttingDataDto>
    {
        public override RequestResult<CuttingDataDto> WebRequest(UpdateCuttingDataEventData eventData)
        {
            CuttingDataClientServiceProxy cuttingDataClientServiceProxy = new CuttingDataClientServiceProxy(apiHost, httpClient);
            return cuttingDataClientServiceProxy.UpdateAsync(eventData.UpdateCuttingData).Result;
        }
    }

    public class CuttingDataByGroupIdHandler : BaseEventHandler<CuttingDataByGroupIdEventData, List<CuttingDataDto>>
    {
        public override RequestResult<List<CuttingDataDto>> WebRequest(CuttingDataByGroupIdEventData eventData)
        {
            CuttingDataClientServiceProxy cuttingDataClientServiceProxy = new CuttingDataClientServiceProxy(apiHost, httpClient);
            var res = cuttingDataClientServiceProxy.GetAllAsync(eventData.machiningDataGroupId, 0, 50).Result;
            return new RequestResult<List<CuttingDataDto>>()
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
