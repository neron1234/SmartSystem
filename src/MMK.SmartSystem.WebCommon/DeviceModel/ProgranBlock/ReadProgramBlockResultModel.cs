using System;
using System.Collections.Generic;
using System.Text;

namespace MMK.SmartSystem.WebCommon.DeviceModel
{
    public class ReadProgramBlockResultModel
    {
        public string Id { get; set; }

        public int Value { get; set; }

        public string ValueStr {
            get {
                return Value.ToString("00000000");
            }
        }
    }
}
