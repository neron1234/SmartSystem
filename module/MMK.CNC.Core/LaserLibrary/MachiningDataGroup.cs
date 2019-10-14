using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MMK.CNC.Core.LaserLibrary
{
    [Table("LaserLibrary_MachiningDataGroup")]
    public class MachiningDataGroup : Entity<int>, IHasCreationTime
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
        public MachiningDataGroup()
        {
            CreationTime = DateTime.Now;
        }
    }
}
