using MMK.SmartSystem.WebCommon.DeviceModel;
using System;

namespace MMK.SmartSystem.RealTime.DeviceHandlers
{

    public static class CycleTimeHelper
    {
        public static Tuple<short, string> ReadCycleTime(ushort flib, ref ReadCycleTimeResultModel data)
        {
            Focas1.IODBPSD param1 = new Focas1.IODBPSD();
            var ret_cy1 = Focas1.cnc_rdparam(flib, 6757, -1, 8, param1);
            Focas1.IODBPSD param2 = new Focas1.IODBPSD();
            var ret_cy2 = Focas1.cnc_rdparam(flib, 6758, -1, 8, param2);

            if (ret_cy1 == 0 && ret_cy2 == 0)
            {
                data.TotalSeconds = param1.u.ldata / 1000 + param2.u.ldata * 60;

                return new Tuple<short, string>(0, null);
            }
            else
            {
                return new Tuple<short, string>(ret_cy1, $"读取加工工件信息错误,返回:{ret_cy1}(1),{ret_cy2}(2)");
            }
        }
    }
}
