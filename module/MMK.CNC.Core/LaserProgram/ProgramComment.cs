using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MMK.CNC.Core.LaserProgram
{
    [Table("LaserProgram_ProgramComment")]
    public class ProgramComment : Entity<int>, IHasCreationTime
    {
        /// <summary>
        /// 文件唯一标识
        /// </summary>
        [StringLength(100)]
        public string FileHash { get; set; }

        /// <summary>
        /// 程序名称
        /// </summary>
        [StringLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// 程序路径
        /// </summary>
        [StringLength(100)]
        public string FullPath { get; set; }

        /// <summary>
        /// 程序大小
        /// </summary>
        public double Size { get; set; }

        /// <summary>
        /// 材料类型
        /// </summary>
        [StringLength(100)]
        public string Material { get; set; }

        /// <summary>
        /// 材料厚度
        /// </summary>
        public double Thickness { get; set; }

        /// <summary>
        /// 辅助气体种类
        /// </summary>
        [StringLength(100)]
        public string Gas { get; set; }

        /// <summary>
        /// 焦点位置
        /// </summary>
        public double FocalPosition { get; set; }

        /// <summary>
        /// 割嘴类型
        /// </summary>
        [StringLength(100)]
        public string NozzleKind { get; set; }

        /// <summary>
        /// 割嘴内径
        /// </summary>
        public double NozzleDiameter { get; set; }

        /// <summary>
        /// 板材尺寸
        /// </summary>
        [StringLength(100)]
        public string PlateSize { get; set; }

        /// <summary>
        /// 已用板材尺寸
        /// </summary>
        [StringLength(100)]
        public string UsedPlateSize { get; set; }

        /// <summary>
        /// 切割总长度
        /// </summary>
        public double CuttingDistance { get; set; }

        /// <summary>
        /// 穿孔总次数
        /// </summary>
        public int PiercingCount { get; set; }

        /// <summary>
        /// 加工时间
        /// </summary>
        public double CuttingTime { get; set; }

        /// <summary>
        /// 缩略图类型
        /// </summary>
        public int ThumbnaiType { get; set; }

        public string ThumbnaiInfo { get; set; }

        /// <summary>
        /// 程序更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        public double? PlateSize_W { get; set; }

        public double? PlateSize_H { get; set; }

        public double? UsedPlateSize_W { get; set; }

        public double? UsedPlateSize_H { get; set; }

        public double? Max_X { get; set; }
        public double? Max_Y { get; set; }
        public double? Min_X { get; set; }
        public double? Min_Y { get; set; }
        public DateTime CreationTime { get; set; }
        public ProgramComment()
        {
            CreationTime = DateTime.Now;
        }
    }
}
