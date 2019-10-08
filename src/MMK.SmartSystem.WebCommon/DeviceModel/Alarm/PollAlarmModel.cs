using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.WebCommon.DeviceModel
{
    public class ReadAlarmModel : CncReadDecoplilersModel<string, string>
    {

    }

    public class ReadAlarmResultItemModel
    {
        [Newtonsoft.Json.JsonProperty("num")]
        public int Num { get; set; }

        [Newtonsoft.Json.JsonProperty("numStr")]

        public string NumStr
        {
            get
            {
                return Num.ToString("0000");
            }
        }

        [Newtonsoft.Json.JsonProperty("ttype")]

        public short Ttype { get; set; }

        [Newtonsoft.Json.JsonProperty("ttypeStr")]

        public string TtypeStr
        {
            get
            {
                string[] alm_type = { "SW", "PW", "IO", "PS", "OT", "OH", "SV", "SR", "MC", "SP", "DS", "IE", "BG", "SN", "", "EX", "", "", "", "PC" };

                return alm_type[Ttype];
            }
        }
        [Newtonsoft.Json.JsonProperty("axis")]

        public short Axis { get; set; }
        [Newtonsoft.Json.JsonProperty("axisStr")]

        public string AxisStr
        {
            get
            {

                return Axis != 0 ? $" 【第{Axis}轴】" : "";
            }
        }
        [Newtonsoft.Json.JsonProperty("message")]

        public string Message { get; set; }
    }

    public class ReadAlarmResultModel : BaseCncResultModel
    {
        public string Id { get; set; }

        public List<ReadAlarmResultItemModel> Value { get; set; } = new List<ReadAlarmResultItemModel>();
    }
}
