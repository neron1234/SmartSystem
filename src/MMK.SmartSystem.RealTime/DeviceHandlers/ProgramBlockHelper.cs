using MMK.SmartSystem.RealTime.DeviceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.RealTime.DeviceHandlers
{
    public static class ProgramBlockHelper
    {
        public static Tuple<short, string> ReadProgramBlock(ushort flib, ref ReadProgramBlockResultModel data)
        {
            Focas1.ODBSEQ seq = new Focas1.ODBSEQ();
            var ret = Focas1.cnc_rdseqnum(flib, seq);
            if (ret == 0)
            {
                data.Value = seq.data;

                return new Tuple<short, string>(0, null);
            }
            else
            {
                return new Tuple<short, string>(ret, $"读取程序段号错误,返回:{ret}");
            }
        }
    }
}
