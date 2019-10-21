using Abp.AspNetCore.SignalR.Hubs;
using Abp.Auditing;
using Abp.RealTime;
using Microsoft.AspNetCore.SignalR;
using MMK.SmartSystem.CNC.Core;
using MMK.SmartSystem.CNC.Core.Configs;
using MMK.SmartSystem.WebCommon.DeviceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.RealTime.Hubs
{
    public class CNCWebClient : AbpCommonHub
    {
        const string DefaultGroupName = "Home-Config";
        IServiceProvider service;

        public CNCWebClient(IOnlineClientManager onlineClientManager, IClientInfoProvider clientInfoProvider, IServiceProvider _service) :
        base(onlineClientManager, clientInfoProvider)
        {
            service = _service;
        }

        public string PageOnLoad()
        {
            try
            {
                if (!SmartSystemCNCCoreConsts.PageCncEventDict.ContainsKey(DefaultGroupName))
                {
                    var list = new HomeEventDataConfig().GetInitEventData();
                    SmartSystemCNCCoreConsts.PageCncEventDict.TryAdd(DefaultGroupName, list);
                    var hubClient = service.GetService(typeof(IHubContext<CncClientHub>)) as IHubContext<CncClientHub>;
                    if (hubClient != null)
                    {
                        hubClient.Clients.All.SendAsync(CncClientHub.ClientGetCncEvent, new List<GroupEventData>() {
                            new GroupEventData() { GroupName = DefaultGroupName, Data = list,Operation=GroupEventOperationEnum.Add } });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);

            }
            return "True";
        }

        public string PageOnLeave()
        {
            try
            {
                if (SmartSystemCNCCoreConsts.PageCncEventDict.ContainsKey(DefaultGroupName))
                {
                    var list = new List<CncEventData>();
                    SmartSystemCNCCoreConsts.PageCncEventDict.TryRemove(DefaultGroupName, out list);
                  
                    var hubClient = service.GetService(typeof(IHubContext<CncClientHub>)) as IHubContext<CncClientHub>;
                    if (hubClient != null)
                    {
                        hubClient.Clients.All.SendAsync(CncClientHub.ClientGetCncEvent,
                            new List<GroupEventData>()
                            {
                              new GroupEventData()
                              {
                                  GroupName = DefaultGroupName,
                                  Operation =GroupEventOperationEnum.Remove
                              } });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);

            }
            return "True";
        }

        public override Task OnConnectedAsync()
        {

            return base.OnConnectedAsync();
        }

    }
}
