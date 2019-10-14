using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using MMK.CNC.Core.LaserLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMK.CNC.Application.LaserLibrary.Dto
{
    [AutoMap(typeof(PiercingData))]
    public class PiercingDataToCncDto : EntityDto<int>
    {
        public short ENo { get; set; }

        public short Power { get; set; }

        public short Frequency { get; set; }

        public short Duty { get; set; }

        public short StepFrequency { get; set; }

        public short StepDuty { get; set; }

        public short StepTime { get; set; }

        public short StepQuantity { get; set; }

        public int PiercingTime { get; set; }

        public double GasPressure { get; set; }

        public short GasCode { get; set; }

        public int GasSettingTime { get; set; }

        public double StandardDisplacement { get; set; }

        public double StandardDisplacement2 { get; set; }

        public char GapAxis { get; set; }

        public short PbPower { get; set; }
    }

    public class PiercingDataDto:EntityDto<int>
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
        /// 步进频率
        /// </summary>
        public short StepFrequency { get; set; }

        /// <summary>
        /// 步进占空比
        /// </summary>
        public short StepDuty { get; set; }

        /// <summary>
        /// 步进时间（毫秒）
        /// </summary>
        public short StepTime { get; set; }

        /// <summary>
        /// 步进次数
        /// </summary>
        public short StepQuantity { get; set; }

        /// <summary>
        /// 穿孔时间（毫秒）
        /// </summary>
        public int PiercingTime { get; set; }

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
    }

    public class CreatePiercingDataDto
    {
        public int MachiningDataGroupId { get; set; }
    }

    public class UpdatePiercingDataDto : EntityDto<int>
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
        /// 步进频率
        /// </summary>
        public short StepFrequency { get; set; }

        /// <summary>
        /// 步进占空比
        /// </summary>
        public short StepDuty { get; set; }

        /// <summary>
        /// 步进时间（毫秒）
        /// </summary>
        public short StepTime { get; set; }

        /// <summary>
        /// 步进次数
        /// </summary>
        public short StepQuantity { get; set; }

        /// <summary>
        /// 穿孔时间（毫秒）
        /// </summary>
        public int PiercingTime { get; set; }

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
    }

    public class PiercingDataResultRequestDto : PagedResultRequestDto
    {
        public int MachiningDataGroupId { get; set; }
    }
}
