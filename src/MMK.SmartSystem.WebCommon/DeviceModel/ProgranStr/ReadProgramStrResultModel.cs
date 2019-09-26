using System;
using System.Collections.Generic;
using System.Text;

namespace MMK.SmartSystem.WebCommon.DeviceModel
{
    public class ReadProgramStrResultModel: BaseCncResultModel
    {
        [Newtonsoft.Json.JsonProperty("value")]
        public string Value { get; set; }
    }
}
