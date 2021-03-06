﻿using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MMK.CNC.Core.SystemClient
{
    [Table("SystemClient_OperationLog")]
    public class OperationLog : Entity<int>, IHasCreationTime
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 接口地址
        /// </summary>
        ///
        public string ServiceName { get; set; }
        /// <summary>
        /// 接口名称
        /// </summary>
        public string MethodName { get; set; }
        /// <summary>
        /// 参数
        /// </summary>
        public string Parameters { get; set; }
        /// <summary>
        /// 模块名称
        /// </summary>
        public string ModuleName { get; set; }
        /// <summary>
        /// 页面名称
        /// </summary>
        public string PageName { get; set; }
        /// <summary>
        /// 返回值
        /// </summary>
        public string ReturnValue { get; set; }
        /// <summary>
        /// 详细报文
        /// </summary>
        public string BrowserInfo { get; set; }
        /// <summary>
        /// 客户端IP地址
        /// </summary>
        public string ClientIpAddress { get; set; }
        /// <summary>
        /// 客户端名称
        /// </summary>
        public string ClientName { get; set; }
        /// <summary>
        /// 自定义数据
        /// </summary>
        public string CustomData { get; set; }
        /// <summary>
        /// 执行时间
        /// </summary>
        public DateTime ExecutionTime { get; set; }
        /// <summary>
        /// 执行持续时间
        /// </summary>
        public int ExecutionDuration { get; set; }
        /// <summary>
        /// 异常信息
        /// </summary>
        public string Exception { get; set; }

        public DateTime CreationTime { get; set; }
        public OperationLog()
        {
            CreationTime = DateTime.Now;
        }
    }
}
