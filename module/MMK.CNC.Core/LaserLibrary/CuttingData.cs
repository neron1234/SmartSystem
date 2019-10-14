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
        /// 辅助气体压力(MPa)
        /// </summary>
        public double GasPressure { get; set; }

        /// <summary>
        /// 辅助气体种类ID
        /// </summary>
        public int GasId { get; set; }

        /// <summary>
        /// 辅助气体上升时间（毫秒）
        /// </summary>
        public int GasSettingTime { get; set; }

        /// <summary>
        /// 基准偏移量
        /// </summary>
        public double StandardDisplacement { get; set; }

        /// <summary>
        /// 补偿量
        /// </summary>
        public double Supple { get; set; }

        /// <summary>
        /// Edge Cutting 选择
        /// </summary>
        public short EdgeSlt { get; set; }

        /// <summary>
        /// Start Up 选择
        /// </summary>
        public short ApprSlt { get; set; }

        /// <summary>
        /// Power Control 选择
        /// </summary>
        public short PwrCtrl { get; set; }

        /// <summary>
        /// 基准偏移量
        /// </summary>
        public double StandardDisplacement2 { get; set; }

        /// <summary>
        /// 间隙轴
        /// </summary>
        public char GapAxis { get; set; }

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

        /// <summary>
        /// 谷底功率
        /// </summary>
        public short PbPower { get; set; }

        [StringLength(100)]
        public string Reserve1 { get; set; }

        [StringLength(100)]
        public string Reserve2 { get; set; }

        [StringLength(100)]
        public string Reserve3 { get; set; }


        public DateTime CreationTime { get; set; }
        public CuttingData()
        {
            CreationTime = DateTime.Now;
        }
        public CuttingData(int index)
        {

        }

        public CuttingData(short index, int gasId, int machiningKindId, int nozzleKindId)
        {
            ENo = (short)(index + 1);
            MachiningKindId = machiningKindId;
            NozzleKindId = nozzleKindId;
            NozzleDiameter = 10;
            Feedrate = 100;
            Power = 10;
            Frequency = 10;
            Duty = 10;
            GasPressure = 10;
            GasId = gasId;
            GasSettingTime = 10;
            StandardDisplacement = 0;
            GapAxis = '\u0001';
            BeamSpot = 10;
            FocalPosition = 10;
            LiftDistance = 10;
            PbPower = 0;

            CreationTime = DateTime.Now;
        }
    }
}
