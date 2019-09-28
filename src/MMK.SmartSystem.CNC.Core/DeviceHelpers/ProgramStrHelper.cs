using MMK.SmartSystem.WebCommon.DeviceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.CNC.Core.DeviceHelpers
{
    public static class ProgramStrHelper
    {

        public static Tuple<short, string> ReadProgramStr(ushort flib, ref ReadProgramStrResultModel data)
        {
            //program string 
            ushort charnum = 2048;
            short blocknum = 200;
            StringBuilder buf = new StringBuilder(2048);
            var ret = Focas1.cnc_rdexecprog(flib, ref charnum, out blocknum, buf);


            if (ret == 0)
            {
                data.Value = buf.ToString();

                return new Tuple<short, string>(0, null);
            }
            else
            {
                return new Tuple<short, string>(ret, $"读取程序执行情况错误,返回:{ret}");
            }
        }
    }

}
