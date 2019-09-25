using MMK.SmartSystem.WebCommon.DeviceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.RealTime.DeviceHandlers
{
    public static class ModalInfoHelper
    {
        public static Tuple<short, string> ReadModalInfo(ushort flib, ref ReadModalResultModel data)
        {
            data.Modals.Clear();

            short num_gcd = 28;
            Focas1.ODBGCD g_code = new Focas1.ODBGCD();
            var ret = Focas1.cnc_rdgcode(flib, -1, 1, ref num_gcd, g_code);
            if(ret==0)
            {
                for(int i=0;i<21;i++)
                {
                    string strdata = "gcd" + (i + 1).ToString();
                    object obj = g_code.GetType().GetField(strdata).GetValue(g_code);
                    string code = obj.GetType().GetField("code").GetValue(obj).ToString();

                    data.Modals.Add(code);
                }
                return new Tuple<short, string>(0, null);
            }
            else
            {
                return new Tuple<short, string>(ret, $"读取模态信息错误,返回:{ret}");
            }
        }
    }
}
