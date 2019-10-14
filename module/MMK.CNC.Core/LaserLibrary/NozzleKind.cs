using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MMK.CNC.Core.LaserLibrary
{
    [Table("LaserLibrary_NozzleKind")]
    public class NozzleKind : Entity<int>, IHasCreationTime
    {

        public short Code { get; set; }
        /// <summary>
        /// 割嘴类型名称（英文）
        /// </summary>
        [StringLength(100)]
        public string Name_EN { get; set; }

        /// <summary>
        /// 割嘴类型名称（中文）
        /// </summary>
        [StringLength(100)]
        public string Name_CN { get; set; }

        public DateTime CreationTime { get; set; }
        public NozzleKind()
        {
            CreationTime = DateTime.Now;
        }
    }
}
