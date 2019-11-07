using MMK.SmartSystem.WebCommon.DeviceModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMK.SmartSystem.CNC.Core.DeviceHelpers
{
    public static class PositionHelper
    {
        public static Tuple<short, string> ReadPositionRange(ushort flib, CncPositionTypeEnum pos_type,  ref int[] data)
        {
            if (data.Length < Focas1.MAX_AXIS) return new Tuple<short, string>(-100, "读取位置信息错误,数据存储区域过小");

            Focas1.ODBAXIS buf = new Focas1.ODBAXIS();
            buf.data = new int[Focas1.MAX_AXIS];
            short ret = -100;

            switch (pos_type)
            {
                case CncPositionTypeEnum.Absolute:
                    ret = Focas1.cnc_absolute(flib, -1, 132, buf);
                    break;
                case CncPositionTypeEnum.Machine:
                    ret = Focas1.cnc_machine(flib, -1, 132, buf);
                    break;
                case CncPositionTypeEnum.Relative:
                    ret = Focas1.cnc_relative(flib, -1, 132, buf);
                    break;
                case CncPositionTypeEnum.Distance:
                    ret = Focas1.cnc_distance(flib, -1, 132, buf);
                    break;
                default:
                    ret = -100;
                    break;
            }

            if (ret == 0)
            {
                buf.data.CopyTo(data, 0);
                return new Tuple<short, string>(0, null);
            }

            return new Tuple<short, string>(ret, $"读取位置信号错误,返回:{ret}");
        }

        public static string DecompilerReadPositionInfo(int[] data, DecompReadPositionItemModel itemModel, ref int val)
        {
            if (itemModel.AxisNum > Focas1.MAX_AXIS || itemModel.AxisNum < 1) return "无法获得信息,轴号设定错误";
            if (data.Length < itemModel.AxisNum) return "无法获得信息,位置信息地址超出读取范围";

            val = data[itemModel.AxisNum - 1];

            return null;
        }
    }
}
