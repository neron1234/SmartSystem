using MMK.SmartSystem.WebCommon.DeviceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.CNC.Core.DeviceHelpers
{
    public static class WorkpartNumHelper
    {
        public static Tuple<short, string> ReadWorkpartNum(ushort flib, ref ReadWorkpartNumResultModel data)
        {
            Focas1.IODBPSD param3 = new Focas1.IODBPSD();
            var ret = Focas1.cnc_rdparam(flib, 6711, 0, 8, param3);
            if (ret == 0)
            {
                data.Value = param3.u.ldata;


                return new Tuple<short, string>(0, null);
            }
            else
            {
                return new Tuple<short, string>(ret, $"读取加工工件信息错误,返回:{ret}");
            }
        }
    }
}
