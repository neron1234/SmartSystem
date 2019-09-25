using System;
using System.Collections.Generic;
using System.Text;

namespace MMK.SmartSystem.WebCommon.DeviceModel
{
    public class ReadPmcResultItemModel
    {
        [Newtonsoft.Json.JsonProperty("id")]

        public string Id { get; set; }

        [Newtonsoft.Json.JsonProperty("value")]

        public string Value { get; set; }

    }

}
