using Abp;
using Abp.Castle.Logging.Log4Net;
using Castle.Facilities.Logging;
using MMK.SmartSystem.CNC.Core.Workers;
using MMK.SmartSystem.CNC.Host.Proxy;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MMK.SmartSystem.CNC.Host
{
    class Program
    {
        private static AbpBootstrapper _bootstrapper;
        static CncCoreWorker cncHandler;
        static SignalrProxy signalrProxy = new SignalrProxy();
        static DateTime currentTime = DateTime.Now;
        static void Main(string[] args)
        {
            _bootstrapper = AbpBootstrapper.Create<SmartSystemCNCHostModule>();
            _bootstrapper.IocManager.IocContainer.AddFacility<LoggingFacility>(
                f => f.UseAbpLog4Net().WithConfig("log4net.config"));
            _bootstrapper.Initialize();
            StartWorker();
            Task.Factory.StartNew(StartSignalr);
            Task.Factory.StartNew(() =>
            {

                while (true)
                {
                    try
                    {
                        cncHandler.Execute();

                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine("【Worker Error】" + ex.Message + " " + ex.InnerException?.Message);

                    }
                    Thread.Sleep(100);
                }
            });
            while (true)
            {
                Console.ReadLine();
            }
        }

        static void StartWorker()
        {
            cncHandler = new CncCoreWorker(_bootstrapper.IocManager);
            cncHandler.ShowErrorLogEvent += CncHandler_ShowErrorLogEvent;
            cncHandler.GetResultEvent += CncHandler_GetResultEvent;
        }

        private async static void CncHandler_GetResultEvent(object obj)
        {
            if ((DateTime.Now - currentTime).TotalSeconds >= 5)
            {
                Console.WriteLine($"【Worker Data】【{DateTime.Now.ToString("HH:mm:ss")}】" + JObject.FromObject(obj).ToString());
                currentTime = DateTime.Now;
            }

            await signalrProxy.SendAction<string>(SmartSystemCNCHostConsts.ClientSuccessEvent, obj);
        }

        private async static void CncHandler_ShowErrorLogEvent(string obj)
        {
            Console.WriteLine("【Worker Error】" + obj);
            await signalrProxy.SendAction<string>(SmartSystemCNCHostConsts.ClientErrorEvent, obj);

        }

        static async void StartSignalr()
        {
            signalrProxy.CncErrorEvent += SignalrProxy_CncErrorEvent;
            signalrProxy.GetCncEventData += SignalrProxy_GetCncEventData;
            await signalrProxy.Start();
        }

        private static void SignalrProxy_GetCncEventData(List<WebCommon.DeviceModel.CncEventData> obj)
        {
            Console.WriteLine("【CncEventData】" + JArray.FromObject(obj).ToString());

            foreach (var item in obj)
            {
                CncCoreWorker.m_EventDatas.Add(item);

            }
        }

        private static void SignalrProxy_CncErrorEvent(string obj)
        {
            Console.WriteLine("【Signalr】" + obj);
        }
    }
}
