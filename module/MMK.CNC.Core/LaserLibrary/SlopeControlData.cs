using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MMK.CNC.Core.LaserLibrary
{
    [Table("LaserLibrary_SlopeControlData")]
    public class SlopeControlData : Entity<int>, IHasCreationTime
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
        /// 割嘴类型
        /// </summary>
        public int NozzleKindId { get; set; }

        /// <summary>
        /// 割嘴内径
        /// </summary>
        public double NozzleDiameter { get; set; }

        /// <summary>
        /// 最小功率
        /// </summary>
        public short PowerMin { get; set; }

        /// <summary>
        /// 功率速度0
        /// </summary>
        public short PowerSpeedZero { get; set; }

        /// <summary>
        /// 最小频率
        /// </summary>
        public short FrequencyMin { get; set; }

        /// <summary>
        /// 频率速度0
        /// </summary>
        public short FrequencySpeedZero { get; set; }

        /// <summary>
        /// 最小占空比
        /// </summary>
        public short DutyMin { get; set; }

        /// <summary>
        /// 占空比速度0
        /// </summary>
        public short DutySpeedZero { get; set; }

        /// <summary>
        /// 速度允许变化量
        /// </summary>
        public double FeedrateR { get; set; }

        /// <summary>
        /// 最小辅助气体压力
        /// </summary>
        public short PbPowerMin { get; set; }

        /// <summary>
        /// 谷底功率速度0
        /// </summary>
        public short PbPowerSpeedZero { get; set; }

        /// <summary>
        /// 最小辅助气体压力
        /// </summary>
        public short GasPressMin { get; set; }

        /// <summary>
        /// 辅助气体压力速度0
        /// </summary>
        public short GasPressSpeedZero { get; set; }

        /// <summary>
        /// 焦斑直径
        /// </summary>
        public double BeamSpot { get; set; }

        /// <summary>
        /// 焦点位置
        /// </summary>
        public double FocalPosition { get; set; }

        /// <summary>
        /// 蛙跳高度
        /// </summary>
        public double LiftDistance { get; set; }

        [StringLength(100)]
        public string Reserve1 { get; set; }

        [StringLength(100)]
        public string Reserve2 { get; set; }

        [StringLength(100)]
        public string Reserve3 { get; set; }

        public DateTime CreationTime { get; set; } = DateTime.Now;

        public SlopeControlData()
        {
            CreationTime = DateTime.Now;
        }
        public SlopeControlData(int index)
        {

        }
    }
}
