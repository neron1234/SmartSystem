using Abp.AspNetCore.SignalR.Hubs;
using Abp.Auditing;
using Abp.RealTime;
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
    }
}
