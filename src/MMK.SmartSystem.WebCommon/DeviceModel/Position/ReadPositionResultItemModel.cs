using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.WebCommon.DeviceModel
{
    public class ReadPositionResultItemModel: BaseCncResultModel
    {
        [Newtonsoft.Json.JsonProperty("id")]

        public string Id { get; set; }

        [Newtonsoft.Json.JsonProperty("value")]

        public double Value { get; set; }
    }
}
