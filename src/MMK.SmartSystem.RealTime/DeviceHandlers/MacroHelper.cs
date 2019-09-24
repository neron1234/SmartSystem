using System;
using System.Collections.Generic;
using System.Text;

namespace MMK.SmartSystem.RealTime.DeviceHandlers
{
    public static class MacroHelper
    {
        //private static string GetMacroRange(ushort flib, short start, ushort num, double[] data)
        //{
        //    if (num > 20) return "读取变量错误,读取数量超限";

        //    Focas1.IODBMR buf = new Focas1.IODBMR();

        //    var end = (short)(start + num);
        //    var length = (short)(8 + 8 * num);

        //    var ret = Focas1.cnc_rdmacror(flib, start, end, length, buf);
        //    if (ret != 0) return $"读取变量错误,返回:{ret}";
        //    int mcr = buf.mcr_val;
        //    short dec = buf.dec_val;
        //    dec = (short)(dec * (-1));
        //    data = (double)(mcr * Math.Pow(10, dec));


        //    return 0;
        //}

        public static Tuple<short, string> GetMacroRange(ushort flib, short start, ushort num, ref double[] data)
        {
            if (num > 20) return new Tuple<short, string>(-100, "读取变量错误,读取数量超限");
            if (data.Length < num) return new Tuple<short, string>(-100, "读取变量错误,数据存储区域过小");

            Focas1.IODBMR buf = new Focas1.IODBMR();

            var end = (short)(start + num);
            var length = (short)(8 + 8 * num);

            var ret = Focas1.cnc_rdmacror(flib, start, end, length, buf);
            if (ret != 0) return new Tuple<short, string>(ret, $"读取变量错误,返回:{ret}");
            //int mcr = buf.mcr_val;
            //short dec = buf.dec_val;
            //dec = (short)(dec * (-1));
            //data = (double)(mcr * Math.Pow(10, dec));

            //buf.ldata = new int[num];
            //ushort adr_end = (ushort)(adr + num * 4 - 1);
            //var ret = Focas1.pmc_rdpmcrng(flib, adr_type, 2, adr, adr_end, 48, buf);

            //if (ret == 0)
            //{
            //    buf.ldata.CopyTo(data, 0);
            //    return new Tuple<short, string>(0, null);
            //}

            return new Tuple<short, string>(ret, $"读取PMC信号错误,返回:{ret}");
        }
    }
}
