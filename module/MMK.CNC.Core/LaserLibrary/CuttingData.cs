using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MMK.CNC.Core.LaserLibrary
{
    [Table("LaserLibrary_CuttingData")]
    public class CuttingData : Entity<int>, IHasCreationTime
    {
        /// <summary>
        /// 加工参数组编号
        /// </summary>
        public int MachiningDataGroupId { get; set; }

        /// <summary>
        /// E编号
        /// </summary>
        public short ENo { get; set; }

        /// <summary>
        /// 加工类型ID
        /// </summary>
        public int MachiningKindId { get; set; }

        /// <summary>
        /// 焦斑直径
        /// </summary>
        //public int MachiningKindId { get; set; }

        /// <summary>
        /// 气体ID
        /// </summary>
        public int GasId { get; set; }

        /// <summary>
        /// 割嘴内径
        /// </summary>
        public double NozzleDiameter { get; set; }

        public DateTime CreationTime { get; set; }
        public CuttingData()
        {
            CreationTime = DateTime.Now;
        }
    }
}
