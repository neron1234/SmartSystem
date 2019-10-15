using Abp.Dependency;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common.Model;
using MMK.SmartSystem.Common.SignalrProxy;
using MMK.SmartSystem.Common.ViewModel;
using MMK.SmartSystem.WebCommon;
using MMK.SmartSystem.WebCommon.DeviceModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MMK.SmartSystem.Common.Base
{

    public abstract class SignalrPage : Page, ITransientDependency
    {
        SignalrProxyClient signalrProxyClient;

        public abstract void PageSignlarLoaded();

        public virtual string GetModule { get; }
        public IIocManager manager { set; get; }
        public abstract List<object> GetResultViewModelMap();
        public abstract void CncOnError(string message);
        public SignalrPage()
        {
            signalrProxyClient = new SignalrProxyClient(this.GetType().FullName + "_" + Guid.NewGuid().ToString("N"));
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
                        foreach (var prop in item.ViewModels.GetType().GetProperties())
                        {
                            var propValue = item?.AutoPropMapAction(list, prop.Name);
                            if (propValue != null)
                            {
                                prop.SetValue(item.ViewModels, propValue.ToString());
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
            Messenger.Default.Send(new BottomWarningLogViewModel() { WarningLogStr = obj });

            CncOnError(obj);
        }
        public virtual List<CncEventData> GetCncEventData()
        {
            var list = new List<CncEventData>();
            var module = SmartSystemCommonConsts.SignalrQueryParmModels.Where(d => d.Module == GetModule).FirstOrDefault();
            if (module == null || module.Pages?.Count == 0)
            {
                return list;
            }
            var page = module.Pages.Where(d => this.GetType().FullName.Contains(d.PageName)).FirstOrDefault();
            if (page == null)
            {
                return list;
            }
            foreach (var item in page.EventNodes)
            {
                var cncData = new CncEventData() { Kind = (CncEventEnum)Enum.Parse(typeof(CncEventEnum), item.Kind) };

                var commonModule = typeof(SmartSystemWebCommonModule).Assembly;
                var dyType = commonModule.GetType($"MMK.SmartSystem.WebCommon.DeviceModel.{item.Type}");
                if (dyType != null)
                {
                    cncData.Para = JsonConvert.SerializeObject(new
                    {
                        Readers = item.CncReadDecopliler.Readers?.Data ?? new List<object>(),
                        Decompilers = item.CncReadDecopliler.Decompilers?.Data ?? new List<object>()
                    });
                    list.Add(cncData);
                }

            }
            return list;
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
