using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.WebCommon.DeviceModel.LaserData
{
    public class ReadLaserDataResultModel
    {
        public double ActualFeed { get; set; }

        public short ActualPower { get; set; }

        public short ActualFreq { get; set; }

        public short ActualDuty { get; set; }

        public short CommandGasKind { get; set; }

        public short GasSettingTime { get; set; }

        public short ActualGasPress { get; set; }

    }
}
