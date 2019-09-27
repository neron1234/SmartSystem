﻿using Abp.BackgroundJobs;
using Abp.Dependency;
using Microsoft.AspNetCore.SignalR;
using MMK.SmartSystem.RealTime.DeviceHandlers;
using MMK.SmartSystem.RealTime.Hubs;
using MMK.SmartSystem.WebCommon.HubModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MMK.SmartSystem.RealTime.Job
{
    [Serializable]
    public class CncBackgroudArgs
    {

    }
    public class CncBackgroudJob : BackgroundJob<CncBackgroudArgs>, ITransientDependency
    {
        CncHandler cncHandler;
        IHubContext<CNCHub> hubContext;
        DateTime dateTime = DateTime.Now;

        public CncBackgroudJob(IServiceProvider service, IIocManager iocManager)
        {
            cncHandler = new CncHandler(iocManager);
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
            Logger.Info($"【Focas Result】{JObject.FromObject(res).ToString()}");

        }

        private void CncHandler_ShowErrorLogEvent(string obj)
        {
            hubContext.Clients.All.SendAsync(CNCHub.GetErrorAction, obj);
            Logger.Error($"【Focas】{ obj}");

        }


        public override void Execute(CncBackgroudArgs args)
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
