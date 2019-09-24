using MMK.SmartSystem.RealTime.DeviceModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMK.SmartSystem.RealTime.DeviceHandlers
{
    public static class MacroHelper
    {

        public static Tuple<short, string> ReadMacroRange(ushort flib, ushort start, int num, ref double[] data)
        {
            if (num > 20) return new Tuple<short, string>(-100, "读取变量错误,读取数量超限");
            if (data.Length < num) return new Tuple<short, string>(-100, "读取变量错误,数据存储区域过小");

            var ret = Focas1.cnc_rdmacror2(flib, start, ref num, data);
            if (ret != 0) return new Tuple<short, string>(ret, $"读取变量错误,返回:{ret}");

            
            return new Tuple<short, string>(ret, $"读取变量错误,返回:{ret}");
        }

        public static string DecompilerReadMacroInfo(double[] data, DecompReadMacroItemModel itemModel,ref double val)
        {
            if (data.Length <= itemModel.RelStartNum) return "无法获得信息,变量地址超出读取范围";

            val = data[itemModel.RelStartNum];

            return null;
        }
    }
}
