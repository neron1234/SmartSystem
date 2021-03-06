﻿using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using MMK.CNC.Core.LaserLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMK.CNC.Application.LaserLibrary.Dto
{

    public class EdgeCuttingDataDto : EntityDto<int>
    {
        /// <summary>
        /// 加工参数组编号
        /// </summary>
        public int MachiningDataGroupId { get; set; }

        /// <summary>
        /// 加工类型名称(外联字段)
        /// </summary>
        public string MachiningKindName { get; set; }

        /// <summary>
        /// 加工厚度(外联字段)
        /// </summary>
        public double MaterialThickness { get; set; }

        /// <summary>
        /// 加工材料名称(外联字段)
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 加工气体名称(外联字段)
        /// </summary>
        public string GasName { set; get; }

        /// <summary>
        /// 割嘴名称(外联字段)
        /// </summary>
        public string NozzleKindName { get; set; }

        /// <summary>
        /// E编号
        /// </summary>
        public short ENo { get; set; }

        /// <summary>
        /// 加工类型ID
        /// </summary>
        public short MachiningKindCode { get; set; }

        /// <summary>
        /// 割嘴类型
        /// </summary>
        public short NozzleKindCode { get; set; }

        /// <summary>
        /// 割嘴内径
        /// </summary>
        public double NozzleDiameter { get; set; }

        /// <summary>
        /// Edge判定角度
        /// </summary>
        public double Angle { get; set; }

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
        public short GasCode { get; set; }

        /// <summary>
        /// 穿孔时间（毫秒）
        /// </summary>
        public int PiercingTime { get; set; }

        /// <summary>
        /// 复归距离
        /// </summary>
        public double RecoveryDistance { get; set; }

        /// <summary>
        /// 复归频率
        /// </summary>
        public short RecoveryFrequency { get; set; }

        /// <summary>
        /// 复归速度
        /// </summary>
        public double RecoveryFeedrate { get; set; }

        /// <summary>
        /// 复归占空比
        /// </summary>
        public short RecoveryDuty { get; set; }

        /// <summary>
        /// 间隙
        /// </summary>
        public double Gap { get; set; }

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
    }

    public class CreateEdgeCuttingDataDto
    {
        public int MachiningDataGroupId { get; set; }
    }

    public class UpdateEdgeCuttingDataDto : EntityDto<int>
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
        public short MachiningKindCode { get; set; }

        /// <summary>
        /// 割嘴类型
        /// </summary>
        public short NozzleKindCode { get; set; }

        /// <summary>
        /// 割嘴内径
        /// </summary>
        public double NozzleDiameter { get; set; }

        /// <summary>
        /// Edge判定角度
        /// </summary>
        public double Angle { get; set; }

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
        public short GasCode { get; set; }

        /// <summary>
        /// 穿孔时间（毫秒）
        /// </summary>
        public int PiercingTime { get; set; }

        /// <summary>
        /// 复归距离
        /// </summary>
        public double RecoveryDistance { get; set; }

        /// <summary>
        /// 复归频率
        /// </summary>
        public short RecoveryFrequency { get; set; }

        /// <summary>
        /// 复归速度
        /// </summary>
        public double RecoveryFeedrate { get; set; }

        /// <summary>
        /// 复归占空比
        /// </summary>
        public short RecoveryDuty { get; set; }

        /// <summary>
        /// 间隙
        /// </summary>
        public double Gap { get; set; }

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
    }

    public class EdgeCuttingDataResultRequestDto : PagedResultRequestDto
    {
        public int MachiningDataGroupId { get; set; }
        public override string ToString()
        {
            return $"{MachiningDataGroupId}{SkipCount}{MaxResultCount}";
        }
    }
}
