using Abp.Dependency;
using Abp.Events.Bus.Handlers;
using Microsoft.AspNetCore.SignalR;
using MMK.SmartSystem.RealTime.Hubs;
using MMK.SmartSystem.WebCommon.EventModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMK.SmartSystem.RealTime.EventHandlers
{
    public class RouteEventHandler : IEventHandler<RouteEventData>, ITransientDependency
    {
        IHubContext<RouteHub> hubContext;
        public RouteEventHandler(IServiceProvider service)
        {
            hubContext = service.GetService(typeof(IHubContext<RouteHub>)) as IHubContext<RouteHub>;
        }
        public void HandleEvent(RouteEventData eventData)
        {
            ushort flib;
            var ret = Focas1.cnc_allclibhndl3("192.168.1.1", 8193, 10, out flib);
            hubContext.Clients.All.SendAsync(RouteHub.ClientAction, eventData.Url);
        }
    }
}
