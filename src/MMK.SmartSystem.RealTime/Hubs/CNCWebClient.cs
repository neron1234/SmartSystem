using Abp.AspNetCore.SignalR.Hubs;
using Abp.Auditing;
using Abp.RealTime;
using Microsoft.AspNetCore.SignalR;
using MMK.SmartSystem.WebCommon;
using MMK.SmartSystem.WebCommon.Configs;
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

        public async Task<string> PageOnLoad()
        {
            try
            {
                var list = new HomeEventDataConfig().GetInitEventData();

                if (!MMKSmartSystemWebCommonConsts.PageCncEventDict.ContainsKey(DefaultGroupName))
                {
                    MMKSmartSystemWebCommonConsts.PageCncEventDict.TryAdd(DefaultGroupName, list);

                }
                var hubClient = service.GetService(typeof(IHubContext<CncClientHub>)) as IHubContext<CncClientHub>;
                if (hubClient != null)
                {
                    var res = new List<GroupEventData>()
                    {
                            new GroupEventData() {
                                GroupName = DefaultGroupName, Data = list,Operation=GroupEventOperationEnum.Add
                            } };
                    await hubClient.Clients.All.SendAsync(CncClientHub.ClientGetCncEvent, res);
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);

            }
            return "True";
        }

        public async Task<string> PageOnLeave()
        {
            try
            {
                if (MMKSmartSystemWebCommonConsts.PageCncEventDict.ContainsKey(DefaultGroupName))
                {
                    var list = new List<CncEventData>();
                    MMKSmartSystemWebCommonConsts.PageCncEventDict.TryRemove(DefaultGroupName, out list);

                    var hubClient = service.GetService(typeof(IHubContext<CncClientHub>)) as IHubContext<CncClientHub>;
                    if (hubClient != null)
                    {
                        await hubClient.Clients.All.SendAsync(CncClientHub.ClientGetCncEvent,
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


    }
}
