using Microsoft.AspNetCore.SignalR.Client;
using MMK.SmartSystem.WebCommon.DeviceModel;
using MMK.SmartSystem.WebCommon.HubModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Common.SignalrProxy
{
    public class SignalrProxyClient
    {
        private const string GetCNCDataAction = "GetCNCData";
        private const string GetErrorAction = "GetError";
        public event Action<string> CncErrorEvent;
        public event Action<HubResultModel> HubRefreshModelEvent;
        HubConnection connection;
        bool isExit = false;
        public SignalrProxyClient()
        {
            connection = new HubConnectionBuilder()
              .WithUrl($"{SmartSystemCommonConsts.ApiHost}/hubs-cncHub")
              .Build();
            initEvent();
        }
        public async Task Start()
        {
            await connection.StartAsync();
        }

        public async Task<T> SendAction<T>(string actionName, object message)
        {
            return await connection.InvokeAsync<T>(actionName, message);
        }

        public async Task Close()
        {

            isExit = true;
            try
            {
                await connection.StopAsync();

            }
            catch (Exception)
            {


            }
        }
        public void SendCncData(List<CncEventData> cncEventDatas)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(cncEventDatas);
            try
            {
                connection.InvokeAsync("Refresh", json);


            }
            catch (Exception ex)
            {

                CncErrorEvent?.Invoke(ex.Message);

            }
        }
        void initEvent()
        {
            connection.Closed += async (error) =>
            {
                await Task.Delay(5000);
                if (!isExit)
                {
                    await connection.StartAsync();

                }
            };
            connection.On<string>(GetErrorAction, (message) =>
            {
                CncErrorEvent?.Invoke(message);
            });

            connection.On<HubResultModel>(GetCNCDataAction, (data) =>
            {
                HubRefreshModelEvent?.Invoke(data);
            });
        }
    }
}
