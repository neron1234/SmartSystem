using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.WebCommon.DeviceModel
{
    public class ReadNoticeModel:CncReadDecoplilersModel<string,string>
    {
    }

    public class ReadNoticeResultModel:BaseCncResultModel
    {
        public string Id { get; set; }

        public List<ReadNoticeResultItemModel> Value { get; set; } = new List<ReadNoticeResultItemModel>();
    }

    public class ReadNoticeResultItemModel
    {
        public string Id { get; set; }

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
