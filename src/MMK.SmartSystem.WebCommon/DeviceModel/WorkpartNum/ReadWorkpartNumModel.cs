using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.WebCommon.DeviceModel
{
    public class ReadWorkpartNumModel:CncReadDecoplilersModel<string,string>
    {        
    }

    public class ReadWorkpartNumResultModel:BaseCncResultModel
    {

        public int Value { get; set; }
    }
}
