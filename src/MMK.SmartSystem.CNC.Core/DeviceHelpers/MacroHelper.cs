using MMK.SmartSystem.WebCommon.DeviceModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMK.SmartSystem.CNC.Core.DeviceHelpers
{
    public class MacroHelper :BaseHelper
    {

        public Tuple<short, string> ReadMacroRange(ushort flib, ushort start, int num, ref double[] data)
        {
            if (num > 500) return new Tuple<short, string>(-100, "读取变量错误,读取数量超限");
            if (data.Length < num) return new Tuple<short, string>(-100, "读取变量错误,数据存储区域过小");

            var ret = Focas1.cnc_rdmacror2(flib, start, ref num, data);
            if (ret != 0) return new Tuple<short, string>(ret, $"读取变量错误,返回:{ret}");

            
            return new Tuple<short, string>(ret, $"读取变量错误,返回:{ret}");
        }

        public string DecompilerReadMacroInfo(Dictionary<CncMacroTypeEnum, double[]> data, DecompReadMacroItemModel itemModel,ref double val)
        {
            if(!data.ContainsKey(itemModel.Type)) return "无法获得信息,变量地址有误";

            
            if (data[itemModel.Type].Length <= itemModel.RelStartNum) return "无法获得信息,变量地址超出读取范围";

            val = data[itemModel.Type][itemModel.RelStartNum];

            return null;
        }

        public string WriteMacro(short mac_num, double data)
        {
            ushort flib = 0;
            short ret = BuildConnect(ref flib);
            if (ret != 0)
            {
                FreeConnect(flib);
                return "写入宏变量失败，连接错误";
            }

            var decims = data.GetDecimals();
            ret = Focas1.cnc_wrmacro(flib, mac_num, 10, decims.Item1, decims.Item2);
            FreeConnect(flib);

            if (ret != 0) {
                return $"写入宏变量失败，返回:{ret}";
            }

            return null;
        }
    }
}
