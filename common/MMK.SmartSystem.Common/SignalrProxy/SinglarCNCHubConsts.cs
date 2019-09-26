using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Common.SignalrProxy
{
    public class SinglarCNCHubConsts
    {
        /// <summary>
        /// 页面打开自动刷新代理
        /// </summary>
        public const string InitRefreshAction = "Refresh";

        /// <summary>
        /// 获取CNC自动刷新数据代理
        /// </summary>
        public const string CNCDataAction = "GetCNCData";

        /// <summary>
        /// 获取CNC刷新错误信息代理
        /// </summary>
        public const string CNCErrorAction = "GetError";

        /// <summary>
        /// 读取CNC程序列表
        /// </summary>
        public const string ReadProgramListAction = "ReadProgramList";
    }
}
