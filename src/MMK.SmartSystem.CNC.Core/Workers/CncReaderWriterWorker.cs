using Abp.Dependency;
using MMK.SmartSystem.WebCommon.HubModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.CNC.Core.Workers
{
    public class CncReaderWriterWorker
    {
        IIocManager iocManager;
        public CncReaderWriterWorker(IIocManager _iocManager)
        {
            iocManager = _iocManager;
        }
        public HubReadWriterResultModel DoWork(HubReadWriterModel hubRead)
        {
            try
            {
                var handlerType = Type.GetType($"MMK.SmartSystem.CNC.Core.DeviceInOut.{hubRead.ProxyName}");
                var handler = iocManager.Resolve(handlerType);
                var connMethod = handlerType.GetMethod("BuildConnect");
                var connRes = (bool)connMethod.Invoke(handler, null);
                if (!connRes)
                {
                    return new HubReadWriterResultModel()
                    {
                        Action = hubRead.Action,
                        ConnectId = hubRead.ConnectId,
                        Error = "CNC建立连接失败",
                        ProxyName = hubRead.ProxyName,
                        Result = new object(),
                        Success = false,
                        Id = hubRead.Id
                    };
                }
                var methodInfo = handlerType.GetMethod(hubRead.Action);
                var res = methodInfo.Invoke(handler, new object[] { hubRead }) as HubReadWriterResultModel ?? new HubReadWriterResultModel();
                res.Action = hubRead.Action;
                res.ConnectId = hubRead.ConnectId;
                res.Id = hubRead.Id;
                res.ProxyName = hubRead.ProxyName;
                return res;
            }
            catch (Exception ex)
            {

                return new HubReadWriterResultModel()
                {
                    Action = hubRead.Action,
                    ConnectId = hubRead.ConnectId,
                    Error = ex.Message + ex.InnerException?.Message,
                    ProxyName = hubRead.ProxyName,
                    Result = new object(),
                    Success = false,
                    Id = hubRead.Id

                };

            }


        }
    }
}
