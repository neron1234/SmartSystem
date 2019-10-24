using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Common.SignalrProxy
{
    public class SignalrRouteProxyClient
    {
        public event Action<string> RouteErrorEvent;

        public event Action<string> GetHomeEvent;
        HubConnection connection;
        bool isExit = false;
        public SignalrRouteProxyClient()
        {
            connection = new HubConnectionBuilder()
              .WithUrl($"{SmartSystemCommonConsts.ApiHost}/hubs-routeHub")
              .Build();
            initEvent();
        }
        public async Task Start()
        {
            try
            {
                await connection.StartAsync();
            }
            catch (Exception ex)
            {
                RouteErrorEvent?.Invoke(ex.Message);

                await Task.Delay(5000);
                await Start();
            }
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

                RouteErrorEvent?.Invoke(ex.Message);
            }


        }


        private void initEvent()
        {
            connection.Closed += async (error) =>
            {
                if (!isExit)
                {
                    await Start();

                }
            };
            connection.On<string>(SinglarCNCHubConsts.GetRouterNavAvtion, (message) =>
            {
                GetHomeEvent?.Invoke(message);
            });

          
        }
    }
}
