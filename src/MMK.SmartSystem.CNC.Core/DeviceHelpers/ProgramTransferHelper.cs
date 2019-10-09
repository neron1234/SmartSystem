using MMK.SmartSystem.WebCommon.DeviceModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MMK.SmartSystem.CNC.Core.DeviceHelpers
{
    public class ProgramTransferHelper : BaseHelper
    {
        public event Action<double> ReportProcessEvent;

        public string LocalDownloadProgramFromPcToCnc(string pcPath, string ncFolder)
        {
            ushort flib = 0;
            var ret_conn = BuildConnect(ref flib);
            if (ret_conn != 0)
            {
                FreeConnect(flib);
                return "传输程序至CNC失败，连接错误";
            }

            string progStr = "";
            using (System.IO.StreamReader sr = new System.IO.StreamReader(pcPath))
            {
                progStr = sr.ReadToEnd();
            }
            progStr = progStr.Replace("\r\n", "\n");

            var ret = Focas1.cnc_dwnstart4(flib, 0, ncFolder);//开始传输
            if (ret != 0) 
            {
                FreeConnect(flib);
                return "传输程序至CNC失败," + GetGeneralErrorMessage(ret);
            }

            var total_size = progStr.Length;
            var len = total_size;
            int n = 0;
            long cur_size = 0;

            while (len > 0)
            {
                n = len;
                ret = Focas1.cnc_download4(flib, ref n, progStr);
                if (ret == 10)
                {
                    continue;
                }
                if (ret == 0)
                {
                    cur_size += n;
                    progStr = progStr.Substring(n);
                    len -= n;
                }
                if (ret != 0)
                {
                    break;
                }

                if(total_size!=0)
                {
                    ReportProcessEvent?.Invoke((double)cur_size / (double)total_size);
                }
            }

            Focas1.cnc_dwnend4(flib);

            FreeConnect(flib);
            if (ret != 0)
            {
                return "传输程序至CNC失败," + GetGeneralErrorMessage(ret);
            }

            return null;
        }

        public string LocalUploadProgramFromCncToPc(string ncPath, string pcPath, double fileSize)
        {
            if (System.IO.File.Exists(pcPath))
            {
                return "传输程序至PC失败,PC文件已经存在";
            }

            ushort flib = 0;
            var ret_conn = BuildConnect(ref flib);
            if (ret_conn != 0)
            {
                FreeConnect(flib);
                return "传输程序至PC失败，连接错误";
            }

            string str = "";
            long cur_size = 0;

            Focas1.cnc_upend4(flib);
            var ret = Focas1.cnc_upstart4(flib, 0, ncPath);
            if (ret != 0)
            {
                FreeConnect(flib);
                return "传输程序至PC失败," + GetGeneralErrorMessage(ret);
            }

            int len;
            do
            {
                len = 1024;

                StringBuilder buf = new StringBuilder(1024);
                ret = Focas1.cnc_upload4(flib, ref len, buf);

                if (ret == 10)
                {
                    continue;
                }
                if (ret == 0)
                {
                    string temp = buf.ToString(0, len);
                    cur_size += len;
                    str += temp;

                }
                if (buf[len - 1] == '%')
                {
                    break;
                }

                if (fileSize != 0)
                {
                    ReportProcessEvent?.Invoke((double)cur_size / (double)fileSize);
                }

            } while ((ret == 0) || (ret == 10));

            Focas1.cnc_upend4(flib);

            if (ret != 0)
            {
                return "传输程序至PC失败," + GetGeneralErrorMessage(ret);
            }
            FreeConnect(flib);

            using (StreamWriter sw = new StreamWriter(pcPath))
            {
                sw.Write(str);
            }

            return null;
        }

        public string DeleteProgramInCnc(string ncPath)
        {
            ushort flib = 0;
            var ret_conn = BuildConnect(ref flib);
            if (ret_conn != 0)
            {
                FreeConnect(flib);
                return "删除PC程序失败，连接错误";
            }

            var ret = Focas1.cnc_pdf_del(flib, ncPath);

            FreeConnect(flib);

            if (ret == 5) return "文件路径不正确或者文件没找到";
            if (ret == 7) return "文件被保护";
            if (ret == 13) return "CNC正在运行或者处于急停状态";
            if (ret != 0) return GetGeneralErrorMessage(ret);
            return null;
        }

    }
}
