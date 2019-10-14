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
    [AutoMap(typeof(MachiningDataGroup))]
    public class MachiningGroupDto : EntityDto<int>
    {
        /// <summary>
        /// 加工参数组编号
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 材料类型ID
        /// </summary>
        public short MaterialCode { get; set; }

        /// <summary>
        /// 材料厚度
        /// </summary>
        public double MaterialThickness { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string Description { get; set; }

        public DateTime CreationTime { get; set; }
    }

    [AutoMap(typeof(MachiningDataGroup))]
    public class UpdateMachiningGroupDto : EntityDto<int>
    {

        /// <summary>
        /// 加工参数组编号
        /// </summary>
        public int Code { get; set; }

     

        /// <summary>
        /// 材料厚度
        /// </summary>
        public double MaterialThickness { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string Description { get; set; }
    }

    [AutoMap(typeof(MachiningDataGroup))]
    public class CreateMachiningGroupDto
    {
        /// <summary>
        /// 加工参数组编号
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 材料类型ID
        /// </summary>
        public short MaterialCode { get; set; }
        /// <summary>
        /// 材料厚度
        /// </summary>
        public double MaterialThickness { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string Description { get; set; }
    }

    public class MachiningGroupResultRequestDto : PagedResultRequestDto
    {
        public int MaterialCode { get; set; }
    }
}
