using System;
using System.Collections.Generic;
using System.Text;

namespace MMK.SmartSystem.RealTime.DeviceModel
{
    public class ReadProgramInfoResultModel
    {
        public short RegeditProgramQuantity { get; set; }
        public short UnRegeditProgramQuantity { get; set; }

        public int UsedMemory { get; set; }

        public int UnUsedMemory { get; set; }
    }
}
