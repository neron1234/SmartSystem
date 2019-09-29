using MMK.SmartSystem.WebCommon.DeviceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.CNC.Core.DeviceHelpers
{

    public static class FeedrateHelper
    {
        public static Tuple<short, string> ReadFeedrate(ushort flib, ref ReadFeedrateResultModel data)
        {
            Focas1.ODBACT buf = new Focas1.ODBACT();
            var ret = Focas1.cnc_actf(flib, buf);
            if (ret == 0)
            {
                data.Value = buf.data;


                return new Tuple<short, string>(0, null);
            }
            else
            {
                return new Tuple<short, string>(ret, $"读取进给F信息错误,返回:{ret}");
            }
        }
    }
}
