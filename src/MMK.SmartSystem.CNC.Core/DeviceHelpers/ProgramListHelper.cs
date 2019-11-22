using MMK.SmartSystem.WebCommon.DeviceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.CNC.Core.DeviceHelpers
{
    public class ProgramListHelper : BaseHelper
    {
        public Tuple<short, string> ReadProgramList(ushort flib, string folder, ref List<ReadProgramListItemResultModel> data)
        {
            data.Clear();

            short num_prog = 50;
            var pdf_adir_out = new Focas1.PRGFOLDER();

            var pdf_adir_in = new Focas1.IDBPDFADIR();
            pdf_adir_in.req_num = 0;
            pdf_adir_in.size_kind = 1;
            pdf_adir_in.type = 1;
            pdf_adir_in.path = folder;

            var ret = Focas1.cnc_rdpdf_alldir(flib, ref num_prog, pdf_adir_in, pdf_adir_out);
            if (ret == 0)
            {
                try
                {
                    for (int i = 0; i < num_prog; i++)
                    {
                        string strdata = "folder" + (i + 1).ToString();
                        object obj = pdf_adir_out.GetType().GetField(strdata).GetValue(pdf_adir_out);

                        short data_kind = short.Parse(obj.GetType().GetField("data_kind").GetValue(obj).ToString());
                        int size = int.Parse(obj.GetType().GetField("size").GetValue(obj).ToString());
                        string name = obj.GetType().GetField("d_f").GetValue(obj).ToString();
                        string comment = obj.GetType().GetField("comment").GetValue(obj).ToString();
                        short year = short.Parse(obj.GetType().GetField("year").GetValue(obj).ToString());
                        short mon = short.Parse(obj.GetType().GetField("mon").GetValue(obj).ToString());
                        short day = short.Parse(obj.GetType().GetField("day").GetValue(obj).ToString());
                        short hour = short.Parse(obj.GetType().GetField("hour").GetValue(obj).ToString());
                        short min = short.Parse(obj.GetType().GetField("min").GetValue(obj).ToString());
                        short sec = short.Parse(obj.GetType().GetField("sec").GetValue(obj).ToString());


                        if (data_kind == 1)
                        {
                            data.Add(new ReadProgramListItemResultModel
                            {
                                Name = name,
                                Description = comment,
                                Size = size,
                                CreateDateTime = new DateTime(year, mon, day, hour, min, sec)
                            }); ;
                        }

                    }
                }
                catch
                {
                    return new Tuple<short, string>(-100, $"读取程序列表错误,系统错误");
                }

                return new Tuple<short, string>(0, null);
            }
            else
            {
                return new Tuple<short, string>(ret, $"读取程序列表错误,返回:{ret}");
            }

        }

        public Tuple<short, string> SelectMainProgram(ushort flib, string file)
        {

            var ret = Focas1.cnc_pdf_slctmain(flib, file);
            
            if (ret == 5) return new Tuple<short, string>(ret, $"设定主程序错误,文件名称错误或者文件未找到");
            if (ret == 12) return new Tuple<short, string>(ret, $"设定主程序错误,操作方式错误");
            if (ret != 0) return new Tuple<short, string>(ret, $"设定主程序错误,{GetGeneralErrorMessage(ret)}");
            return new Tuple<short, string>(0, null);
        }

        public Tuple<short, string> DeleteProgram(ushort flib, string file)
        {

            var ret = Focas1.cnc_pdf_del(flib, file);
            if (ret == 5) return new Tuple<short, string>(ret, $"删除程序错误,文件名称错误或者文件未找到");
            if (ret == 7) return new Tuple<short, string>(ret, $"删除程序错误,写保护");
            if (ret == 13) return new Tuple<short, string>(ret, $"删除程序错误,CNC正在执行程序或者CNC处于急停状态");
            if (ret != 0) return new Tuple<short, string>(ret, $"删除程序错误,{GetGeneralErrorMessage(ret)}");
            return new Tuple<short, string>(0, null);

        }
    }
}
