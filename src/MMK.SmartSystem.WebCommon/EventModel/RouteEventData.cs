using Abp.Events.Bus;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMK.SmartSystem.WebCommon.EventModel
{
    public class RouteEventData:EventData
    {
        public string Url { get; set; }
    }
}
