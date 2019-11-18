using MMK.SmartSystem.WebCommon.DeviceModel.LaserData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.CNC.Core.DeviceHelpers
{
    public class LaserDataHelper : BaseHelper
    {
        public Tuple<short, string> ReadLaserData(ushort flib, ref ReadLaserDataResultModel data)
        {
            Focas1.ODBLCMDT cmmddat = new Focas1.ODBLCMDT();
            var ret = Focas1.cnc_rdlcmddat(flib, cmmddat);

            if(ret!=0)
            {
                return new Tuple<short, string>(ret, $"读取激光数据错误,返回:{ret}");
            }

            data.ActualFeed = (double)cmmddat.feed * Math.Pow(-cmmddat.feed_dec, 10);
            data.ActualFreq = cmmddat.freq;
            data.ActualDuty = cmmddat.duty;
            data.ActualPower = cmmddat.power;
            data.CommandGasKind = cmmddat.g_kind;
            data.GasSettingTime = cmmddat.g_ready_t;
            data.ActualGasPress = cmmddat.g_press;

            return new Tuple<short, string>(0, null);
        }
    }
}
