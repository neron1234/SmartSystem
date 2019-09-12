using Abp.Application.Services;
using Abp.Events.Bus;
using MMK.SmartSystem.WebCommon.EventModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMK.CNC.Application.SystemClient.WebApp
{
    public interface IWebRouteApplicationService : IApplicationService
    {
        void Navigate(string url);
    }
    public class WebRouteApplicationService : ApplicationService, IWebRouteApplicationService
    {
        public void Navigate(string url)
        {
            EventBus.Default.Trigger(new RouteEventData() { Url = url });
        }
    }
}
