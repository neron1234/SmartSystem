using Abp.AspNetCore.SignalR;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Threading.BackgroundWorkers;
using MMK.SmartSystem.RealTime.Job;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMK.SmartSystem.RealTime
{
    [DependsOn(typeof(AbpAspNetCoreSignalRModule))]
    public class SmartSystemRealTimeModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SmartSystemRealTimeModule).GetAssembly());
        }
        public override void PostInitialize()
        {
            //var workManager = IocManager.Resolve<IBackgroundWorkerManager>();
            //workManager.Add(IocManager.Resolve<CncBackgroudWorker>());
        }
    }
}
