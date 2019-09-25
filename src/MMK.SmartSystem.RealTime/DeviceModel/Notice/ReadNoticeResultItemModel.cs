using System;
using System.Collections.Generic;
using System.Text;

namespace MMK.SmartSystem.RealTime.DeviceModel
{
    public class ReadNoticeResultItemModel
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
                if (Ttype == 4) return "宏";
                else return "系统";
            }
        }

        public string Message { get; set; }
    }
}
