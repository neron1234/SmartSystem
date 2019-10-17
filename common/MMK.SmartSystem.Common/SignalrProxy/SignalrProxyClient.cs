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

        public event Action<string> CncErrorEvent;
        public event Action<HubResultModel> HubRefreshModelEvent;
        public string ConnectId;

        public event Action<HubReadWriterResultModel> HubReaderWriterResultEvent;
        HubConnection connection;
        bool isExit = false;
        public SignalrProxyClient(string groupName)
        {
            connection = new HubConnectionBuilder()
              .WithUrl($"{SmartSystemCommonConsts.ApiHost}/hubs-cncHub?groupName={groupName}")
              .Build();
            initEvent();
        }
        public async Task Start()
        {
            try
            {
                await connection.StartAsync();
                ConnectId = connection.ConnectionId;
            }
            catch (Exception ex)
            {

                CncErrorEvent?.Invoke(ex.Message);
            }
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
            catch (Exception ex)
            {

                CncErrorEvent?.Invoke(ex.Message);
            }


        }
        public void SendCncData(object cncEventDatas)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(cncEventDatas);
            try
            {
                connection.InvokeAsync(SinglarCNCHubConsts.InitRefreshAction, json);
            }
            catch (Exception ex)
            {

                CncErrorEvent?.Invoke(ex.Message);

            }
        }


        private void initEvent()
        {
            connection.Closed += async (error) =>
            {
                await Task.Delay(5000);
                if (!isExit)
                {
                    try
                    {
                        await connection.StartAsync();

                    }
                    catch (Exception ex)
                    {

                        CncErrorEvent?.Invoke(ex.Message);

                    }

                }
            };
            connection.On<string>(SinglarCNCHubConsts.CNCErrorAction, (message) =>
            {
                CncErrorEvent?.Invoke(message);
            });

            connection.On<HubResultModel>(SinglarCNCHubConsts.CNCDataAction, (data) =>
            {
                HubRefreshModelEvent?.Invoke(data);
            });
            connection.On<HubReadWriterResultModel>(SinglarCNCHubConsts.GetCNCReaderWriterResultAction, data =>{
                HubReaderWriterResultEvent?.Invoke(data);
            });
        }
    }
}
