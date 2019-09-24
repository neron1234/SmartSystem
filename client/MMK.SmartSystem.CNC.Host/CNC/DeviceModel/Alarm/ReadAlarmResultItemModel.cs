using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.CNC.Host.DeviceModel
{
    public class ReadAlarmResultItemModel
    {
        public int Num { get; set; }

        public string NumStr
        {
            get
            {
                return Num.ToString("0000");
            }
        }

        public short Ttype { get; set; }

        public string TtypeStr
        {
            get
            {
                string[] alm_type = { "SW", "PW", "IO", "PS", "OT", "OH", "SV", "SR", "MC", "SP", "DS", "IE", "BG", "SN", "", "EX", "", "", "", "PC" };

                return alm_type[Ttype];
            }
        }

        public short Axis { get; set; }

        public string AxisStr
        {
            get
            {
                return $"第{Axis}轴";
            }
        }

        public string Message { get; set; }
    }
}
