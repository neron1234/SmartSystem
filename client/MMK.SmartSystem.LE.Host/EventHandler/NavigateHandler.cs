using Abp.Dependency;
using Abp.Events.Bus.Handlers;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common;
using MMK.SmartSystem.Common.EventDatas;
using MMK.SmartSystem.LE.Host.SystemControl.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.LE.Host.EventHandler
{
    public class NavigateHandler : IEventHandler<NavigateEventData>, ITransientDependency
    {
        public void HandleEvent(NavigateEventData eventData)
        {
            WebRouteClient webRouteClient = new WebRouteClient(SmartSystemCommonConsts.ApiHost, new System.Net.Http.HttpClient());
            if (eventData.NavigateType == NavigateEnum.Url)
            {
                webRouteClient.NavigateAsync(eventData.Url);
                return;
            }
            if (eventData.NavigateType == NavigateEnum.Component)
            {
                webRouteClient.NavigateComponentAsync(eventData.ComponentDto);
            }

        }
    }
}
