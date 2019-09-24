using MMK.SmartSystem.RealTime.DeviceModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMK.SmartSystem.RealTime.DeviceHandlers
{
    public static class NoticeHelper
    {
        public static Tuple<short, string> ReadNoticeRange(ushort flib, ref List<ReadNoticeResultItemModel> data)
        {
            if (data == null) return new Tuple<short, string>(-100, "读取操作信息信号错误,数据存储区域过小");

            Focas1.OPMSG3 opmsg = new Focas1.OPMSG3();
            short opmsg_num = 5;
            var ret = Focas1.cnc_rdopmsg3(flib, -1, ref opmsg_num, opmsg);
            if (ret == 0)
            {
                for (int i = 0; i < opmsg_num; i++)
                {
                    try
                    {
                        string strdata = "msg" + (i + 1).ToString();
                        object op = opmsg.GetType().GetField(strdata).GetValue(opmsg);

                        short op_no = short.Parse(op.GetType().GetField("datano").GetValue(op).ToString());
                        short op_type = short.Parse(op.GetType().GetField("type").GetValue(op).ToString());
                        string op_msg = op.GetType().GetField("data").GetValue(op).ToString();

                        if (op_no != -1)
                        {
                            data.Add(new ReadNoticeResultItemModel { Num = op_no, Ttype = op_type, Message = op_msg });
                        }
                    }
                    catch
                    {
                        new Tuple<short, string>(-100, "读取操作信息信号错误,数据处理出错");
                    }

                }

                return new Tuple<short, string>(0, null);
                
            }

            return new Tuple<short, string>(ret, $"读取操作信息信号错误,返回:{ret}");
        }
    }
}
