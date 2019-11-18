using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.CNC.Core.DeviceHelpers
{
    public class ProgramRestartHelper : BaseHelper
    {
        public Tuple<short, string> NcodeRestart(string filePath, int code)
        {
            ushort flib = 0;
            var ret_conn = BuildConnect(ref flib);
            if (ret_conn != 0)
            {
                FreeConnect(flib);
                return new Tuple<short, string>(-16, "N号续切设定错误，连接错误");
            }

            string buf = "";
            var ret_buf = LocalUploadProgramFromCncToBuf(flib, ref buf, filePath);
            if(ret_buf.Item1!=0)
            {
                FreeConnect(flib);
                return ret_buf;
            }

            int index = 0;
            try
            {
                index = buf.IndexOf('\n', 0);
                index = buf.IndexOf('\n', index + 1);
                if (index < 0)
                {
                    FreeConnect(flib);
                    return new Tuple<short, string>(-199, "N号续切设定错误,程序文本有误");
                }
            }
            catch
            {
                FreeConnect(flib);
                return new Tuple<short, string>(-199, "N号续切设定错误,程序文本有误");
            }


            buf = buf.Insert(index + 1, "GOTO" + code.ToString() + "\n");

            var ret_del = Focas1.cnc_pdf_del(flib, filePath);
            if(ret_del!=0)
            {
                FreeConnect(flib);
                return new Tuple<short, string>(ret_del, "N号续切设定错误,删除主程序失败"+ GetGeneralErrorMessage(ret_del));
            }

            var ret_cnc = LocalDownloadProgramFromBufToCnc(flib, buf, filePath);
            if (ret_cnc.Item1 != 0)
            {
                FreeConnect(flib);
                return ret_cnc;
            }

            var ret_main = Focas1.cnc_pdf_slctmain(flib, filePath);
            if(ret_main!=0)
            {
                FreeConnect(flib);
                return new Tuple<short, string>(ret_del, "N号续切设定错误,设定主程序错误," + GetGeneralErrorMessage(ret_del));
            }

            FreeConnect(flib);
            return new Tuple<short, string>(0, null);

        }

        private Tuple<short, string> LocalUploadProgramFromCncToBuf(ushort flib, ref string buf, string filePath)
        {
            long cur_size = 0;

            Focas1.cnc_upend4(flib);
            var ret = Focas1.cnc_upstart4(flib, 0, filePath);
            if (ret != 0)
            {
                return new Tuple<short, string>(-16, "N号续切设定错误,获得当前程序文本错误," + GetGeneralErrorMessage(ret));
            }



            int len;
            do
            {
                len = 1024;

                StringBuilder tempBuf = new StringBuilder(1024);
                ret = Focas1.cnc_upload4(flib, ref len, tempBuf);

                if (ret == 10)
                {
                    continue;
                }
                if (ret == 0)
                {
                    string temp = tempBuf.ToString(0, len);
                    cur_size += len;
                    buf += temp;

                }
                if (tempBuf[len - 1] == '%')
                {
                    break;
                }

            } while ((ret == 0) || (ret == 10));

            Focas1.cnc_upend4(flib);

            if (ret != 0)
            {
                return new Tuple<short, string>(ret, "N号续切设定错误,获得当前程序文本错误," + GetGeneralErrorMessage(ret));
            }
            else
            {
                return new Tuple<short, string>(0, null);
            }
        }

        private Tuple<short, string> LocalDownloadProgramFromBufToCnc(ushort flib, string buf, string filePath)
        {
            string[] sArray = filePath.Split('/');
            var index = filePath.LastIndexOf('/');
            var folder = filePath.Substring(0, index);
            

            var ret = Focas1.cnc_dwnstart4(flib, 0, folder);//开始传输
            if (ret != 0)
            {
                return new Tuple<short, string>(ret, "N号续切设定错误,写入新程序错误" + GetGeneralErrorMessage(ret));
            }

            var len = buf.Length;
            int n = 0;

            while (len > 0)
            {
                n = len;
                ret = Focas1.cnc_download4(flib, ref n, buf);
                if (ret == 10)
                {
                    continue;
                }
                if (ret == 0)
                {
                    buf = buf.Substring(n);
                    len -= n;
                }
                if (ret != 0)
                {
                    break;
                }

            }

            Focas1.cnc_dwnend4(flib);

            if (ret != 0)
            {
                return new Tuple<short, string>(ret, "N号续切设定错误,写入新程序错误" + GetGeneralErrorMessage(ret));
            }

            return new Tuple<short, string>(0,null);
        }

    }
}
