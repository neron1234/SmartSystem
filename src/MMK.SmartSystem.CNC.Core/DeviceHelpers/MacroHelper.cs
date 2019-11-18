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

            val = double.IsNaN(data[itemModel.Type][itemModel.RelStartNum]) ? 0 : data[itemModel.Type][itemModel.RelStartNum];

            return null;
        }

        public string WriteMacro(ushort flib, short mac_num, double data)
        {
            var decims = data.GetDecimals();
            var ret = Focas1.cnc_wrmacro(flib, mac_num, 10, decims.Item1, decims.Item2);

            if (ret != 0) {
                return $"写入宏变量失败，返回:{ret}";
            }

            return null;
        }
    }
}
