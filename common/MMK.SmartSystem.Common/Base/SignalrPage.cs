using Abp.Dependency;
using MMK.SmartSystem.Common.Model;
using MMK.SmartSystem.Common.SignalrProxy;
using MMK.SmartSystem.Common.ViewModel;
using MMK.SmartSystem.WebCommon.DeviceModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MMK.SmartSystem.Common.Base
{

    public abstract class SignalrPage : Page, ITransientDependency
    {
        SignalrProxyClient signalrProxyClient;

        public abstract void PageSignlarLoaded();

        public abstract List<CncEventData> GetCncEventData();
        public abstract List<object> GetResultViewModelMap();
        public abstract void CncOnError(string message);
        public SignalrPage()
        {
            signalrProxyClient = new SignalrProxyClient(this.GetType().FullName);
            signalrProxyClient.CncErrorEvent += SignalrProxyClient_CncErrorEvent;
            signalrProxyClient.HubRefreshModelEvent += SignalrProxyClient_HubRefreshModelEvent;
            this.Loaded += SignalrPage_Loaded;
            this.Unloaded += SignalrPage_Unloaded;
        }

        protected async Task<T> SendProxyAction<T>(string actionName, object message)
        {
            return await signalrProxyClient.SendAction<T>(actionName, message);
        }

        private async void SignalrPage_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            await signalrProxyClient.Close();
        }
        private void SignalrProxyClient_HubRefreshModelEvent(WebCommon.HubModel.HubResultModel obj)
        {
            JObject jobject = JObject.Parse(obj.Data.ToString());
            var listMap = GetResultViewModelMap();
            foreach (var d in listMap)
            {
                var item = d as dynamic;
                var list = item.ViewModels.GetCncResult(jobject);
                if (list.Count > 0)
                {
                    if (item.MapType == SignalrMapModelEnum.AutoPropMap)
                    {
                        foreach (var prop in item.GetType().GetProperties())
                        {
                            var propValue = item?.AutoPropMapAction(list, prop.Name);
                            if (propValue != null)
                            {
                                prop.SetValue(item, propValue.ToString());
                            }
                        }
                    }
                    else if (item.MapType == SignalrMapModelEnum.CustomAction)
                    {
                        item?.MapAction(list);
                    }

                }
            }
        }

        private void SignalrProxyClient_CncErrorEvent(string obj)
        {
            CncOnError(obj);
        }

        private async void SignalrPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            PageSignlarLoaded();
            await signalrProxyClient.Start();

            var cncEventDatas = GetCncEventData();
            if (cncEventDatas != null && cncEventDatas.Count > 0)
            {
                signalrProxyClient.SendCncData(cncEventDatas);

            }

        }
    }
}
