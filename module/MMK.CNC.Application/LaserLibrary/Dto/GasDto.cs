﻿using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using MMK.CNC.Core.LaserLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.CNC.Application.LaserLibrary.Dto
{
    [AutoMap(typeof(Gas))]
    public class GasDto : EntityDto<int>
    {
        /// <summary>
        /// 气体编号
        /// </summary>
        public short Code { get; set; }

        /// <summary>
        /// 气体名称（英文）
        /// </summary>
        public string Name_EN { get; set; }

        /// <summary>
        /// 气体名称（中文）
        /// </summary>
        public string Name_CN { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string Description { get; set; }

        public DateTime CreationTime { get; set; }
    }
    [AutoMap(typeof(Gas))]
    public class CreateGasDto
    {
        /// <summary>
        /// 气体编号
        /// </summary>
        public short Code { get; set; }

        /// <summary>
        /// 气体名称（英文）
        /// </summary>
        public string Name_EN { get; set; }

        /// <summary>
        /// 气体名称（中文）
        /// </summary>
        public string Name_CN { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string Description { get; set; }
    }
    [AutoMap(typeof(Gas))]
    public class UpdateGasDto : EntityDto<int>
    {
        /// <summary>
        /// 气体编号
        /// </summary>
        public short Code { get; set; }

        /// <summary>
        /// 气体名称（英文）
        /// </summary>
        public string Name_EN { get; set; }

        /// <summary>
        /// 气体名称（中文）
        /// </summary>
        public string Name_CN { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string Description { get; set; }
    }

  
}
