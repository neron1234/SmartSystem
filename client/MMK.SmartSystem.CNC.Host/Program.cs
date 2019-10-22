using Abp;
using Abp.Castle.Logging.Log4Net;
using Castle.Core.Logging;
using Castle.Facilities.Logging;
using MMK.SmartSystem.CNC.Core;
using MMK.SmartSystem.CNC.Core.DeviceHelpers;
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
        static Dictionary<string, DateTime> dictConsole = new Dictionary<string, DateTime>();
        static DateTime currentErrorTime = DateTime.Now;
        static CncReaderWriterWorker writerWorker;
        public static ILogger Logger { get; set; }

        static void Main(string[] args)
        {
            _bootstrapper = AbpBootstrapper.Create<SmartSystemCNCHostModule>();
            _bootstrapper.IocManager.IocContainer.AddFacility<LoggingFacility>(
                f => f.UseAbpLog4Net().WithConfig("log4net.config"));
            _bootstrapper.Initialize();
            Logger = _bootstrapper.IocManager.Resolve<ILogger>();
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
            writerWorker = new CncReaderWriterWorker(_bootstrapper.IocManager);
        }

        private async static void CncHandler_GetResultEvent(object obj)
        {
            await signalrProxy.SendAction<string>(SmartSystemCNCHostConsts.ClientSuccessEvent, obj);
            var jobj = JObject.FromObject(obj);
            string key = jobj["FullNamespace"].ToString();
            if (!dictConsole.ContainsKey(key))
            {
                dictConsole.Add(key, DateTime.Now);
            }
            if ((DateTime.Now - dictConsole[key]).TotalSeconds >= 5)
            {
                Console.WriteLine($"【Worker Data】【{DateTime.Now.ToString("HH:mm:ss")}】" + JObject.FromObject(obj).ToString());
                dictConsole[key] = DateTime.Now;
            }

        }

        private async static void CncHandler_ShowErrorLogEvent(string obj)
        {
            await signalrProxy.SendAction<string>(SmartSystemCNCHostConsts.ClientErrorEvent, obj);
            if ((DateTime.Now - currentErrorTime).TotalSeconds >= 5)
            {
                currentErrorTime = DateTime.Now;
                Logger.Error(obj);
                Console.WriteLine($"【Worker Error】【{DateTime.Now.ToString("HH: mm:ss")}】" + obj);
            }
        }

        static async void StartSignalr()
        {
            signalrProxy.CncErrorEvent += SignalrProxy_CncErrorEvent;
            signalrProxy.GetCncEventData += SignalrProxy_GetCncEventData;
            signalrProxy.GetClientReaderWriterEvent += SignalrProxy_GetClientReaderWriterEvent;
            signalrProxy.GetClientProgramResovleEvent += SignalrProxy_GetClientProgramResovleEvent;
            await signalrProxy.Start();
        }

        private static async void SignalrProxy_GetClientProgramResovleEvent(WebCommon.EventModel.ProgramResovleDto obj)
        {
            //obj.FilePath = @"C:\Users\wjj-yl\Desktop\测试用DXF\0001";
            //obj.BmpPath = @"C:\Users\wjj-yl\Desktop\测试用DXF\";

            var res = new LaserProgramDemo().ProgramResolve(obj);
            res.Data.FileHash = obj.FileHash;
            await signalrProxy.SendAction<string>(SmartSystemCNCHostConsts.ClientRrogramRosolveResultEvent, res);
            Console.WriteLine($"【程序解析】:{res.BmpName} | {res.Data.ToString()} | {obj.ConnectId} | {obj.FileHash}");
        }

        private static async void SignalrProxy_GetClientReaderWriterEvent(WebCommon.HubModel.HubReadWriterModel obj)
        {
            var res = writerWorker.DoWork(obj);
            if (!res.Success)
            {
                Console.WriteLine($"【{obj.ProxyName}-{obj.Action}】:【{DateTime.Now.ToString("HH:mm:ss")}】 {res.Error}");
            }
            await signalrProxy.SendAction<string>(SmartSystemCNCHostConsts.ClientReaderWriterResultEvent, res);

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
