using Abp.Events.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Common.EventDatas
{
    public class OperationLogEventData : EventData
    {
        /// <summary>
        /// 模块名
        /// </summary>
        public string ModuleName { get; set; }
        /// <summary>
        /// 页面名
        /// </summary>
        public string PageName { get; set; }
        /// <summary>
        /// 方法名
        /// </summary>
        public string MethodName { get; set; }
        /// <summary>
        /// 自定义参数
        /// </summary>
        public string CustomData { get; set; }
        /// <summary>
        /// 接口名称
        /// </summary>
        public string ServiceName { get; set; }
        /// <summary>
        /// 接口参数
        /// </summary>
        public string Parameters { get; set; }
        /// <summary>
        /// 接口返回值
        /// </summary>
        public string ReturnValue { get; set; }
        /// <summary>
        /// api报文
        /// </summary>
        public string BrowserInfo { get; set; }
        /// <summary>
        /// 执行时间
        /// </summary>
        public DateTime ExecutionTime { get; set; }
        /// <summary>
        /// 执行用时
        /// </summary>
        public int ExecutionDuration { get; set; }
        /// <summary>
        /// 执行异常
        /// </summary>
        public string Exception { get; set; }
    }
}
