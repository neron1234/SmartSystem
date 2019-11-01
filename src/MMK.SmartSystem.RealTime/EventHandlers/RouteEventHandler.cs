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
            if (eventData.RouteEnum == WebRouteEnum.Url)
            {
                hubContext.Clients.All.SendAsync(RouteHub.ClientAction, eventData.Url);
                return;
            }
            if (eventData.RouteEnum == WebRouteEnum.Component)
            {
                hubContext.Clients.All.SendAsync(RouteHub.ClientDialogAction, eventData.WebRoute);

            }
        }
    }
}
