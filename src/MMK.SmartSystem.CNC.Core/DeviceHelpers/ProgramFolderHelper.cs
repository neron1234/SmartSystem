using MMK.SmartSystem.WebCommon.DeviceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.CNC.Core.DeviceHelpers
{
    public static class ProgramFolderHelper
    {
        public static Tuple<short, string> ReadProgramFolder(ushort flib, ref ReadProgramFolderItemModel data)
        {
            var ret = ReadSubProgramFolder(flib, true, ref data);
            if (ret == 0)
            {
                return new Tuple<short, string>(0, null);
            }
            else
            {
                return new Tuple<short, string>(ret, $"获得程序目录列表错误,返回:{ret}");
            }
        }

        public static short ReadSubProgramFolder(ushort flib,  bool drill,  ref ReadProgramFolderItemModel data)
        {
            short num_prog = 50;
            var pdf_adir_out = new Focas1.PRGFOLDER();

            var pdf_adir_in = new Focas1.IDBPDFADIR();
            pdf_adir_in.req_num = 0;
            pdf_adir_in.size_kind = 2;
            pdf_adir_in.type = 0;
            pdf_adir_in.path = data.Folder;

            var ret = Focas1.cnc_rdpdf_alldir(flib, ref num_prog, pdf_adir_in, pdf_adir_out);
            if (ret == 0)
            {
                try
                {
                    for (int i = 0; i < num_prog; i++)
                    {
                        string strdata = "folder" + (i + 1).ToString();
                        object obj = pdf_adir_out.GetType().GetField(strdata).GetValue(pdf_adir_out);

                        string name = obj.GetType().GetField("d_f").GetValue(obj).ToString();
                        short data_kind = short.Parse(obj.GetType().GetField("data_kind").GetValue(obj).ToString());

                        if (data_kind == 0)
                        {
                            var item = new ReadProgramFolderItemModel()
                            {
                                Name = data.Folder + name + @"/",
                                Folder = data.Folder + name + @"/"
                            };

                            if(drill ==true)
                            {
                                ret = ReadSubProgramFolder(flib, true, ref item);
                                if (ret != 0) return ret;
                            }

                            data.Nodes.Add(item);
                        }

                    }
                }
                catch {
                    return -100;
                }
            }

            return ret;

        }
    }
}
