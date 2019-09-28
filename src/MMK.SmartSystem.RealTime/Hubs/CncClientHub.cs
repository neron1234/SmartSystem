using Abp.AspNetCore.SignalR.Hubs;
using Abp.Auditing;
using Abp.Dependency;
using Abp.RealTime;
using Microsoft.AspNetCore.SignalR;
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
        IServiceProvider service;
        IHubContext<CNCHub> hubClient;
        public CncClientHub(IOnlineClientManager onlineClientManager, IServiceProvider _service, IClientInfoProvider clientInfoProvider) :
        base(onlineClientManager, clientInfoProvider)
        {
            service = _service;
            hubClient = service.GetService(typeof(IHubContext<CNCHub>)) as IHubContext<CNCHub>;

        }

        public string PushErrorMessage(object info)
        {
            hubClient.Clients.All.SendAsync(CNCHub.GetErrorAction, info);
            return "True";
        }
        public string PushCncDataMessage(object info)
        {
            hubClient.Clients.All.SendAsync(CNCHub.GetDataAction, info);
            return "True";

        }
    }
}
