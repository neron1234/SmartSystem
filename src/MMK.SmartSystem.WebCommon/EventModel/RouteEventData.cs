using Abp.Events.Bus;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMK.SmartSystem.WebCommon.EventModel
{
    public class WebRouteComponentDto
    {
        public string WindowName { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public double PositionX { get; set; }

        public double PositionY { get; set; }

        public string ComponentUrl { get; set; }
    }
    public enum WebRouteEnum
    {
        Url,
        Component
    }
    public class RouteEventData : EventData
    {
        public WebRouteEnum RouteEnum { get; set; }
        public string Url { get; set; }

        public WebRouteComponentDto WebRoute { get; set; }

    }
}
