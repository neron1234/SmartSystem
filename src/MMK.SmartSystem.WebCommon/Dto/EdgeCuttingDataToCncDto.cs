using System;
using System.Collections.Generic;
using System.Text;

namespace MMK.SmartSystem.WebCommon.Dto
{
    public class EdgeCuttingDataToCncDto
    {
        public short ENo { get; set; }

        public double Angle { get; set; }

        public short Power { get; set; }

        public short Frequency { get; set; }

        public short Duty { get; set; }

        public double GasPressure { get; set; }

        public short GasCode { get; set; }

        public int PiercingTime { get; set; }

        public double RecoveryDistance { get; set; }

        public short RecoveryFrequency { get; set; }

        public double RecoveryFeedrate { get; set; }

        public short RecoveryDuty { get; set; }

        public double Gap { get; set; }

        public char GapAxis { get; set; }

        public short PbPower { get; set; }
    }
}
