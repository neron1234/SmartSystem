using Abp.Dependency;
using Abp.Events.Bus.Handlers;
using MMK.SmartSystem.Common;
using MMK.SmartSystem.Common.EventDatas;
using MMK.SmartSystem.Common.SerivceProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.LE.Host.EventHandler
{
    public class OperationLogHandler: IEventHandler<OperationLogEventData>, ITransientDependency
    {
        public void HandleEvent(OperationLogEventData eventData)
        {
            OperationLogClientServiceProxy operationLogClientService = new OperationLogClientServiceProxy(SmartSystemCommonConsts.ApiHost, new System.Net.Http.HttpClient());
            operationLogClientService.CreateAsync(new OperationLogDto
            {
                UserId = (int)SmartSystemCommonConsts.AuthenticateModel.UserId,
                ClientName = GetIpAddress().Item1,
                ClientIpAddress = GetIpAddress().Item2,
                CreationTime = DateTime.Now,
                CustomData = eventData.CustomData,
                //接口以及参数
                ServiceName = eventData.ServiceName,
                Parameters = eventData.Parameters,
                //调用接口的返回值，报文，异常信息，执行时间，执行用时
                ReturnValue = eventData.ReturnValue,
                BrowserInfo = eventData.BrowserInfo,
                Exception = eventData.Exception,
                ExecutionDuration = eventData.ExecutionDuration,
                ExecutionTime = eventData.ExecutionTime,
                //日志来源信息，模块名，页面名，方法名
                ModuleName = eventData.ModuleName,
                PageName = eventData.PageName,
                MethodName = eventData.MethodName
            });
        }

        private Tuple<string,string> GetIpAddress()
        {
 　　        string hostName = Dns.GetHostName();   //获取本机名
 　　        IPHostEntry localhost = Dns.GetHostByName(hostName);    //方法已过期，可以获取IPv4的地址
 　　        //IPHostEntry localhost = Dns.GetHostEntry(hostName);   //获取IPv6地址
 　　        IPAddress localaddr = localhost.AddressList[0];
 
 　　        return new Tuple<string, string>(hostName,localaddr.ToString());
         }
    }
}
