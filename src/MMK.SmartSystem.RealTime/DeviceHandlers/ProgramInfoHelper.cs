using MMK.SmartSystem.RealTime.DeviceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.RealTime.DeviceHandlers
{
    public static class ProgramInfoHelper
    {
        public static Tuple<short, string> ReadProgramInfo(ushort flib, ref ReadProgramInfoResultModel data)
        {
            Focas1.ODBNC_1 prginfo = new Focas1.ODBNC_1();
            var ret = Focas1.cnc_rdproginfo(flib, 0, 12, prginfo);
            if (ret == 0)
            {
                data.RegeditProgramQuantity = prginfo.reg_prg;
                data.UnRegeditProgramQuantity = prginfo.unreg_prg;
                data.UsedMemory = prginfo.used_mem;
                data.UnUsedMemory = prginfo.unused_mem;

                return new Tuple<short, string>(0, null);
            }
            else
            {
                return new Tuple<short, string>(ret, $"读取程序信息错误,返回:{ret}");
            }
        }
    }
}
