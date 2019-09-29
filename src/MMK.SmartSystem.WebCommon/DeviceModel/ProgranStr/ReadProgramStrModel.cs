using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.WebCommon.DeviceModel
{
    public class ReadProgramStrModel:CncReadDecoplilersModel<string,string>
    {
    }

    public class ReadProgramStrResultModel : BaseCncResultModel
    {
        [Newtonsoft.Json.JsonProperty("id")]
        public string Id { get; set; }

        [Newtonsoft.Json.JsonProperty("value")]
        public string Value { get; set; }
    }
}
