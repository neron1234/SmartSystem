using MMK.CNC.Application.LaserProgram.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MMK.SmartSystem.CNC.Core.DeviceHelpers
{
    public class LaserProgramHelper : BaseHelper
    {
        public ushort CommentLineCount { get; set; } = 20;

        public string GetProgramCommentInfo(string ncPath, ProgramCommentFromCncDto info)
        {
            ushort flib = 0;
            var ret_conn = BuildConnect(ref flib);
            if (ret_conn != 0)
            {
                FreeConnect(flib);
                return "获得程序信息失败，连接错误";
            }

            string str = "";
            Focas1.cnc_upend4(flib);
            var ret = Focas1.cnc_upstart4(flib, 0, ncPath);
            if (ret != 0)
            {
                FreeConnect(flib);
                return "获得程序信息失败," + GetGeneralErrorMessage(ret);
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
                    str += temp;

                    if (str.Count(x=>x=='\n') > CommentLineCount)
                    {
                        break;
                    }
                }
                if (buf[len - 1] == '%')
                {
                    break;
                }

            } while ((ret == 0) || (ret == 10));

            Focas1.cnc_upend4(flib);

            if (ret != 0)
            {
                return "获得程序信息失败," + GetGeneralErrorMessage(ret);
            }
            FreeConnect(flib);

            Regex matRegex = new Regex(@"(?<=\(#MATERIAL=)\w*(?=\))");
            Match matMatch = matRegex.Match(str);
            if (matMatch.Success == true) info.Material = matMatch.Value;

            Regex thickRegex = new Regex(@"(?<=\(#THICKNESS=)\w*(?=\))");
            Match thickMatch = thickRegex.Match(str);
            if (thickMatch.Success == true) info.Thickness = double.Parse(thickMatch.Value);

            Regex gasRegex = new Regex(@"(?<=\(#CUTTING_GAS_KIND=)\w*(?=\))");
            Match gasMatch = gasRegex.Match(str);
            if (gasMatch.Success == true) info.Gas = gasMatch.Value;

            Regex focalRegex = new Regex(@"(?<=\(#FOCAL_POSITION=)\w*(?=\))");
            Match focalMatch = focalRegex.Match(str);
            if (focalMatch.Success == true) info.FocalPosition =double.Parse(focalMatch.Value);

            Regex nozzleDiaRegex = new Regex(@"(?<=\(#NOZZLE_DIA=)\w*(?=\))");
            Match nozzleDiaMatch = nozzleDiaRegex.Match(str);
            if (nozzleDiaMatch.Success == true) info.NozzleDiameter = double.Parse(nozzleDiaMatch.Value);

            Regex nozzleTypeRegex = new Regex(@"(?<=\(#NOZZLE_TYPE=)\w*(?=\))");
            Match nozzleTypeMatch = nozzleTypeRegex.Match(str);
            if (nozzleTypeMatch.Success == true) info.NozzleKind = nozzleTypeMatch.Value;

            Regex plateSizeRegex = new Regex(@"(?<=\(#PLATE_SIZE=)\w*(?=\))");
            Match plateSizeMatch = plateSizeRegex.Match(str);
            if (plateSizeMatch.Success == true) info.PlateSize = plateSizeMatch.Value;

            Regex usedPlateSizeRegex = new Regex(@"(?<=\(#USED_SIZE=)\w*(?=\))");
            Match usedPlateSizeMatch = usedPlateSizeRegex.Match(str);
            if (usedPlateSizeMatch.Success == true) info.UsedPlateSize = usedPlateSizeMatch.Value;

            Regex cuttingDistanceRegex = new Regex(@"(?<=\(#CUTTING_DISTANCE=)\w*(?=\))");
            Match cuttingDistanceMatch = cuttingDistanceRegex.Match(str);
            if (cuttingDistanceMatch.Success == true) info.CuttingDistance = double.Parse(cuttingDistanceMatch.Value);

            Regex piercingRegex = new Regex(@"(?<=\(#CUTTING_DISTANCE=)\w*(?=\))");
            Match piercingMatch = piercingRegex.Match(str);
            if (piercingMatch.Success == true) info.PiercingCount = int.Parse(piercingMatch.Value);

            return null;
        }

    }
}
