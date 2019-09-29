using Abp.Dependency;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using Microsoft.AspNetCore.SignalR;
using MMK.SmartSystem.CNC.Core.Workers;
using MMK.SmartSystem.RealTime.Hubs;
using MMK.SmartSystem.WebCommon.HubModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MMK.SmartSystem.RealTime.Job
{
    public class CncBackgroudWorker : PeriodicBackgroundWorkerBase, ISingletonDependency
    {
        CncCoreWorker cncHandler = new CncCoreWorker(null);
        IHubContext<CNCHub> hubContext;
        DateTime dateTime = DateTime.Now;

        public CncBackgroudWorker(AbpTimer timer, IServiceProvider service, IIocManager _iocManager)
       : base(timer)
        {
            Timer.Period = 100;
            cncHandler = new CncCoreWorker(_iocManager);
            cncHandler.ShowErrorLogEvent += CncHandler_ShowErrorLogEvent;
            cncHandler.GetResultEvent += CncHandler_GetResultEvent;
            hubContext = service.GetService(typeof(IHubContext<CNCHub>)) as IHubContext<CNCHub>;          
        }

        private void CncHandler_GetResultEvent(object obj)
        {
            var res = new HubResultModel
            {
                Data = obj,
                Time = DateTime.Now.ToString("HH:mm:ss.ffff")
            };
            hubContext.Clients.All.SendAsync(CNCHub.GetDataAction, res);
            Logger.Info($"【Focas Result】{obj.ToString()}");
        }

        private void CncHandler_ShowErrorLogEvent(string obj)
        {
            hubContext.Clients.All.SendAsync(CNCHub.GetErrorAction, obj);
            Logger.Error($"【Focas】{ obj}");
        }

        protected override void DoWork()
        {
            cncHandler.Execute();


        }
    }
}
