using System;
using System.Collections.Generic;
using System.Text;

namespace MMK.SmartSystem.WebCommon.Dto
{
 
    public class SlopeControlDataToCncDto 
    {
        public short ENo { get; set; }

        public short PowerMin { get; set; }

        public short PowerSpeedZero { get; set; }

        public short FrequencyMin { get; set; }

        public short FrequencySpeedZero { get; set; }

        public short DutyMin { get; set; }

        public short DutySpeedZero { get; set; }

        public double FeedrateR { get; set; }

        public short PbPowerMin { get; set; }

        public short PbPowerSpeedZero { get; set; }

        public short GasPressMin { get; set; }

        public short GasPressSpeedZero { get; set; }
    }
}
