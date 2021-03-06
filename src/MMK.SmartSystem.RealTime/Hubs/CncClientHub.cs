﻿using Abp.AspNetCore.SignalR.Hubs;
using Abp.Auditing;
using Abp.Dependency;
using Abp.RealTime;
using Microsoft.AspNetCore.SignalR;
using MMK.CNC.Application.LaserProgram;
using MMK.CNC.Application.LaserProgram.Dto;
using MMK.SmartSystem.WebCommon;
using MMK.SmartSystem.WebCommon.DeviceModel;
using MMK.SmartSystem.WebCommon.HubModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.RealTime.Hubs
{
    public class CncClientHub : AbpCommonHub
    {
        public const string ClientGetCncEvent = "GetCncEvent";
        public const string ClientReadWriter = "ReaderWriterEvent";


        public const string ClientReadProgram = "ReadProgramEvent";
        IServiceProvider service;
        IHubContext<CNCHub> hubClient;
        public CncClientHub(IOnlineClientManager onlineClientManager, IServiceProvider _service, IClientInfoProvider clientInfoProvider) :
        base(onlineClientManager, clientInfoProvider)
        {
            service = _service;
            hubClient = service.GetService(typeof(IHubContext<CNCHub>)) as IHubContext<CNCHub>;

        }

        public string UpdateProgramProxy(ProgramResolveResultDto programResolve)
        {
           

            hubClient.Clients.Client(programResolve.ConnectId).SendAsync(CNCHub.GetReadWriterAction, new HubReadWriterResultModel()
            {
                Result = programResolve.Data,
                Id = CncClientHub.ClientReadProgram,
                ConnectId = programResolve.ConnectId,
                Success = true
            });
            hubClient.Clients.Group("WEB").SendAsync("GetProgram", programResolve);

            return "True";
        }

        public string PushErrorMessage(object info)
        {
            hubClient.Clients.All.SendAsync(CNCHub.GetErrorAction, info);
            return "True";
        }

        public string PushCncDataMessage(object info)
        {
            var res = new HubResultModel
            {
                Data = info,
                Time = DateTime.Now.ToString("HH:mm:ss.ffff")
            };
            hubClient.Clients.All.SendAsync(CNCHub.GetDataAction, res);
            return "True";

        }

        public string PushReadWriter(HubReadWriterResultModel model)
        {
            hubClient.Clients.Client(model.ConnectId).SendAsync(CNCHub.GetReadWriterAction, model);
            hubClient.Clients.Group("WEB").SendAsync("GetReadWriter", model);

            return "True";
        }

        public override async Task OnConnectedAsync()
        {
           
            var list = MMKSmartSystemWebCommonConsts.PageCncEventDict.ToList().Select(d => new GroupEventData()
            {
                GroupName = d.Key,
                Data = d.Value,
                Operation = GroupEventOperationEnum.Add

            }).ToList();

            await Clients.Client(Context.ConnectionId).SendAsync(ClientGetCncEvent, list);

        }
    }
}
