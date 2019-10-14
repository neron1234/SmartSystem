using MMK.CNC.Application.LaserProgram.Dto;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MMK.SmartSystem.CNC.Core.DeviceHelpers
{
    public class ProgramBlock
    {
        public int G_Code_Count { get; set; }
        public int M_Code_Count { get; set; }

        public int BlockNum { get; set; }
        public double?[] G_Codes { get; set; }
        public double?[] M_Codes { get; set; }

        public double? X_Adr { get; set; }
        public double? Y_Adr { get; set; }
        public double? Z_Adr { get; set; }
        public double? I_Adr { get; set; }
        public double? J_Adr { get; set; }
        public double? K_Adr { get; set; }
        public double? R_Adr { get; set; }

        public ProgramBlock()
        {
            G_Codes = new double?[3];
            M_Codes = new double?[3];

            G_Code_Count = 0;
            M_Code_Count = 0;
        }

    }

    public class DrawBlock
    {
        public double G01Group { get; set; }

        public double StartX { get; set; }
        public double StartY { get; set; }
        public double StartZ { get; set; }
        public double EndX { get; set; }
        public double EndY { get; set; }
        public double EndZ { get; set; }

        public double? I_Adr { get; set; }
        public double? J_Adr { get; set; }
        public double? K_Adr { get; set; }
        public double? R_Adr { get; set; }

    }

    public class LaserProgramHelper : BaseHelper
    {
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

                    if (str.Count(x => x == '\n') > LaserCommentLineCount)
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
            FreeConnect(flib);

            if (ret != 0)
            {
                return "获得程序信息失败," + GetGeneralErrorMessage(ret);
            }
            
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

            Regex piercingRegex = new Regex(@"(?<=\(#PIERCING_COUNT=)\w*(?=\))");
            Match piercingMatch = piercingRegex.Match(str);
            if (piercingMatch.Success == true) info.PiercingCount = int.Parse(piercingMatch.Value);

            Regex cuttingTimeRegex = new Regex(@"(?<=\(#CUTTING_TIME=)\w*(?=\))");
            Match cuttingTimeMatch = cuttingTimeRegex.Match(str);
            if (cuttingTimeMatch.Success == true) info.CuttingTime = double.Parse(cuttingTimeMatch.Value);

            return null;
        }

        public string GetProgramString(string ncPath, string str)
        {
            ushort flib = 0;
            var ret_conn = BuildConnect(ref flib);
            if (ret_conn != 0)
            {
                FreeConnect(flib);
                return "获得程序信息失败，连接错误";
            }

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
                }
                if (buf[len - 1] == '%')
                {
                    break;
                }

            } while ((ret == 0) || (ret == 10));

            Focas1.cnc_upend4(flib);
            FreeConnect(flib);

            if (ret != 0)
            {
                return "获得程序信息失败," + GetGeneralErrorMessage(ret);
            }

            return null;
        }

        private void ProgramBlockDecompile(string str, ProgramCommentFromCncDto info, List<ProgramBlock> pBlocks)
        {
            //str.Replace("\r\n", "\n");
            var progBlocks = str.Split('\n');

            int blockIndex = 0;

            foreach (var block in progBlocks)
            {
                if(blockIndex< LaserCommentLineCount)
                {
                    GetInfo(info, block);
                }

                var tempStr = Regex.Replace(block, @"(?<=\()\w*(?=\))", "").Replace(" ", "");

                var str_num = Regex.Replace(tempStr, @"[A-Z]", "_");
                var nums = str_num.Split('_');
                var adrs = Regex.Replace(tempStr, @"[^A-Z]", "");


                var dblock = new ProgramBlock();
                for (int i = 0; i < adrs.Length; i++)
                {
                    switch (adrs[i])
                    {
                        case 'G':
                            dblock.G_Codes[dblock.G_Code_Count] = double.Parse(nums[i + 1]);
                            dblock.G_Code_Count++;
                            break;
                        case 'M':
                            dblock.M_Codes[dblock.M_Code_Count] = double.Parse(nums[i + 1]);
                            dblock.M_Code_Count++;
                            break;
                        case 'N':
                            dblock.BlockNum = int.Parse(nums[i + 1]);
                            break;
                        case 'X':
                            dblock.X_Adr = double.Parse(nums[i + 1]);
                            break;
                        case 'Y':
                            dblock.Y_Adr = double.Parse(nums[i + 1]);
                            break;
                        case 'Z':
                            dblock.Z_Adr = double.Parse(nums[i + 1]);
                            break;
                        case 'I':
                            dblock.I_Adr = double.Parse(nums[i + 1]);
                            break;
                        case 'J':
                            dblock.J_Adr = double.Parse(nums[i + 1]);
                            break;
                        case 'K':
                            dblock.K_Adr = double.Parse(nums[i + 1]);
                            break;
                        case 'R':
                            dblock.R_Adr = double.Parse(nums[i + 1]);
                            break;
                        default:
                            break;


                    }
                }

                pBlocks.Add(dblock);


                blockIndex++;
            }


        }

        private void ProgramBlockDraw(List<ProgramBlock> pBlocks, List<DrawBlock> dBlocks)
        {
            double lastG01Group = 0;// G01 G02 G03 G00
            double lastG03Group = 90;// G90 G91

            double lastX = 0;
            double lastY = 0;
            double lastZ = 0;

            foreach (var pitem in pBlocks)
            {
                var g03Group = pitem.G_Codes.Where(x => x.HasValue == true).Where(x => x.Value == 91 || x.Value == 90).FirstOrDefault();
                if (g03Group != null)
                {
                    lastG03Group = g03Group.Value;
                }

                var g01Group = pitem.G_Codes.Where(x => x.HasValue == true).Where(x => x.Value >= 0 && x.Value <= 3).FirstOrDefault();
                if (g01Group!=null)
                {
                    lastG01Group = g01Group.Value;

                    if (lastG03Group == 91)
                    {
                        if (pitem.X_Adr.HasValue) pitem.X_Adr += lastX;
                        if (pitem.Y_Adr.HasValue) pitem.Y_Adr += lastY;
                        if (pitem.Z_Adr.HasValue) pitem.Z_Adr += lastZ;
                    }

                    if (!pitem.X_Adr.HasValue) pitem.X_Adr = lastX;
                    if (!pitem.Y_Adr.HasValue) pitem.Y_Adr = lastY;
                    if (!pitem.Z_Adr.HasValue) pitem.Z_Adr = lastZ;

                    var ditem = new DrawBlock();
                    ditem.G01Group = lastG01Group;
                    ditem.StartX = lastX;
                    ditem.StartY = lastY;
                    ditem.StartZ = lastZ;
                    ditem.EndX = pitem.X_Adr.Value;
                    ditem.EndY = pitem.Y_Adr.Value;
                    ditem.EndZ = pitem.Z_Adr.Value;
                    ditem.I_Adr = pitem.I_Adr;
                    ditem.J_Adr = pitem.J_Adr;
                    ditem.K_Adr = pitem.K_Adr;
                    ditem.R_Adr = pitem.R_Adr;

                    dBlocks.Add(ditem);

                }
                else if (pitem.G_Code_Count==0 && pitem.M_Code_Count==0 && (pitem.X_Adr.HasValue || pitem.Y_Adr.HasValue || pitem.Z_Adr.HasValue))
                {
                    if (lastG03Group == 91)
                    {
                        if (pitem.X_Adr.HasValue) pitem.X_Adr += lastX;
                        if (pitem.Y_Adr.HasValue) pitem.Y_Adr += lastY;
                        if (pitem.Z_Adr.HasValue) pitem.Z_Adr += lastZ;
                    }

                    if (!pitem.X_Adr.HasValue) pitem.X_Adr = lastX;
                    if (!pitem.Y_Adr.HasValue) pitem.Y_Adr = lastY;
                    if (!pitem.Z_Adr.HasValue) pitem.Z_Adr = lastZ;

                    var ditem = new DrawBlock();
                    ditem.G01Group = lastG01Group;
                    ditem.StartX = lastX;
                    ditem.StartY = lastY;
                    ditem.StartZ = lastZ;
                    ditem.EndX = pitem.X_Adr.Value;
                    ditem.EndY = pitem.Y_Adr.Value;
                    ditem.EndZ = pitem.Z_Adr.Value;
                    ditem.I_Adr = pitem.I_Adr;
                    ditem.J_Adr = pitem.J_Adr;
                    ditem.K_Adr = pitem.K_Adr;
                    ditem.R_Adr = pitem.R_Adr;

                    dBlocks.Add(ditem);
                }
            }
        }

        private void DrawXYThumbnai(List<DrawBlock> dBlocks, int picWidth, int picHeight, string path)
        {
            //新建一个默认大小的图片
            Bitmap bmp = new Bitmap(picWidth, picHeight);
            //利用该图片对象生成画板
            Graphics graphic = Graphics.FromImage(bmp);
            //设置黑色背景
            graphic.Clear(Color.White);

            //画刷用来绘制表格线条，画笔用来绘制文字内容
            //新建一个画刷
            SolidBrush brush = new SolidBrush(Color.Red);
            //定义一个红色、线条宽度为1的画笔
            Pen pen = new Pen(Color.Red, (float)0.5);
            
            foreach(var item in dBlocks)
            {
                if(item.G01Group==1)
                {
                    graphic.DrawLine(pen, (float)item.StartX, (float)item.StartY, (float)item.EndX, (float)item.EndY);
                }
                else if(item.G01Group==2)
                {
                    List<Tuple<double, double>> tempPs = new List<Tuple<double, double>>();
                    DrawXYCircle(0.2, false, item, tempPs);

                    float? lastX=null;
                    float? lastY = null;
                    foreach(var p in tempPs)
                    {
                        if(lastX.HasValue && lastY.HasValue)
                        {
                            graphic.DrawLine(pen, lastX.Value, lastY.Value, (float)p.Item1, (float)p.Item2);
                        }

                        lastX = (float)p.Item1;
                        lastY = (float)p.Item2;
                    }
                }
                else if (item.G01Group == 3)
                {
                    List<Tuple<double, double>> tempPs = new List<Tuple<double, double>>();
                    DrawXYCircle(0.2, true, item, tempPs);

                    float? lastX = null;
                    float? lastY = null;
                    foreach (var p in tempPs)
                    {
                        if (lastX.HasValue && lastY.HasValue)
                        {
                            graphic.DrawLine(pen, lastX.Value, lastY.Value, (float)p.Item1, (float)p.Item2);
                        }

                        lastX = (float)p.Item1;
                        lastY = (float)p.Item2;
                    }
                }
            }

            //释放资源
            graphic.Dispose();
            //注意：程序要有该目录下该文件的访问权限
            bmp.Save(path, ImageFormat.Bmp);
        }

        private void DrawXYCircle(double inc, bool cw_ccw, DrawBlock block, List<Tuple<double,double>> points)
        {
            double st_x = block.StartX;
            double st_y = block.StartY;

            double center_x = st_x + block.I_Adr.Value;
            double center_y = st_y + block.J_Adr.Value;

            double r_pow = Math.Pow(block.I_Adr.Value, 2) + Math.Pow(block.J_Adr.Value, 2);
            double inc_pow = Math.Pow(inc, 2);
            double splitQ = Math.Sqrt(r_pow) / Math.Sqrt(2);

            double last_x_move2zero = st_x - center_x;
            double last_y_move2zero = st_y - center_y;

            double fin_x_move2zero = block.EndX - center_x;
            double fin_y_move2zero = block.EndY - center_y;


            if (cw_ccw == false)//G02 CW 顺时针
            {
                double? last_d11 = new double?();
                double? last_d12 = new double?();
                double? last_d21 = new double?();
                double? last_d22 = new double?();
                double? last_d31 = new double?();
                double? last_d32 = new double?();
                double? last_d41 = new double?();
                double? last_d42 = new double?();

                while (Math.Abs(fin_x_move2zero - last_x_move2zero) > inc * 5 || Math.Abs(fin_y_move2zero - last_y_move2zero) > inc * 5)
                {
                    last_x_move2zero = Math.Round(last_x_move2zero, 2);
                    last_y_move2zero = Math.Round(last_y_move2zero, 2);

                    if (last_x_move2zero > splitQ && last_y_move2zero > 0) //第一象限 1
                    {
                        var next_x_m = last_x_move2zero + inc / 2;
                        var next_y = last_y_move2zero - inc;

                        if (!last_d12.HasValue)
                        {
                            last_d12 = Math.Pow(next_x_m, 2) + Math.Pow(next_y, 2) - r_pow;
                        }

                        if (last_d12 > 0)
                        {
                            points.Add(new Tuple<double, double>(last_x_move2zero + center_x, last_y_move2zero + center_x - inc));
                            last_d12 += 2 * inc * -last_y_move2zero + 3 * inc_pow;


                            last_y_move2zero = last_y_move2zero - inc;
                        }
                        else
                        {
                            points.Add(new Tuple<double, double>(last_x_move2zero + center_x + inc, last_y_move2zero + center_x - inc));
                            last_d12 += 2 * inc * (last_x_move2zero - last_y_move2zero) + 5 * inc_pow;

                            last_x_move2zero = last_x_move2zero + inc;
                            last_y_move2zero = last_y_move2zero - inc;
                        }
                    }
                    else if (last_x_move2zero > 0 && last_x_move2zero <= splitQ && last_y_move2zero > 0) //第一象限 1
                    {
                        var next_x = last_x_move2zero + inc;
                        var next_y_m = last_y_move2zero - inc / 2;

                        if (!last_d11.HasValue)
                        {
                            last_d11 = Math.Pow(next_x, 2) + Math.Pow(next_y_m, 2) - r_pow;
                        }

                        if (last_d11 < 0)
                        {
                            points.Add(new Tuple<double, double>(last_x_move2zero + center_x + inc, last_y_move2zero + center_y));
                            last_d11 += 2 * inc * last_x_move2zero + 3 * inc_pow;

                            last_x_move2zero = last_x_move2zero + inc;
                        }
                        else
                        {
                            points.Add(new Tuple<double, double>(last_x_move2zero + center_x + inc, last_y_move2zero + center_y - inc));
                            last_d11 += 2 * inc * (last_x_move2zero - last_y_move2zero) + 5 * inc_pow;

                            last_x_move2zero = last_x_move2zero + inc;
                            last_y_move2zero = last_y_move2zero - inc;
                        }
                    }
                    else if (last_x_move2zero < 0 && last_x_move2zero >= -splitQ && last_y_move2zero >= 0)//第二象限 2
                    {
                        var next_x = last_x_move2zero + inc;
                        var next_y_m = last_y_move2zero + inc / 2;

                        if (!last_d22.HasValue)
                        {
                            last_d22 = Math.Pow(next_x, 2) + Math.Pow(next_y_m, 2) - r_pow;
                        }

                        if (last_d22 > 0)
                        {
                            points.Add(new Tuple<double, double>(last_x_move2zero + center_x + inc, last_y_move2zero + center_y));
                            last_d22 += 2 * inc * (last_x_move2zero) + 3 * inc_pow;

                            last_x_move2zero = last_x_move2zero + inc;
                        }
                        else
                        {
                            points.Add(new Tuple<double, double>(last_x_move2zero + center_x + inc, last_y_move2zero + center_y + inc));
                            last_d22 += 2 * inc * (last_x_move2zero + last_y_move2zero) + 5 * inc_pow;

                            last_x_move2zero = last_x_move2zero + inc;
                            last_y_move2zero = last_y_move2zero + inc;
                        }
                    }
                    else if (last_x_move2zero < -splitQ && last_y_move2zero >= 0)//第二象限 1
                    {
                        var next_x_m = last_x_move2zero + inc / 2;
                        var next_y = last_y_move2zero + inc;

                        if (!last_d21.HasValue)
                        {
                            last_d21 = Math.Pow(next_x_m, 2) + Math.Pow(next_y, 2) - r_pow;
                        }

                        if (last_d21 < 0)
                        {
                            points.Add(new Tuple<double, double>(last_x_move2zero + center_x, last_y_move2zero + center_y + inc));
                            last_y_move2zero = last_y_move2zero + inc;

                            last_d21 += 2 * inc * last_y_move2zero + 3 * inc_pow;
                        }
                        else
                        {
                            points.Add(new Tuple<double, double>(last_x_move2zero + center_x + inc, last_y_move2zero + center_y + inc));
                            last_x_move2zero = last_x_move2zero + inc;
                            last_y_move2zero = last_y_move2zero + inc;

                            last_d21 += 2 * inc * (last_x_move2zero + last_y_move2zero) + 5 * inc_pow;
                        }
                    }
                    else if (last_x_move2zero < -splitQ && last_y_move2zero < 0)//第三象限 1
                    {
                        var next_x_m = last_x_move2zero - inc / 2;
                        var next_y = last_y_move2zero + inc;

                        if (!last_d31.HasValue)
                        {
                            last_d31 = Math.Pow(next_x_m, 2) + Math.Pow(next_y, 2) - r_pow;
                        }

                        if (last_d31 > 0)
                        {
                            points.Add(new Tuple<double, double>(last_x_move2zero + center_x, last_y_move2zero + center_y + inc));
                            last_y_move2zero = last_y_move2zero + inc;

                            last_d31 += 2 * inc * last_y_move2zero + 3 * inc_pow;
                        }
                        else
                        {
                            points.Add(new Tuple<double, double>(last_x_move2zero + center_x - inc, last_y_move2zero + center_y + inc));
                            last_d31 += 2 * inc * (-last_x_move2zero + last_y_move2zero) + 5 * inc_pow;

                            last_x_move2zero = last_x_move2zero - inc;
                            last_y_move2zero = last_y_move2zero + inc;
                        }
                    }
                    else if (last_x_move2zero < 0 && last_x_move2zero >= -splitQ && last_y_move2zero < 0)//第三象限 2
                    {
                        var next_x = last_x_move2zero - inc;
                        var next_y_m = last_y_move2zero + inc / 2;

                        if (!last_d32.HasValue)
                        {
                            last_d32 = Math.Pow(next_x, 2) + Math.Pow(next_y_m, 2) - r_pow;
                        }

                        if (last_d32 < 0)
                        {
                            points.Add(new Tuple<double, double>(last_x_move2zero + center_x - inc, last_y_move2zero + center_y));
                            last_d32 += 2 * inc * (-last_x_move2zero) + 3 * inc_pow;

                            last_x_move2zero = last_x_move2zero - inc;
                        }
                        else
                        {
                            points.Add(new Tuple<double, double>(last_x_move2zero + center_x - inc, last_y_move2zero + center_y + inc));
                            last_d32 += 2 * inc * (-last_x_move2zero + last_y_move2zero) + 5 * inc_pow;

                            last_x_move2zero = last_x_move2zero - inc;
                            last_y_move2zero = last_y_move2zero + inc;
                        }
                    }
                    else if (last_x_move2zero > 0 && last_x_move2zero <= +splitQ && last_y_move2zero < 0)//第四象限 1
                    {
                        var next_x = last_x_move2zero - inc;
                        var next_y_m = last_y_move2zero - inc / 2;

                        if (!last_d41.HasValue)
                        {
                            last_d41 = Math.Pow(next_x, 2) + Math.Pow(next_y_m, 2) - r_pow;
                        }

                        if (last_d41 > 0)
                        {
                            points.Add(new Tuple<double, double>(last_x_move2zero + center_x - inc, last_y_move2zero + center_y));
                            last_d41 += 2 * inc * (-last_x_move2zero) + 3 * inc_pow;

                            last_x_move2zero = last_x_move2zero - inc;
                        }
                        else
                        {
                            points.Add(new Tuple<double, double>(last_x_move2zero + center_x - inc, last_y_move2zero + center_y - inc));
                            last_d41 += 2 * inc * (-last_x_move2zero - last_y_move2zero) + 5 * inc_pow;

                            last_x_move2zero = last_x_move2zero - inc;
                            last_y_move2zero = last_y_move2zero - inc;
                        }
                    }
                    else if (last_x_move2zero > +splitQ && last_y_move2zero < 0)//第四象限 2
                    {
                        var next_x_m = last_x_move2zero - inc / 2;
                        var next_y = last_y_move2zero - inc;

                        if (!last_d42.HasValue)
                        {
                            last_d42 = Math.Pow(next_x_m, 2) + Math.Pow(next_y, 2) - r_pow;
                        }

                        if (last_d42 < 0)
                        {
                            points.Add(new Tuple<double, double>(last_x_move2zero + center_x, last_y_move2zero + center_y - inc));
                            last_d42 += 2 * inc * (-last_y_move2zero) + 3 * inc_pow;

                            last_y_move2zero = last_y_move2zero - inc;
                        }
                        else
                        {
                            points.Add(new Tuple<double, double>(last_x_move2zero + center_x - inc, last_y_move2zero + center_y - inc));
                            last_d42 += 2 * inc * (-last_x_move2zero - last_y_move2zero) + 5 * inc_pow;

                            last_x_move2zero = last_x_move2zero - inc;
                            last_y_move2zero = last_y_move2zero - inc;
                        }
                    }

                }
            }
            else//G03 CW 逆时针
            {
                double? last_d11 = new double?();
                double? last_d12 = new double?();
                double? last_d21 = new double?();
                double? last_d22 = new double?();
                double? last_d31 = new double?();
                double? last_d32 = new double?();
                double? last_d41 = new double?();
                double? last_d42 = new double?();

                while (Math.Abs(fin_x_move2zero - last_x_move2zero) > inc * 2 || Math.Abs(fin_y_move2zero - last_y_move2zero) > inc * 2)
                {



                    //Console.WriteLine(Math.Abs(fin_x_move2zero - last_x_move2zero) + "     " + Math.Abs(fin_y_move2zero - last_y_move2zero));

                    last_x_move2zero = Math.Round(last_x_move2zero, 2);
                    last_y_move2zero = Math.Round(last_y_move2zero, 2);

                    if (last_x_move2zero > splitQ && last_y_move2zero >= 0) //第一象限 2
                    {
                        var next_x_m = last_x_move2zero - inc / 2;
                        var next_y = last_y_move2zero + inc;

                        if (!last_d12.HasValue)
                        {
                            last_d12 = Math.Pow(next_x_m, 2) + Math.Pow(next_y, 2) - r_pow;
                        }

                        if (last_d12 < 0)
                        {
                            points.Add(new Tuple<double, double>(last_x_move2zero + center_x, last_y_move2zero + center_y + inc));
                            last_y_move2zero = last_y_move2zero + inc;
                            last_d12 += 2 * inc * last_y_move2zero + 3 * inc_pow;

                        }
                        else
                        {
                            points.Add(new Tuple<double, double>(last_x_move2zero + center_x - inc, last_y_move2zero + center_y + inc));

                            last_x_move2zero = last_x_move2zero - inc;
                            last_y_move2zero = last_y_move2zero + inc;
                            last_d12 += 2 * inc * (-last_x_move2zero + last_y_move2zero) + 5 * inc_pow;

                        }
                    }
                    else if (last_x_move2zero > 0 && last_x_move2zero <= splitQ && last_y_move2zero > 0) //第一象限 1
                    {
                        var next_x = last_x_move2zero - inc;
                        var next_y_m = last_y_move2zero + inc / 2;

                        if (!last_d11.HasValue)
                        {
                            last_d11 = Math.Pow(next_x, 2) + Math.Pow(next_y_m, 2) - r_pow;
                        }

                        if (last_d11 > 0)
                        {
                            points.Add(new Tuple<double, double>(last_x_move2zero + center_x - inc, last_y_move2zero + center_y));

                            last_x_move2zero = last_x_move2zero - inc;
                            last_d11 += 2 * inc * (-last_x_move2zero) + 3 * inc_pow;
                        }
                        else
                        {
                            points.Add(new Tuple<double, double>(last_x_move2zero + center_x - inc, last_y_move2zero + center_y + inc));

                            last_x_move2zero = last_x_move2zero - inc;
                            last_y_move2zero = last_y_move2zero + inc;
                            last_d11 += 2 * inc * (-last_x_move2zero + last_y_move2zero) + 5 * inc_pow;

                        }
                    }
                    else if (last_x_move2zero <= 0 && last_x_move2zero > -splitQ && last_y_move2zero > 0)//第二象限 2
                    {
                        var next_x = last_x_move2zero - inc;
                        var next_y_m = last_y_move2zero - inc / 2;

                        if (!last_d22.HasValue)
                        {
                            last_d22 = Math.Pow(next_x, 2) + Math.Pow(next_y_m, 2) - r_pow;
                        }

                        if (last_d22 < 0)
                        {
                            points.Add(new Tuple<double, double>(last_x_move2zero + center_x - inc, last_y_move2zero + center_y));

                            last_x_move2zero = last_x_move2zero - inc;
                            last_d22 += 2 * inc * (-last_x_move2zero) + 3 * inc_pow;
                        }
                        else
                        {
                            points.Add(new Tuple<double, double>(last_x_move2zero + center_x - inc, last_y_move2zero + center_y - inc));


                            last_x_move2zero = last_x_move2zero - inc;
                            last_y_move2zero = last_y_move2zero - inc;
                            last_d22 += 2 * inc * (-last_x_move2zero - last_y_move2zero) + 5 * inc_pow;
                        }
                    }
                    else if (last_x_move2zero <= -splitQ && last_y_move2zero > 0)//第二象限 1
                    {
                        var next_x_m = last_x_move2zero - inc / 2;
                        var next_y = last_y_move2zero - inc;

                        if (!last_d21.HasValue)
                        {
                            last_d21 = Math.Pow(next_x_m, 2) + Math.Pow(next_y, 2) - r_pow;
                        }

                        if (last_d21 > 0)
                        {
                            points.Add(new Tuple<double, double>(last_x_move2zero + center_x, last_y_move2zero + center_y - inc));

                            last_y_move2zero = last_y_move2zero - inc;
                            last_d21 += 2 * inc * (-last_y_move2zero) + 3 * inc_pow;
                        }
                        else
                        {
                            points.Add(new Tuple<double, double>(last_x_move2zero + center_x - inc, last_y_move2zero + center_y - inc));

                            last_x_move2zero = last_x_move2zero - inc;
                            last_y_move2zero = last_y_move2zero - inc;
                            last_d21 += 2 * inc * (-last_x_move2zero - last_y_move2zero) + 5 * inc_pow;
                        }
                    }
                    else if (last_x_move2zero < -splitQ && last_y_move2zero <= 0)//第三象限 2
                    {
                        var next_x_m = last_x_move2zero + inc / 2;
                        var next_y = last_y_move2zero - inc;

                        if (!last_d32.HasValue)
                        {
                            last_d32 = Math.Pow(next_x_m, 2) + Math.Pow(next_y, 2) - r_pow;
                        }

                        if (last_d32 < 0)
                        {
                            points.Add(new Tuple<double, double>(last_x_move2zero + center_x, last_y_move2zero + center_y - inc));

                            last_y_move2zero = last_y_move2zero - inc;
                            last_d32 += 2 * inc * (-last_y_move2zero) + 3 * inc_pow;
                        }
                        else
                        {
                            points.Add(new Tuple<double, double>(last_x_move2zero + center_x + inc, last_y_move2zero + center_y - inc));

                            last_x_move2zero = last_x_move2zero + inc;
                            last_y_move2zero = last_y_move2zero - inc;
                            last_d32 += 2 * inc * (last_x_move2zero - last_y_move2zero) + 5 * inc_pow;
                        }
                    }
                    else if (last_x_move2zero < 0 && last_x_move2zero >= -splitQ && last_y_move2zero < 0)//第三象限 1
                    {
                        var next_x = last_x_move2zero + inc;
                        var next_y_m = last_y_move2zero - inc / 2;

                        if (!last_d31.HasValue)
                        {
                            last_d31 = Math.Pow(next_x, 2) + Math.Pow(next_y_m, 2) - r_pow;
                        }

                        if (last_d31 > 0)
                        {
                            points.Add(new Tuple<double, double>(last_x_move2zero + center_x + inc, last_y_move2zero + center_y));

                            last_x_move2zero = last_x_move2zero + inc;
                            last_d31 += 2 * inc * (last_x_move2zero) + 3 * inc_pow;
                        }
                        else
                        {
                            points.Add(new Tuple<double, double>(last_x_move2zero + center_x + inc, last_y_move2zero + center_y - inc));

                            last_x_move2zero = last_x_move2zero + inc;
                            last_y_move2zero = last_y_move2zero - inc;
                            last_d31 += 2 * inc * (last_x_move2zero - last_y_move2zero) + 5 * inc_pow;
                        }
                    }
                    else if (last_x_move2zero >= 0 && last_x_move2zero < splitQ && last_y_move2zero < 0)//第四象限 2
                    {
                        var next_x = last_x_move2zero + inc;
                        var next_y_m = last_y_move2zero + inc / 2;

                        if (!last_d42.HasValue)
                        {
                            last_d42 = Math.Pow(next_x, 2) + Math.Pow(next_y_m, 2) - r_pow;
                        }

                        if (last_d42 < 0)
                        {
                            points.Add(new Tuple<double, double>(last_x_move2zero + center_x + inc, last_y_move2zero + center_y));

                            last_x_move2zero = last_x_move2zero + inc;
                            last_d42 += 2 * inc * (last_x_move2zero) + 3 * inc_pow;
                        }
                        else
                        {
                            points.Add(new Tuple<double, double>(last_x_move2zero + center_x + inc, last_y_move2zero + center_y + inc));
                            last_x_move2zero = last_x_move2zero + inc;
                            last_y_move2zero = last_y_move2zero + inc;
                            last_d42 += 2 * inc * (last_x_move2zero + last_y_move2zero) + 5 * inc_pow;
                        }
                    }
                    else if (last_x_move2zero >= splitQ && last_y_move2zero <= 0)//第四象限 1
                    {
                        var next_x_m = last_x_move2zero + inc / 2;
                        var next_y = last_y_move2zero + inc;

                        if (!last_d41.HasValue)
                        {
                            last_d41 = Math.Pow(next_x_m, 2) + Math.Pow(next_y, 2) - r_pow;
                        }

                        if (last_d41 > 0)
                        {
                            points.Add(new Tuple<double, double>(last_x_move2zero + center_x, last_y_move2zero + center_y + inc));

                            last_y_move2zero = last_y_move2zero + inc;
                            last_d41 += 2 * inc * (last_y_move2zero) + 3 * inc_pow;
                        }
                        else
                        {
                            points.Add(new Tuple<double, double>(last_x_move2zero + center_x + inc, last_y_move2zero + center_y + inc));

                            last_x_move2zero = last_x_move2zero + inc;
                            last_y_move2zero = last_y_move2zero + inc;
                            last_d41 += 2 * inc * (last_x_move2zero + last_y_move2zero) + 5 * inc_pow;
                        }
                    }

                }
            }


        }

        private void GetInfo(ProgramCommentFromCncDto info, string blockStr)
        {
            Regex matRegex = new Regex(@"(?<=\(#MATERIAL=)\w*(?=\))");
            Match matMatch = matRegex.Match(blockStr);
            if (matMatch.Success == true) info.Material = matMatch.Value;

            Regex thickRegex = new Regex(@"(?<=\(#THICKNESS=)\w*(?=\))");
            Match thickMatch = thickRegex.Match(blockStr);
            if (thickMatch.Success == true) info.Thickness = double.Parse(thickMatch.Value);

            Regex gasRegex = new Regex(@"(?<=\(#CUTTING_GAS_KIND=)\w*(?=\))");
            Match gasMatch = gasRegex.Match(blockStr);
            if (gasMatch.Success == true) info.Gas = gasMatch.Value;

            Regex focalRegex = new Regex(@"(?<=\(#FOCAL_POSITION=)\w*(?=\))");
            Match focalMatch = focalRegex.Match(blockStr);
            if (focalMatch.Success == true) info.FocalPosition = double.Parse(focalMatch.Value);

            Regex nozzleDiaRegex = new Regex(@"(?<=\(#NOZZLE_DIA=)\w*(?=\))");
            Match nozzleDiaMatch = nozzleDiaRegex.Match(blockStr);
            if (nozzleDiaMatch.Success == true) info.NozzleDiameter = double.Parse(nozzleDiaMatch.Value);

            Regex nozzleTypeRegex = new Regex(@"(?<=\(#NOZZLE_TYPE=)\w*(?=\))");
            Match nozzleTypeMatch = nozzleTypeRegex.Match(blockStr);
            if (nozzleTypeMatch.Success == true) info.NozzleKind = nozzleTypeMatch.Value;

            Regex plateSizeRegex = new Regex(@"(?<=\(#PLATE_SIZE=)\w*(?=\))");
            Match plateSizeMatch = plateSizeRegex.Match(blockStr);
            if (plateSizeMatch.Success == true) info.PlateSize = plateSizeMatch.Value;

            Regex usedPlateSizeRegex = new Regex(@"(?<=\(#USED_SIZE=)\w*(?=\))");
            Match usedPlateSizeMatch = usedPlateSizeRegex.Match(blockStr);
            if (usedPlateSizeMatch.Success == true) info.UsedPlateSize = usedPlateSizeMatch.Value;

            Regex cuttingDistanceRegex = new Regex(@"(?<=\(#CUTTING_DISTANCE=)\w*(?=\))");
            Match cuttingDistanceMatch = cuttingDistanceRegex.Match(blockStr);
            if (cuttingDistanceMatch.Success == true) info.CuttingDistance = double.Parse(cuttingDistanceMatch.Value);

            Regex piercingRegex = new Regex(@"(?<=\(#PIERCING_COUNT=)\w*(?=\))");
            Match piercingMatch = piercingRegex.Match(blockStr);
            if (piercingMatch.Success == true) info.PiercingCount = int.Parse(piercingMatch.Value);

            Regex cuttingTimeRegex = new Regex(@"(?<=\(#CUTTING_TIME=)\w*(?=\))");
            Match cuttingTimeMatch = cuttingTimeRegex.Match(blockStr);
            if (cuttingTimeMatch.Success == true) info.CuttingTime = double.Parse(cuttingTimeMatch.Value);
        }

    }
}
