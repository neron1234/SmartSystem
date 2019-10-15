using Abp.AspNetCore.SignalR.Hubs;
using Abp.Auditing;
using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.RealTime;
using Microsoft.AspNetCore.SignalR;
using MMK.SmartSystem.CNC.Core;
using MMK.SmartSystem.CNC.Core.Workers;
using MMK.SmartSystem.RealTime.Job;
using MMK.SmartSystem.WebCommon.DeviceModel;
using MMK.SmartSystem.WebCommon.HubModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.RealTime.Hubs
{
    public class CNCHub : AbpCommonHub
    {
        public const string GetDataAction = "GetCNCData";
        public const string GetErrorAction = "GetError";
        public const string GetReadWriterAction = "GetReadWriter";
        IServiceProvider service;
        public CNCHub(IOnlineClientManager onlineClientManager, IClientInfoProvider clientInfoProvider, IServiceProvider _service) :
          base(onlineClientManager, clientInfoProvider)
        {
            service = _service;

        }

        public string SendReadWriter(HubReadWriterModel model)
        {
            var hubClient = service.GetService(typeof(IHubContext<CncClientHub>)) as IHubContext<CncClientHub>;
            if (hubClient != null)
            {
                model.ConnectId = Context.ConnectionId;
                hubClient.Clients.All.SendAsync(CncClientHub.ClientReadWriter, model);
            }
            return "True";
        }

        public Task Refresh(string info)
        {
            List<CncEventData> cncEvents = new List<CncEventData>();
            try
            {

                cncEvents = JsonConvert.DeserializeObject<List<CncEventData>>(info);
                string groupName = Context.GetHttpContext().Request.Query["groupName"].ToString();
                if (SmartSystemCNCCoreConsts.PageCncEventDict.ContainsKey(groupName))
                {
                    var listRes = SmartSystemCNCCoreConsts.PageCncEventDict[groupName];
                    listRes.AddRange(cncEvents);
                    var hubClient = service.GetService(typeof(IHubContext<CncClientHub>)) as IHubContext<CncClientHub>;
                    if (hubClient != null)
                    {
                        hubClient.Clients.All.SendAsync(CncClientHub.ClientGetCncEvent, new List<GroupEventData>() {
                            new GroupEventData() { GroupName = groupName, Data = listRes,Operation=GroupEventOperationEnum.Add } });
                    }
                }

                Logger.Info($"【CncConfig】:{info}");


            }
            catch (Exception ex)
            {

                Logger.Error(ex.Message);
            }

            return Task.CompletedTask;
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                string groupName = Context.GetHttpContext().Request.Query["groupName"].ToString();
                if (SmartSystemCNCCoreConsts.PageCncEventDict.ContainsKey(groupName))
                {
                    var nodes = new List<CncEventData>();
                    SmartSystemCNCCoreConsts.PageCncEventDict.TryRemove(groupName, out nodes);
                    var hubClient = service.GetService(typeof(IHubContext<CncClientHub>)) as IHubContext<CncClientHub>;
                    if (hubClient != null)
                    {
                        hubClient.Clients.All.SendAsync(CncClientHub.ClientGetCncEvent,
                            new List<GroupEventData>()
                            {
                              new GroupEventData()
                              {
                                  GroupName = groupName,
                                  Operation =GroupEventOperationEnum.Remove
                              } });
                    }


                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);

            }
            return base.OnDisconnectedAsync(exception);
        }
        public override Task OnConnectedAsync()
        {
            try
            {
                string groupName = Context.GetHttpContext().Request.Query["groupName"].ToString();
                if (!SmartSystemCNCCoreConsts.PageCncEventDict.ContainsKey(groupName))
                {
                    var nodes = new List<CncEventData>();
                    SmartSystemCNCCoreConsts.PageCncEventDict.TryAdd(groupName, nodes);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);

            }
            return base.OnConnectedAsync();
        }
    }
}
