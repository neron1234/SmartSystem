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
    public class UpLoadProgramClientHandler : BaseEventHandler<UpLoadProgramClientEventData, object>
    {
        public override RequestResult<object> WebRequest(UpLoadProgramClientEventData eventData)
        {
            ProgramClientServiceProxy programClientService = new ProgramClientServiceProxy(apiHost, httpClient);
            var res = programClientService.UploadProgramAsync(eventData.FileParameter, eventData.ConnectId, eventData.FileHashCode).Result;
            return new RequestResult<object>()
            {
                Success = res.Success,
                Error = res.Error,
                Result = res.Result,
                TargetUrl = res.TargetUrl,
                UnAuthorizedRequest = res.UnAuthorizedRequest
            };
        }
    }

    public class UpdateProgramCientEventHandler : BaseEventHandler<UpdateProgramClientEventData, object>
    {
        public override RequestResult<object> WebRequest(UpdateProgramClientEventData eventData)
        {
            ProgramClientServiceProxy programClientService = new ProgramClientServiceProxy(apiHost, httpClient);
            var res = programClientService.UpdateAsync(eventData.Data).Result;
            return new RequestResult<object>()
            {
                Success = res.Success,
                Error = res.Error,
                Result = res.Result,
                TargetUrl = res.TargetUrl,
                UnAuthorizedRequest = res.UnAuthorizedRequest
            };
        }
    }

    public class QuaryProgramAllClientHandler : BaseEventHandler<ProgramClientEventData, List<ProgramCommentFromCncDto>>
    {
        public override RequestResult<List<ProgramCommentFromCncDto>> WebRequest(ProgramClientEventData eventData)
        {
            ProgramClientServiceProxy programClientService = new ProgramClientServiceProxy(apiHost, httpClient);
            var res = programClientService.GetAllAsync(0, 100).Result;
            return new RequestResult<List<ProgramCommentFromCncDto>>()
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
