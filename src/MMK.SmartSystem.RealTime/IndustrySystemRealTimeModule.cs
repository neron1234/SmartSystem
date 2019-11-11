using Abp.AspNetCore.SignalR;
using Abp.BackgroundJobs;
using Abp.Hangfire;
using Abp.Hangfire.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Threading.BackgroundWorkers;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMK.SmartSystem.RealTime
{
    [DependsOn(typeof(AbpAspNetCoreSignalRModule) )]
    public class SmartSystemRealTimeModule : AbpModule
    {
  
       
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SmartSystemRealTimeModule).GetAssembly());
        }
    }
}
