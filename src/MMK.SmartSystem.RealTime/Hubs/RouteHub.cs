using Abp.AspNetCore.SignalR.Hubs;
using Abp.Auditing;
using Abp.RealTime;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMK.SmartSystem.RealTime.Hubs
{
    public class RouteHub : AbpCommonHub
    {
        public const string ClientAction = "GetRoute";
        public RouteHub(IOnlineClientManager onlineClientManager, IClientInfoProvider clientInfoProvider) :
          base(onlineClientManager, clientInfoProvider)
        {
        }

        public string NavHome()
        {

            Clients.All.SendAsync("GetRouterNav", "home");
            return "True";
        }
    }
}
