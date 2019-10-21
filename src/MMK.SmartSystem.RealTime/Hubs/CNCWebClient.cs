using Abp.AspNetCore.SignalR.Hubs;
using Abp.Auditing;
using Abp.RealTime;
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
        public CNCWebClient(IOnlineClientManager onlineClientManager, IClientInfoProvider clientInfoProvider) :
        base(onlineClientManager, clientInfoProvider)
        {

        }

        public string PageOnLoad()
        {
            try
            {
                if (!SmartSystemCNCCoreConsts.PageCncEventDict.ContainsKey(DefaultGroupName))
                {
                    SmartSystemCNCCoreConsts.PageCncEventDict.TryAdd(DefaultGroupName, new HomeEventDataConfig().GetInitEventData());
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
