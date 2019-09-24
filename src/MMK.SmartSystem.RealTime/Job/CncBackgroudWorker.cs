using Abp.Dependency;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using Microsoft.AspNetCore.SignalR;
using MMK.SmartSystem.RealTime.DeviceHandlers;
using MMK.SmartSystem.RealTime.Hubs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MMK.SmartSystem.RealTime.Job
{
    public class CncBackgroudWorker : PeriodicBackgroundWorkerBase, ISingletonDependency
    {
        CncHandler cncHandler = new CncHandler();
        IHubContext<CNCHub> hubContext;
        DateTime dateTime = DateTime.Now;

        public CncBackgroudWorker(AbpTimer timer, IServiceProvider service)
       : base(timer)
        {
            Timer.Period = 100000000;
            cncHandler.ShowErrorLogEvent += CncHandler_ShowErrorLogEvent;
            cncHandler.GetResultEvent += CncHandler_GetResultEvent;
            hubContext = service.GetService(typeof(IHubContext<CNCHub>)) as IHubContext<CNCHub>;          
        }

        private void CncHandler_GetResultEvent(object obj)
        {
            hubContext.Clients.All.SendAsync(CNCHub.GetDataAction, obj);
        }

        private void CncHandler_ShowErrorLogEvent(string obj)
        {
            hubContext.Clients.All.SendAsync(CNCHub.GetErrorAction, obj);

        }

        protected override void DoWork()
        {
            while (true)
            {
                try
                {
                    cncHandler.Execute();

                }
                catch (Exception ex)
                {
                    if ((DateTime.Now - dateTime).TotalSeconds >= 5)
                    {
                        hubContext.Clients.All.SendAsync(CNCHub.GetErrorAction, ex.Message);

                        Logger.Error($"【Focas】{ ex.Message}");
                        dateTime = DateTime.Now;
                    }

                }
                Thread.Sleep(100);
            }
          
        }
    }
}
