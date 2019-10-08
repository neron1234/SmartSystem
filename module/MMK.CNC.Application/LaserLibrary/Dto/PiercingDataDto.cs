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
}
