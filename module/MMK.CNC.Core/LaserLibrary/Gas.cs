using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MMK.CNC.Core.LaserLibrary
{
    [Table("LaserLibrary_Gas")]
    public class Gas : Entity<int>, IHasCreationTime
    {
        /// <summary>
        /// 气体编号
        /// </summary>
        public short Code { get; set; }

        /// <summary>
        /// 气体名称（英文）
        /// </summary>
        [StringLength(100)]
        public string Name_EN { get; set; }

        /// <summary>
        /// 气体名称（中文）
        /// </summary>
        [StringLength(100)]
        public string Name_CN { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string Description { get; set; }

        public DateTime CreationTime { get; set; }
        public Gas()
        {
            CreationTime = DateTime.Now;
        }
    }
}
