using MMK.SmartSystem.WebCommon.DeviceModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMK.SmartSystem.CNC.Core.DeviceHelpers
{
    public class PmcHelper : BaseHelper
    {
        //PMC;adr_type;adr;num
        public Tuple<short, string> ReadPmcRange(ushort flib, short adr_type, ushort adr, int num,ref int[] data)
        {
            if (num > 1000) return new Tuple<short, string>(-100, "读取PMC信号错误,读取数量超限");
            if (data.Length < num) return new Tuple<short, string>(-100, "读取PMC信号错误,数据存储区域过小");

            Focas1.IODBPMC2 buf = new Focas1.IODBPMC2();
            buf.ldata = new int[num];
            ushort adr_end = (ushort)(adr + num * 4 - 1);
            var ret = Focas1.pmc_rdpmcrng(flib, adr_type, 2, adr, adr_end, 48, buf);

            if (ret == 0)
            {
                buf.ldata.CopyTo(data, 0);
                return new Tuple<short, string>(0, null);
            }

            return new Tuple<short, string>(ret, $"读取PMC信号错误,返回:{ret}");
        }

        public string DecompilerReadPmcInfo(int[] data, DecompReadPmcItemModel itemModel,ref string val)
        {
            string message;
            if (data == null) return "没法获得信息,读取的信息种类不包含该信息";

            switch(itemModel.DataType)
            {
                case DataTypeEnum.Boolean:
                    {
                        if (itemModel.Bit.HasValue == false) message = "无法获得信息,Bool型地址没有配置BIT位";
                        else
                        {
                            var area = (int)Math.Ceiling((double)itemModel.RelStartAdr / 4.0);
                            if (area >= data.Length)
                            {
                                message = "无法获得信息,Bool型地址超出读取范围";
                            }
                            else
                            {
                                var area_rem = itemModel.RelStartAdr % 4;

                                int bd = (int)(0x01 << area_rem * 8 + itemModel.Bit.Value);
                                var res = (data[area] & bd) > 0;

                                val = res.ToString();

                                message = null;
                            }

                        }
                    }
                    break;
                case DataTypeEnum.Byte:
                    {
                        var area = (int)Math.Ceiling((double)itemModel.RelStartAdr / 4.0);
                        if (area >= data.Length)
                        {
                            message = "无法获得信息,Byte型地址超出读取范围";
                        }
                        else
                        {
                            var area_rem = itemModel.RelStartAdr % 4;
                            int bd = (int)(0x0F << area_rem * 8);

                            var res = (byte)((data[area] >> area_rem * 8) & 0x0F);

                            val = res.ToString();

                            message = null;
                        }
                    }
                    break;
                case DataTypeEnum.Int16:
                    {
                        var area = (int)Math.Ceiling((double)itemModel.RelStartAdr / 4.0);
                        if (area >= data.Length)
                        {
                            message = "无法获得信息,Word型地址超出读取范围";
                        }
                        else
                        {
                            var area_rem = itemModel.RelStartAdr % 4;
                            if(area_rem==0)
                            {
                                var res = (short)(data[area] & 0xFF);
                                val = res.ToString();

                                message = null;
                            }
                            else if(area_rem==2)
                            {
                                var res = (short)((data[area] >> 16) & 0xFF);
                                val = res.ToString();

                                message = null;
                            }
                            else
                            {
                                message = "无法获得信息,Word型地址没有对齐";
                            }
                        }
                    }
                    break;
                case DataTypeEnum.Int32:
                    {
                        var area = (int)Math.Ceiling((double)itemModel.RelStartAdr / 4.0);
                        if (area >= data.Length)
                        {
                            message = "无法获得信息,Word型地址超出读取范围";
                        }
                        else
                        {
                            var area_rem = itemModel.RelStartAdr % 4;
                            if (area_rem == 0)
                            {
                                var res = data[area];
                                val = res.ToString();

                                message = null;
                            }
                            else
                            {
                                message = "无法获得信息,Word型地址没有对齐";
                            }
                        }
                    }
                    break;
                default:
                    message = "没法获得信息,读取的信息种类错误";
                    break;
            }

            return message;
        }

        //PMC;adr_type;data_type;adr;bit;data
        //adr_type:0-G;1-F;2-Y;3-X;4-A;5-R;6-T;7-K;8-C;9-D;10-M;11-N;12-E;13-Z
        //data_type:0-BYTE;1-WORD;2-LONG;9-BIT;
        public string DecompileWritePmcPara(string para, ref short adr_type, ref short data_type, ref ushort adr, ref ushort bit, ref string data)
        {
            //PMC;adr_type;data_type;adr;bit;data
            var temps = para.Split(';');

            if (temps.Length < 6) return "PMC通讯参数错误";

            var ret_T = short.TryParse(temps[1], out adr_type);
            var ret_D = short.TryParse(temps[2], out data_type);
            var ret_A = ushort.TryParse(temps[3], out adr);
            var ret_B = ushort.TryParse(temps[4], out bit);
            data = temps[5];

            if (ret_T == false || ret_D == false || ret_A == false || ret_B == false)
            {
                return "PMC通讯参数错误";
            }

            return null;

        }

        //写操作
        public string WritePmc(short adr_type, short data_type, ushort adr, ushort bit, string data)
        {
            ushort flib = 0;

            var ret_conn = BuildConnect(ref flib);
            if(ret_conn !=0 )
            {
                FreeConnect(flib);
                return "写入PMC信号失败，连接错误";
            }

            string ret;
            switch (data_type)
            {
                case 1:
                    ret = WritePmcByte(flib, adr_type, adr, data);
                    break;
                case 2:
                    ret = WritePmcWord(flib, adr_type, adr, data);
                    break;
                case 3:
                    ret = WritePmcLong(flib, adr_type, adr, data);
                    break;
                case 9:
                    ret = WritePmcBit(flib, adr_type, adr, bit, data);
                    break;
                default:
                    ret = "写入PMC信号失败，数据类型错误";
                    break;
            }

            FreeConnect(flib);
            return ret;
        }

        public string SetPmcBit(short adr_type, ushort adr, ushort bit)
        {
            ushort flib = 0;

            var ret_conn = BuildConnect(ref flib);
            if (ret_conn != 0)
            {
                FreeConnect(flib);
                return "置位PMC信号失败，连接错误";
            }

            string ret = WritePmcBit(flib, adr_type, adr, bit, "true");

            FreeConnect(flib);
            return ret;
        }

        public string ResetPmcBit(short adr_type, ushort adr, ushort bit)
        {
            ushort flib = 0;

            var ret_conn = BuildConnect(ref flib);
            if (ret_conn != 0)
            {
                FreeConnect(flib);
                return "置位PMC信号失败，连接错误";
            }

            string ret = WritePmcBit(flib, adr_type, adr, bit, "false");

            FreeConnect(flib);
            return ret;
        }

        public string ReversalPmcBit(short adr_type, ushort adr, ushort bit)
        {
            ushort flib = 0;

            var ret_conn = BuildConnect(ref flib);
            if (ret_conn != 0)
            {
                FreeConnect(flib);
                return "翻转PMC信号失败，连接错误";
            }

            bool pmcBit = false;
            string ret = ReadPmcBit(flib, adr_type, adr, bit, ref pmcBit);
            if(ret!=null)
            {
                FreeConnect(flib);
                return $"翻转PMC信号失败(ret)";
            }
            
            if(pmcBit==true)
            {
                ret = WritePmcBit(flib, adr_type, adr, bit, "false");
            }
            else
            {
                ret = WritePmcBit(flib, adr_type, adr, bit, "true");
            }

            FreeConnect(flib);
            return ret;
        }

        #region 私有方法
        private string WritePmcByte(ushort flib, short adr_type, ushort adr, string data)
        {
            byte btemp;
            bool ret_b = byte.TryParse(data, out btemp);
            if (ret_b == false) return "写入PMC信号失败,数据格式错误";

            Focas1.IODBPMC0 buf = new Focas1.IODBPMC0();
            buf.cdata = new byte[12];
            buf.cdata[0] = btemp;
            buf.datano_s = (short)adr;
            buf.datano_e = (short)adr;
            buf.type_a = adr_type;
            buf.type_d = 0;
            var ret = Focas1.pmc_wrpmcrng(flib, 9, buf);

            ret = Focas1.pmc_wrpmcrng(flib, 9, buf);
            if (ret != 0) return $"写入PMC信号失败,返回:{ret}";

            return null;
        }

        private string WritePmcWord(ushort flib, short adr_type, ushort adr, string data)
        {
            short itemp;
            bool ret_b = short.TryParse(data, out itemp);
            if (ret_b == false) return "写入PMC信号失败,数据格式错误";

            Focas1.IODBPMC1 buf = new Focas1.IODBPMC1();
            buf.idata = new short[5];
            buf.idata[0] = itemp;
            buf.datano_s = (short)adr;
            buf.datano_e = (short)(adr + 1);
            buf.type_a = adr_type;
            buf.type_d = 1;
            var ret = Focas1.pmc_wrpmcrng(flib, 10, buf);
            if (ret != 0) return $"写入PMC信号失败,返回:{ret}";

            return null;
        }

        private string WritePmcLong(ushort flib, short adr_type, ushort adr, string data)
        {
            int ltemp;
            bool ret_b = int.TryParse(data, out ltemp);
            if (ret_b == false) return "写入PMC信号失败,数据格式错误";

            Focas1.IODBPMC2 buf = new Focas1.IODBPMC2();
            buf.ldata = new int[5];
            buf.ldata[0] = ltemp;
            buf.datano_s = (short)adr;
            buf.datano_e = (short)(adr + 3);
            buf.type_a = adr_type;
            buf.type_d = 2;
            var ret = Focas1.pmc_wrpmcrng(flib, 12, buf);
            if (ret != 0) return $"写入PMC信号失败,返回:{ret}";

            return null;
        }

        private string WritePmcBit(ushort flib, short adr_type, ushort adr, ushort bit, string data)
        {
            bool btemp;
            bool ret_b = bool.TryParse(data, out btemp);
            if (ret_b == false) return "写入PMC信号失败,数据格式错误";

            Focas1.IODBPMC0 buf = new Focas1.IODBPMC0();
            buf.cdata = new byte[1];
            var ret = Focas1.pmc_rdpmcrng(flib, adr_type, 0, adr, adr, 9, buf);
            if (ret != 0) return $"写入PMC信号失败,获得当期值失败,返回:{ret}";

            byte bd = (byte)(0x01 << bit);
            if (btemp == true)
            {
                buf.cdata[0] = (byte)(buf.cdata[0] | bd);
            }
            else
            {
                buf.cdata[0] = (byte)(buf.cdata[0] & (~bd));
            }

            ret = Focas1.pmc_wrpmcrng(flib, 9, buf);
            if (ret != 0) return $"写入PMC信号失败,返回:{ret}";

            return null;
        }

        private string ReadPmcBit(ushort flib, short adr_type, ushort adr, ushort bit, ref bool data)
        {

            Focas1.IODBPMC0 buf = new Focas1.IODBPMC0();
            buf.cdata = new byte[1];
            short ret = Focas1.pmc_rdpmcrng(flib, adr_type, 0, adr, adr, 9, buf);
            if (ret != 0) return $"读取PMC信号失败,返回:{ret}";

            byte bd = (byte)(0x01 << bit);
            data = (buf.cdata[0] & bd) > 0;

            return null;
        }

        #endregion
    }
}
