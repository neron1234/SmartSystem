using Abp;
using Abp.Castle.Logging.Log4Net;
using Castle.Facilities.Logging;
using MMK.SmartSystem.CNC.Core;
using MMK.SmartSystem.CNC.Core.Workers;
using MMK.SmartSystem.CNC.Host.Proxy;
using MMK.SmartSystem.WebCommon.DeviceModel;
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
        static DateTime currentErrorTime = DateTime.Now;

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

                        Console.WriteLine($"【Worker Error】-【{DateTime.Now.ToString("HH:mm:ss")}】" + ex.Message + " " + ex.InnerException?.Message);

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
            await signalrProxy.SendAction<string>(SmartSystemCNCHostConsts.ClientSuccessEvent, obj);

            if ((DateTime.Now - currentTime).TotalSeconds >= 5)
            {
                Console.WriteLine($"【Worker Data】【{DateTime.Now.ToString("HH:mm:ss")}】" + JObject.FromObject(obj).ToString());
                currentTime = DateTime.Now;
            }

        }

        private async static void CncHandler_ShowErrorLogEvent(string obj)
        {
            await signalrProxy.SendAction<string>(SmartSystemCNCHostConsts.ClientErrorEvent, obj);

            if ((DateTime.Now - currentErrorTime).TotalSeconds >= 5)
            {
                currentErrorTime = DateTime.Now;

                Console.WriteLine($"【Worker Error】【{DateTime.Now.ToString("HH: mm:ss")}】" + obj);
            }


        }

        static async void StartSignalr()
        {
            signalrProxy.CncErrorEvent += SignalrProxy_CncErrorEvent;
            signalrProxy.GetCncEventData += SignalrProxy_GetCncEventData;
            await signalrProxy.Start();
        }

        private static void SignalrProxy_GetCncEventData(List<GroupEventData> obj)
        {
            Console.WriteLine("【CncEventData】" + JArray.FromObject(obj).ToString());

            foreach (var item in obj)
            {
                if (item.Operation == GroupEventOperationEnum.Add)
                {
                    if (!SmartSystemCNCCoreConsts.PageCncEventDict.ContainsKey(item.GroupName))
                    {
                        SmartSystemCNCCoreConsts.PageCncEventDict.TryAdd(item.GroupName, new List<CncEventData>());
                    }
                    SmartSystemCNCCoreConsts.PageCncEventDict[item.GroupName].AddRange(item.Data);
                }
                else if (item.Operation == GroupEventOperationEnum.Remove)
                {
                    var res = new List<CncEventData>();
                    SmartSystemCNCCoreConsts.PageCncEventDict.TryRemove(item.GroupName, out res);
                }
            }
        }

        private static void SignalrProxy_CncErrorEvent(string obj)
        {
            Console.WriteLine("【Signalr】" + obj);
        }
    }
}
