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
    public class MaterialInfoHandler : BaseEventHandler<MaterialInfoEventData, List<MeterialGroupThicknessDto>>
    {
        public override RequestResult<List<MeterialGroupThicknessDto>> WebRequest(MaterialInfoEventData eventData)
        {
            MaterialClientServiceProxy materialClientServiceProxy = new MaterialClientServiceProxy(apiHost, httpClient);
            var res = materialClientServiceProxy.GetMaterialAllAsync(eventData.IsCheckSon, 0, 50).Result;
            return new RequestResult<List<MeterialGroupThicknessDto>>()
            {
                Result = res.Result.Items.ToList(),
                Error = res.Error,
                Success = res.Success,
                TargetUrl = res.TargetUrl,
                UnAuthorizedRequest = res.UnAuthorizedRequest
            };
        }
    }

    public class AddMaterialHandler : BaseEventHandler<AddMachiningInfoEventData, MaterialDto>
    {
        public override RequestResult<MaterialDto> WebRequest(AddMachiningInfoEventData eventData)
        {
            MaterialClientServiceProxy materialClientServiceProxy = new MaterialClientServiceProxy(apiHost, httpClient);
            var rs = materialClientServiceProxy.GetAllAsync(false, 0, 50).Result;

            if (rs.Success)
            {
                eventData.CreateMaterial.Code = rs.Result.Items.LastOrDefault() == null ? 1 : rs.Result.Items.LastOrDefault()?.Code + 1;
                var addRs = materialClientServiceProxy.CreateAsync(eventData.CreateMaterial).Result;
                return addRs;
            }
            return new RequestResult<MaterialDto>() { Success = false, Error = rs.Error };
        }
    }

    public class AddMaterialGroupHandler : BaseEventHandler<AddMachiningGroupInfoEventData, MachiningGroupDto>
    {
        public override RequestResult<MachiningGroupDto> WebRequest(AddMachiningGroupInfoEventData eventData)
        {
            MachiningGroupClientServiceProxy machiningGroupClientServiceProxy = new MachiningGroupClientServiceProxy(apiHost, httpClient);
            var rs = machiningGroupClientServiceProxy.GetAllAsync(eventData.CreateMachiningGroup.MaterialCode, 0, 50).Result;
            if (rs.Success && !rs.Result.Items.Any(n => n.MaterialThickness == eventData.CreateMachiningGroup.MaterialThickness))
            {
                eventData.CreateMachiningGroup.Code = rs.Result.Items.LastOrDefault() == null ? 1 : rs.Result.Items.LastOrDefault()?.Code + 1;
                var addRs = machiningGroupClientServiceProxy.CreateAsync(eventData.CreateMachiningGroup).Result;
                return addRs;
            }
            return new RequestResult<MachiningGroupDto>() { Success = false, Error = rs.Error };
        }
    }
    public class MaterialGroupHandler : BaseEventHandler<MachiningGroupInfoEventData, List<MachiningGroupDto>>
    {
        public override RequestResult<List<MachiningGroupDto>> WebRequest(MachiningGroupInfoEventData eventData)
        {
            MachiningGroupClientServiceProxy machiningGroupClientServiceProxy = new MachiningGroupClientServiceProxy(SmartSystemCommonConsts.ApiHost, new System.Net.Http.HttpClient());
            var res = machiningGroupClientServiceProxy.GetAllAsync(eventData.MaterialId, 0, 50).Result;
            return new RequestResult<List<MachiningGroupDto>>()
            {
                Result = res.Result.Items.ToList(),
                Error = res.Error,
                Success = res.Success,
                TargetUrl = res.TargetUrl,
                UnAuthorizedRequest = res.UnAuthorizedRequest
            };
        }
    }
    public class DeleteMachiningHandler : BaseEventHandler<DeleteMachiningInfoEventData, object>
    {
        public override RequestResult<object> WebRequest(DeleteMachiningInfoEventData eventData)
        {
            MaterialClientServiceProxy materialClientServiceProxy = new MaterialClientServiceProxy(apiHost, httpClient);
            materialClientServiceProxy.DeleteAsync(eventData.MaterialId).Wait();
            return new RequestResult<object>()
            {
                Success = true
            };
        }
    }

    public class DeleteMaterialGroupHandler : BaseEventHandler<DeleteMachiningGroupInfoEventData, object>
    {
        public override RequestResult<object> WebRequest(DeleteMachiningGroupInfoEventData eventData)
        {
            MachiningGroupClientServiceProxy machiningGroupClientServiceProxy = new MachiningGroupClientServiceProxy(apiHost, httpClient);
            machiningGroupClientServiceProxy.DeleteAsync(eventData.MachiningGroupId).Wait();
            return new RequestResult<object>()
            {
                Success = true
            };
        }
    }

}
