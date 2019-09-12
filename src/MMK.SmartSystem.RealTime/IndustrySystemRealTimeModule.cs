using Abp.AspNetCore.SignalR;
using Abp.Modules;
using Abp.Reflection.Extensions;
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
    }
}
