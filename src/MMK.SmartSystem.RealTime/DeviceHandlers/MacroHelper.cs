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
    }
}
