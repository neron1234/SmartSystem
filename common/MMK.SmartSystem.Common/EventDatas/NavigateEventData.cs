using Abp.Events.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Common.EventDatas
{
    public class NavigateEventData : EventData
    {
        public string Url { get; set; }

        public NavigateEnum NavigateType { get; set; }

        public WebRouteComponentDto ComponentDto { get; set; }


    }
    public enum NavigateEnum
    {
        Url,
        Component
    }
}
