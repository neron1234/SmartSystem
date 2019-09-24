using MMK.SmartSystem.CNC.Host.DeviceModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMK.SmartSystem.CNC.Host.DeviceHandlers
{
    public static class PositionHelper
    {
        public static Tuple<short, string> ReadPositionRange(ushort flib, CncPositionTypeEnum pos_type,  ref int[] data)
        {
            if (data.Length < Focas1.MAX_AXIS) return new Tuple<short, string>(-100, "读取位置信息错误,数据存储区域过小");

            Focas1.ODBAXIS buf = new Focas1.ODBAXIS();
            buf.data = new int[Focas1.MAX_AXIS];
            var ret = Focas1.cnc_absolute(flib, -1, 132, buf);

            if (ret == 0)
            {
                buf.data.CopyTo(data, 0);
                return new Tuple<short, string>(0, null);
            }

            return new Tuple<short, string>(ret, $"读取位置信号错误,返回:{ret}");
        }

        public static string DecompilerReadPositionInfo(int[] data, DecompReadPositionItemModel itemModel, ref int val)
        {
            if (data.Length < itemModel.AxisNum) return "无法获得信息,位置信息地址超出读取范围";

            val = data[itemModel.AxisNum - 1];

            return null;
        }
    }
}
