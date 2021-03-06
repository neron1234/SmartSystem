﻿using MMK.SmartSystem.WebCommon.DeviceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.CNC.Core.DeviceHelpers
{
    public class ProgramFolderHelper : BaseHelper
    {
        public Tuple<short, string> ReadProgramFolder(ushort flib, ref ReadProgramFolderItemModel data)
        {
            //data.RegNum = 0;
            var ret = ReadSubProgramFolder(flib, true, ref data);

            if (ret == -1) return new Tuple<short, string>(ret, $"获得程序目录列表错误,CNC忙碌");
            if (ret == 3) return new Tuple<short, string>(ret, $"获得程序目录列表错误,子目录错误");
            if (ret == 4) return new Tuple<short, string>(ret, $"获得程序目录列表错误,功能错误");
            if (ret == 5) return new Tuple<short, string>(ret, $"获得程序目录列表错误,文件夹未找到");
            if (ret == 13) return new Tuple<short, string>(ret, $"获得程序目录列表错误,CNC拒绝执行");
            if (ret != 0) return new Tuple<short, string>(ret, $"获得程序目录列表错误,{GetGeneralErrorMessage(ret)}");

            return new Tuple<short, string>(0, null);
        }

        public short ReadSubProgramFolder(ushort flib,  bool drill,  ref ReadProgramFolderItemModel data)
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
                                Folder = data.Folder + name + @"/",
                                RegNum= (short)(data.RegNum+1)

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
            else if (ret == 3) return 0;

            return ret;

        }
    }
}
