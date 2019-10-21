using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.WebCommon.DeviceModel
{
    public class ReadCycleTimeModel : CncReadDecoplilersModel<string, string>
    {
    }

    public class ReadCycleTimeResultModel : BaseCncResultModel
    {
        [Newtonsoft.Json.JsonProperty("id")]
        public string Id { get; set; }

        [Newtonsoft.Json.JsonProperty("value")]
        public int Value { get; set; }

        [Newtonsoft.Json.JsonProperty("cycleTimeStr")]
        public string CycleTimeStr
        {
            get
            {
                var hour = Math.Floor((double)Value / 3600);
                var min = Math.Floor((double)Value % 3600 / 60);
                var sec = Math.Floor((double)Value % 60);

                return (hour == 0 ? "" : (hour.ToString("00") + "H:")) + ((hour == 0 && min == 0) ? "" : (min.ToString("00") + "M: ")) + sec.ToString("00") + "S";

            }
        }
    }
}
