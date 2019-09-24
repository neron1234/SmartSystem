using MMK.SmartSystem.CNC.Host.DeviceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.CNC.Host.DeviceHandlers
{
    public static class AlarmHelper
    {
        public static Tuple<short, string> ReadAlarmRange(ushort flib, ref List<ReadAlarmResultItemModel> data)
        {
            if (data ==null) return new Tuple<short, string>(-100, "读取报警信号错误,数据存储区域过小");

            Focas1.ODBALMMSG2 almmsg = new Focas1.ODBALMMSG2();
            short alarm_num = 10;
            var ret = Focas1.cnc_rdalmmsg2(flib, -1, ref alarm_num, almmsg);

            if (ret == 0)
            {
                for (int i = 0; i < alarm_num; i++)
                {
                    try
                    {
                        string strdata = "msg" + (i + 1).ToString();
                        object alm = almmsg.GetType().GetField(strdata).GetValue(almmsg);

                        int alm_no = int.Parse(alm.GetType().GetField("alm_no").GetValue(alm).ToString());
                        short type = short.Parse(alm.GetType().GetField("type").GetValue(alm).ToString());
                        short axis = short.Parse(alm.GetType().GetField("axis").GetValue(alm).ToString());
                        short len = short.Parse(alm.GetType().GetField("msg_len").GetValue(alm).ToString());

                        byte[] alm_msg_array = Encoding.GetEncoding("GBK").GetBytes(alm.GetType().GetField("alm_msg").GetValue(alm).ToString());
                        var alm_msg = System.Text.Encoding.Default.GetString(alm_msg_array.Take(len).ToArray()).Replace("\n", "");

                        data.Add(new ReadAlarmResultItemModel { Num = alm_no, Ttype = type, Axis = axis, Message = alm_msg });
                    }
                    catch {
                        new Tuple<short, string>(-100, "读取报警信号错误,数据处理出错");
                    }

                }

                return new Tuple<short, string>(0, null);
            }

            return new Tuple<short, string>(ret, $"读取报警信号错误,返回:{ret}");
        }
    }
}
