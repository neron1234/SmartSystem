using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.WebCommon.DeviceModel
{
    public class ReadProgramBlockModel:CncReadDecoplilersModel<string,string>
    {
    }

    public class ReadProgramBlockResultModel:BaseCncResultModel
    {
        public string Id { get; set; }

        public int Value { get; set; }

        public string ValueStr
        {
            get
            {
                return Value.ToString("00000000");
            }
        }
    }
}
