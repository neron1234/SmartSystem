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
    [AutoMap(typeof(Material))]

    public class MaterialDto : EntityDto<int>
    {
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 材料名称（英文）
        /// </summary>
        public string Name_EN { get; set; }

        /// <summary>
        /// 材料名称（中文）
        /// </summary>
        public string Name_CN { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string Description { get; set; }

        public DateTime CreationTime { get; set; }
    }

    public class MeterialGroupThicknessDto
    {
        public int Code { get; set; }

        public short MaterialCode { get; set; }
        public string Name_EN { get; set; }

        public string Name_CN { get; set; }

        public IEnumerable<ThicknessItem> ThicknessNodes { get; set; }
    }

    public class ThicknessItem
    {
        public int Id { get; set; }

        public double Thickness { get; set; }
    }
    public class MeterialThicknessDto : IEquatable<MeterialThicknessDto>
    {

        public int Id { get; set; }
        public int Code { get; set; }

        public short MaterialCode { get; set; }
        public string Name_EN { get; set; }

        public string Name_CN { get; set; }

        public double MaterialThickness { get; set; }

        public bool Equals(MeterialThicknessDto other)
        {
            return Code == other.Code && MaterialCode == other.MaterialCode && Name_CN == other.Name_CN && MaterialThickness == other.MaterialThickness;
        }

        public override int GetHashCode()
        {
            return $"{Code}_{MaterialThickness}_{MaterialCode}_{Name_CN}".GetHashCode();
        }
    }
    [AutoMap(typeof(Material))]

    public class UpdateMaterialDto : EntityDto<int>
    {
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 材料名称（英文）
        /// </summary>
        public string Name_EN { get; set; }

        /// <summary>
        /// 材料名称（中文）
        /// </summary>
        public string Name_CN { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string Description { get; set; }
    }
    [AutoMap(typeof(Material))]

    public class CreateMaterialDto
    {

        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 材料名称（英文）
        /// </summary>
        public string Name_EN { get; set; }

        /// <summary>
        /// 材料名称（中文）
        /// </summary>
        public string Name_CN { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string Description { get; set; }
    }

    public class MaterialResultRequestDto : PagedResultRequestDto
    {
        public bool IsCheckSon { get; set; }
    }
}
