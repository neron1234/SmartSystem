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
        public int FocalSpotDiameter { get; set; }

        /// <summary>
        /// 焦点位置
        /// </summary>
        public int FocalPosition { get; set; }

        /// <summary>
        /// 割嘴类型
        /// </summary>
        public int NozzleKindId { get; set; }

        /// <summary>
        /// 割嘴内径
        /// </summary>
        public double NozzleDiameter { get; set; }

        /// <summary>
        /// 速度
        /// </summary>
        public double Feedrate { get; set; }

        /// <summary>
        /// 功率
        /// </summary>
        public short Power { get; set; }

        /// <summary>
        /// 频率
        /// </summary>
        public short Frequency { get; set; }

        /// <summary>
        /// 占空比
        /// </summary>
        public short Duty { get; set; }

        /// <summary>
        /// 辅助气体压力
        /// </summary>
        public double GasPressure { get; set; }

        /// <summary>
        /// 辅助气体种类ID
        /// </summary>
        public int GasId { get; set; }

        /// <summary>
        /// 辅助气体上升时间
        /// </summary>
        public double GasSettingTime { get; set; }

        /// <summary>
        /// 基准偏移量
        /// </summary>
        public double StandardDisplacement { get; set; }

        /// <summary>
        /// 补偿量
        /// </summary>
        public double Cutting_Supple { get; set; }
        public short Cutting_Edge_Slt { get; set; }
        public short Cutting_Appr_Slt { get; set; }
        public short Cutting_Pwr_Ctrl { get; set; }
        public double Cutting_Displace_2 { get; set; }
        public string Cutting_Gap_Axis { get; set; }
        public string Cutting_Feed_Dec { get; set; }
        public string Cutting_Supple_Dec { get; set; }
        public string Cutting_Dsp2_Dec { get; set; }

        public DateTime CreationTime { get; set; }
        public CuttingData()
        {
            CreationTime = DateTime.Now;
        }
    }
}
