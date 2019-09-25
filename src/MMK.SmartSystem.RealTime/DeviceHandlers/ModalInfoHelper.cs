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
            //Focas1.ODBSEQ seq = new Focas1.ODBSEQ();
            //var ret = Focas1.cnc_rdseqnum(flib, seq);
            //if (ret == 0)
            //{
                //data.Value = seq.data;

                return new Tuple<short, string>(0, null);
            //}
            //else
            //{
            //    return new Tuple<short, string>(ret, $"读取程序段号错误,返回:{ret}");
            //}
        }
    }
}
