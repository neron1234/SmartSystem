using MMK.SmartSystem.WebCommon.DeviceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.RealTime.DeviceHandlers
{
    public static class SpindleSpeedHelper
    {
        public static Tuple<short, string> ReadSpindleSpeed(ushort flib, ref ReadSpindleSpeedResultModel data)
        {
            Focas1.ODBACT buf = new Focas1.ODBACT();
            var ret = Focas1.cnc_acts(flib, buf);
            if (ret == 0)
            {
                data.Value = buf.data;


                return new Tuple<short, string>(0, null);
            }
            else
            {
                return new Tuple<short, string>(ret, $"读取主轴S信息错误,返回:{ret}");
            }
        }
    }
}
