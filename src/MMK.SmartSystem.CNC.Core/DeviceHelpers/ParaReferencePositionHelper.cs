using MMK.SmartSystem.WebCommon.DeviceModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMK.SmartSystem.CNC.Core.DeviceHelpers
{
    public class ParaReferencePositionHelper : BaseHelper
    {
        public Tuple<short, string> ReadParaReferencePositionRange(ushort flib, int start, int num, ref double[,] data)
        {

            if(start>4 || start<1) return new Tuple<short, string>(-100, "读取参考点数据错误,参考点参数数据设定错误");
            if ((start + num - 1) > 4 || (start + num - 1) < 1) return new Tuple<short, string>(-100, "读取参考点数据错误,参考点参数数据设定错误");
            if (data.Length < Focas1.MAX_AXIS * 4) return new Tuple<short, string>(-100, "读取参考点数据错误,数据存储区域过小");

            Focas1.ODBAXIS buf = new Focas1.ODBAXIS();
            buf.data = new int[Focas1.MAX_AXIS];
            short ret = -100;

            short s_number = (short)(start + 1239);
            short e_number = (short)(s_number + num - 1);
            short length = (short)((4 + 8 * Focas1.MAX_AXIS + 2) * num);
            Focas1.IODBPSD_0IF param = new Focas1.IODBPSD_0IF();
            ret = Focas1.cnc_rdparar(flib, ref s_number, -1, ref e_number, ref length, param);
            if(ret==0)
            {
                for (int i = 0; i < num; i++)
                {
                    string strdata = "data" + (i + 1).ToString();
                    object obj = param.GetType().GetField(strdata).GetValue(param);
                    var u = (Focas1.IODBPSD_U)obj.GetType().GetField("u").GetValue(obj);

                    for (int j = 0; j < Focas1.MAX_AXIS; j++)
                    {
                        data[start + i - 1,j] = u.rdatas[j].prm_val * Math.Pow(-10, u.rdatas[j].dec_val);
                    }
                }
                return new Tuple<short, string>(0, null);
            }

            return new Tuple<short, string>(ret, $"读取参考点数据错误,返回:{ret}");
        }

        public string DecompilerReadParaReferencePositionInfo(double[,] data, DecompReadParaReferencePositionItemModel itemModel, ref double val)
        {
            if (itemModel.AxisNum > Focas1.MAX_AXIS || itemModel.AxisNum < 1) return "无法获得参考点数据,轴号设定错误";
            if (data.Length < (itemModel.ReferencePositionType-1)*Focas1.MAX_AXIS + itemModel.AxisNum) return "无法获得参考点数据,位置信息地址超出读取范围";

            val = data[itemModel.ReferencePositionType - 1,itemModel.AxisNum - 1];

            return null;
        }

        public string WriteParaReferencePosition(ushort flib, int type, short axis, double data)
        {
            if (type < 1 || type > 4) return "写入参考点数据失败，参数设定错误";

            var temp_rdata = data.GetDecimals();

            Focas1.IODBPSD param = new Focas1.IODBPSD();
            param.datano = (short)(1239 + type);
            param.type = axis;
            param.u.rdatas[0].prm_val = temp_rdata.Item1;
            param.u.rdatas[0].dec_val = temp_rdata.Item2;

            var ret = Focas1.cnc_wrparam(flib, 12, param);

            if (ret != 0)
            {
                return $"写入参考点数据失败，返回:{ret}";
            }

            return null;
        }

        public string GetParaReferencePosition(ushort flib, int type, short axis,ref double data)
        {
            if (type < 1 || type > 4) return "获得参考点数据失败，参数设定错误";

            Focas1.IODBPSD param = new Focas1.IODBPSD();
            short number = (short)(1239 + type);
            var ret = Focas1.cnc_rdparam3(flib, number, axis,12,1, param);

            if (ret != 0)
            {
                return $"获得参考点数据失败，返回:{ret}";
            }

            data = param.u.rdatas[0].prm_val * Math.Pow(-10, param.u.rdatas[0].dec_val);
            return null;
        }
    }
}
