using MMK.SmartSystem.RealTime.DeviceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.RealTime.DeviceHandlers
{
    public static class ProgramNameHelper
    {
        public static Tuple<short, string> ReadProgramName(ushort flib, ref ReadProgramNameResultModel data)
        {
            StringBuilder str = new StringBuilder();
            var ret = Focas1.cnc_pdf_rdmain(flib, str);
            if (ret == 0)
            {
                string[] sArray = str.ToString().Split('/');
                data.Name = sArray[sArray.Count() - 1];

                data.FullName = str.ToString() ;

                return new Tuple<short, string>(0, null);
            }
            else
            {
                return new Tuple<short, string>(ret, $"读取主程序名称错误,返回:{ret}");
            }
        }
    }
}
