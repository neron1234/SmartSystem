using Abp.AspNetCore.SignalR;
using Abp.BackgroundJobs;
using Abp.Hangfire;
using Abp.Hangfire.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Threading.BackgroundWorkers;
using Hangfire;
using MMK.SmartSystem.CNC.Core;
using MMK.SmartSystem.RealTime.Job;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMK.SmartSystem.RealTime
{
    [DependsOn(typeof(AbpAspNetCoreSignalRModule),
        typeof(SmartSytemCNCCoreModule))]
    public class SmartSystemRealTimeModule : AbpModule
    {
        private IBackgroundJobManager _backgroundJobManager;
        IBackgroundJobClient backgroundJobs;
        public SmartSystemRealTimeModule(IBackgroundJobClient backgroundJobs)
        {
            this.backgroundJobs = backgroundJobs;
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SmartSystemRealTimeModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            //_backgroundJobManager = IocManager.Resolve<IBackgroundJobManager>();

            //_backgroundJobManager.Enqueue<CncBackgroudJob, CncBackgroudArgs>(new CncBackgroudArgs());
            CncBackgroudJob cncBackgroudJob = IocManager.Resolve<CncBackgroudJob>();
            backgroundJobs.Enqueue(() => cncBackgroudJob.Execute(new CncBackgroudArgs()));
            //var workManager = IocManager.Resolve<IBackgroundWorkerManager>();
            //workManager.Add(IocManager.Resolve<CncBackgroudWorker>());
        }
    }
}
