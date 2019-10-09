using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using MMK.CNC.Core.LaserLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.CNC.Application.LaserLibrary.Dto
{
    [AutoMap(typeof(MachiningKind))]
    public class MachiningKindDto : EntityDto<int>
    {
        public string Name_EN { get; set; }

        /// <summary>
        /// 加工类型名称（中文）
        /// </summary>
        public string Name_CN { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string Description { get; set; }

        public DateTime CreationTime { get; set; }

    }
    [AutoMap(typeof(MachiningKind))]

    public class UpdateMachiningKindDto : EntityDto<int>
    {
        public string Name_EN { get; set; }

        /// <summary>
        /// 加工类型名称（中文）
        /// </summary>
        public string Name_CN { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string Description { get; set; }
    }

    [AutoMap(typeof(MachiningKind))]
    public class CreateMachiningKindDto
    {
        public string Name_EN { get; set; }

        /// <summary>
        /// 加工类型名称（中文）
        /// </summary>
        public string Name_CN { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string Description { get; set; }
    }
}
