using Abp;
using Abp.Castle.Logging.Log4Net;
using Castle.Facilities.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.CNC.Host
{
    class Program
    {
        private static  AbpBootstrapper _bootstrapper;

        static void Main(string[] args)
        {
            _bootstrapper = AbpBootstrapper.Create<SmartSystemCNCHostModule>();
            _bootstrapper.IocManager.IocContainer.AddFacility<LoggingFacility>(
                f => f.UseAbpLog4Net().WithConfig("log4net.config"));
            _bootstrapper.Initialize();

        }


    }
}
