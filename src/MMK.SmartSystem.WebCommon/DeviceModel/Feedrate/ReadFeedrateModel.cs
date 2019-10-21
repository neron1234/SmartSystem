using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.WebCommon.DeviceModel
{
    public class ReadFeedrateModel : CncReadDecoplilersModel<string, string>
    {
        //public List<string> Decompilers { get; set; } = new List<string>();
    }

    public class ReadFeedrateResultModel : BaseCncResultModel
    {
        [Newtonsoft.Json.JsonProperty("id")]
        public string Id { get; set; }

        [Newtonsoft.Json.JsonProperty("value")]
        public int Value { get; set; }
    }
}
