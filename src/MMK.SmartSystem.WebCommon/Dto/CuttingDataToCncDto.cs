using System;
using System.Collections.Generic;
using System.Text;

namespace MMK.SmartSystem.WebCommon.Dto
{
    public class CuttingDataToCncDto
    {
        public short ENo { get; set; }

        public double Feedrate { get; set; }

        public short Power { get; set; }

        public short Frequency { get; set; }

        public short Duty { get; set; }

        public double GasPressure { get; set; }

        public short GasCode { get; set; }// 辅助气体种类CODE

        public int GasSettingTime { get; set; }

        public double StandardDisplacement { get; set; }

        public double Supple { get; set; }

        public short EdgeSlt { get; set; }

        public short ApprSlt { get; set; }

        public short PwrCtrl { get; set; }

        public double StandardDisplacement2 { get; set; }

        public char GapAxis { get; set; }

        public double BeamSpot { get; set; }

        public double FocalPosition { get; set; }

        public double LiftDistance { get; set; }

        public short PbPower { get; set; }
    }
}
