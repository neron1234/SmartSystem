using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MMK.CNC.Core.LaserLibrary
{
    [Table("LaserLibrary_Material")]
    public class Material : Entity<string>, IHasCreationTime
    {
        /// <summary>
        /// 材料编号
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 材料名称（英文）
        /// </summary>
        [StringLength(100)]
        public string Name_EN { get; set; }

        /// <summary>
        /// 材料名称（中文）
        /// </summary>
        [StringLength(100)]
        public string Name_CN { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string Description { get; set; }

        public DateTime CreationTime { get; set; }
        public Material()
        {
            CreationTime = DateTime.Now;
        }
    }
}
