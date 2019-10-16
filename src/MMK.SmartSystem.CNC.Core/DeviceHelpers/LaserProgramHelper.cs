using MMK.CNC.Application.LaserProgram.Dto;
using MMK.SmartSystem.WebCommon.EventModel;
using MMK.SmartSystem.WebCommon.HubModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MMK.SmartSystem.CNC.Core.DeviceHelpers
{
    public class LaserProgramDemo
    {

        public ProgramResolveResultDto ProgramResolve(ProgramResovleDto resovleDto)
        {
            LaserProgramHelper helper = new LaserProgramHelper();

            string progStr = "";
            helper.GetProgramString(resovleDto.FilePath, ref progStr);

            ProgramDetailDto info = new ProgramDetailDto();
            List<ProgramBlock> pBlocks = new List<ProgramBlock>();
            helper.ProgramBlockDecompile(progStr, info, pBlocks);

            List<DrawBlock> dBlocks = new List<DrawBlock>();
            helper.ProgramBlockDraw(pBlocks, dBlocks);

            double rWidth = 1000;
            double rHeight = 1000;
            if (info.PlateSize != null)
            {
                var rSize = info.PlateSize.Split('x');
                if (rSize.Length == 2)
                {
                    rWidth = double.Parse(rSize[0]);
                    rHeight = double.Parse(rSize[1]);
                }
            }
            string saveFullName = $"{resovleDto.BmpPath}\\{resovleDto.FileName}.bmp";
            helper.DrawXYThumbnai(dBlocks, 0.2, 680, 460, rWidth, rHeight, saveFullName);

            return new ProgramResolveResultDto()
            {
                Data = info,
                ImagePath = saveFullName,
                BmpName = resovleDto.FileName + ".bmp"
            };

        }
        public void Dowork()
        {
            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff"));

            LaserProgramHelper helper = new LaserProgramHelper();

            string progStr = "";
            helper.GetProgramString(@"F:\Machine_Software\LE1.1\激光程序\0005", ref progStr);

            ProgramDetailDto info = new ProgramDetailDto();
            List<ProgramBlock> pBlocks = new List<ProgramBlock>();
            helper.ProgramBlockDecompile(progStr, info, pBlocks);

            List<DrawBlock> dBlocks = new List<DrawBlock>();
            helper.ProgramBlockDraw(pBlocks, dBlocks);

            double rWidth = 1000;
            double rHeight = 1000;
            if (info.PlateSize != null)
            {
                var rSize = info.PlateSize.Split('x');
                if (rSize.Length == 2)
                {
                    rWidth = double.Parse(rSize[0]);
                    rHeight = double.Parse(rSize[1]);
                }
            }

            helper.DrawXYThumbnai(dBlocks, 0.2, 3000, 2000, rWidth, rHeight, @"F:\LaserPic.bmp");

            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff"));

            Console.ReadKey();
        }

        public void DoWork2()
        {
            //LaserProgramHelper helper = new LaserProgramHelper();

            //string progStr = "";
            //helper.GetProgramString(@"F:\Machine_Software\LE1.1\激光程序\0005", ref progStr);

            //ProgramCommentFromCncDto info = new ProgramCommentFromCncDto();
            //List<ProgramBlock> pBlocks = new List<ProgramBlock>();
            //helper.ProgramBlockDecompile(progStr, info, pBlocks);

            //List<DrawBlock> dBlocks = new List<DrawBlock>();
            //helper.ProgramBlockDraw(pBlocks, dBlocks);

            //double rWidth = 1000;
            //double rHeight = 1000;
            //if (info.PlateSize != null)
            //{
            //    var rSize = info.PlateSize.Split('x');
            //    if (rSize.Length == 2)
            //    {
            //        rWidth = double.Parse(rSize[0]);
            //        rHeight = double.Parse(rSize[1]);
            //    }
            //}

            //helper.DrawXYCanvasOneShot(dBlocks, 0.2, 3000, 2000, rWidth, rHeight, drawPanel);
        }

    }


    public static class GraphicsAnglExtension
    {
        public static float ToGraphicsAngle(this float data)
        {
            if (data < 0) data = 360 + data;
            //data = -1 * data;

            //return data < 0 ? 360 + data : data;
            return data;
        }

        public static float ToGraphicsSweep(this float start, float end, bool cw)
        {
            var temp_start = start.ToGraphicsAngle();
            var temp_end = end.ToGraphicsAngle();

            if (cw == true)
            {
                if (temp_start < temp_end)
                {
                    return 360 - (temp_end - temp_start);
                }
                else
                {
                    return temp_start - temp_end;
                }
            }
            else
            {
                if (temp_start < temp_end)
                {
                    return -temp_end + temp_start;
                }
                else
                {
                    return (temp_start - temp_end) - 360;
                }
            }

        }


    }


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

    public class LaserProgramHelper
    {
        private int LaserCommentLineCount = 10;

        public string GetProgramString(string ncPath, ref string str)
        {
            using (StreamReader sr = new StreamReader(ncPath))
            {
                str = sr.ReadToEnd();
            }

            return null;
        }

        public void ProgramBlockDecompile(string str, ProgramDetailDto info, List<ProgramBlock> pBlocks)
        {
            //str.Replace("\r\n", "\n");
            var progBlocks = str.Split('\n');

            int blockIndex = 0;

            foreach (var block in progBlocks)
            {
                if (blockIndex < LaserCommentLineCount)
                {
                    GetInfo(info, block);
                }

                var tempStr = Regex.Replace(block, @"\(.*\)", "");
                tempStr = Regex.Replace(tempStr, @"\<.*\>", "");
                tempStr = tempStr.Replace(" ", "").Replace("\r", "");

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

        public void ProgramBlockDraw(List<ProgramBlock> pBlocks, List<DrawBlock> dBlocks)
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
                if (g01Group != null)
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

                    lastX = ditem.EndX;
                    lastY = ditem.EndY;
                    lastZ = ditem.EndZ;

                }
                else if (pitem.G_Code_Count == 0 && pitem.M_Code_Count == 0 && (pitem.X_Adr.HasValue || pitem.Y_Adr.HasValue || pitem.Z_Adr.HasValue))
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

                    lastX = ditem.EndX;
                    lastY = ditem.EndY;
                    lastZ = ditem.EndZ;
                }
            }
        }

        public void DrawXYThumbnai(List<DrawBlock> dBlocks, double inc, int picWidth, int picHeight, double rWidth, double rHeight, string path)
        {
            float radioWidth = (float)(picWidth / rWidth);
            float radioHeight = (float)(picHeight / rHeight);

            float picradio = radioWidth > radioHeight ? radioHeight : radioWidth;

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
            Pen pen = new Pen(Color.Red, (float)0.1);


            foreach (var item in dBlocks)
            {
                //if (item.G01Group == 0)
                //{
                //    DrawXYLine(item, graphic, pen, picradio);
                //}
                if (item.G01Group == 1)
                {
                    DrawXYLine(item, graphic, pen, picradio);
                }
                else if (item.G01Group == 2)
                {
                    if (item.I_Adr.HasValue && item.J_Adr.HasValue)
                    {
                        DrawXYCircleWithIJ(item, graphic, pen, picradio);
                    }
                    else if (item.R_Adr.HasValue)
                    {
                        DrawXYCircleWithR(item, graphic, pen, picradio);
                    }
                }
                else if (item.G01Group == 3)
                {
                    if (item.I_Adr.HasValue && item.J_Adr.HasValue)
                    {
                        DrawXYCircleWithIJ(item, graphic, pen, picradio);
                    }
                    else if (item.R_Adr.HasValue)
                    {
                        DrawXYCircleWithR(item, graphic, pen, picradio);
                    }
                }

                #region old
                //else if (item.G01Group == 2)
                //{
                //    List<Tuple<double, double>> tempPs = new List<Tuple<double, double>>();

                //    DrawXYCircle(inc, false, item, tempPs);

                //    float? lastX = null;
                //    float? lastY = null;
                //    foreach (var p in tempPs)
                //    {
                //        if (lastX.HasValue && lastY.HasValue)
                //        {
                //            graphic.DrawLine(pen, (float)(lastX.Value * picradio), (float)(lastY.Value * picradio), (float)(p.Item1 * picradio), (float)(p.Item2 * picradio));
                //        }

                //        lastX = (float)p.Item1;
                //        lastY = (float)p.Item2;
                //    }
                //}
                //else if (item.G01Group == 3)
                //{
                //    //DrawXYCircleWithIJ(item, graphic, pen, picradio);

                //    List<Tuple<double, double>> tempPs = new List<Tuple<double, double>>();

                //    DrawXYCircle(inc, true, item, tempPs);

                //    float? lastX = null;
                //    float? lastY = null;
                //    foreach (var p in tempPs)
                //    {
                //        if (lastX.HasValue && lastY.HasValue)
                //        {
                //            graphic.DrawLine(pen, (float)(lastX.Value * picradio), (float)(lastY.Value * picradio), (float)(p.Item1 * picradio), (float)(p.Item2 * picradio));
                //        }

                //        lastX = (float)p.Item1;
                //        lastY = (float)p.Item2;
                //    }
                //}
                #endregion
            }

            //释放资源
            graphic.Dispose();
            //注意：程序要有该目录下该文件的访问权限
            bmp.Save(path, ImageFormat.Bmp);
        }

        private void DrawXYLine(DrawBlock block, Graphics graphic, Pen pen, double picRadio)
        {
            graphic.DrawLine(pen,
                (float)(block.StartX * picRadio),
                (float)(block.StartY * picRadio),
                (float)(block.EndX * picRadio),
                (float)(block.EndY * picRadio));
        }

        private void DrawXYCircleWithR(DrawBlock block, Graphics graphic, Pen pen, float picRadio)
        {
            double r_pow = block.R_Adr.Value * block.R_Adr.Value;

            var e = (-2 * block.StartX + 2 * block.EndX) / (2 * block.StartY - 2 * block.EndY);
            var f = (block.StartX * block.StartX - block.EndX * block.EndX + block.StartY * block.StartY - block.EndY * block.EndY) /
                (2 * block.StartY - 2 * block.EndY);

            var r = 1 + e * e;
            var s = -2 * block.StartX + 2 * e * f - 2 * block.StartY * e;
            var t = -r_pow + block.StartX * block.StartX + block.StartY * block.StartY + 2 * block.StartY * f;

            var judge = s * s - 4 * r * t;
            if (judge >= 0)
            {
                var center_x_1 = (-s + Math.Sqrt(s * s - 4 * r * t)) / 2 * s;
                var center_y_1 = e * center_x_1 + f;

                var center_x_2 = (-s - Math.Sqrt(s * s - 4 * r * t)) / 2 * s;
                var center_y_2 = e * center_x_2 + f;

                var arr_chord_x = block.EndX - block.StartX;
                var arr_chord_y = block.EndY - block.StartY;

                var arr_r_x_1 = center_x_1 - block.StartX;
                var arr_r_y_1 = center_y_1 - block.StartY;

                if (block.G01Group == 2)
                {
                    var temp = arr_chord_x * arr_r_y_1 - arr_chord_y * arr_r_x_1;
                    if (temp <= 0)
                    {
                        block.I_Adr = center_x_1 - block.StartX;
                        block.J_Adr = center_y_1 - block.StartX;

                        double center_x = (block.StartX + block.I_Adr.Value) * picRadio;
                        double center_y = (block.StartY + block.J_Adr.Value) * picRadio;
                        double radio = Math.Sqrt(r_pow) * picRadio;

                        float startArc = (float)(Math.Atan2(block.EndY * picRadio - center_y, block.EndX * picRadio - center_x) * 180 / Math.PI);
                        startArc = startArc > 0 ? 360 - startArc : -startArc;
                        float endArc = (float)(Math.Atan2(-block.J_Adr.Value * picRadio, -block.I_Adr.Value * picRadio) * 180 / Math.PI);
                        endArc = endArc > 0 ? 360 - endArc : -endArc;

                        graphic.DrawArc(
                            pen,
                            (float)(center_x - radio),
                            (float)(center_y - radio),
                            (float)radio * 2,
                            (float)radio * 2,
                            endArc,
                            Math.Abs(startArc - endArc)
                            );
                    }
                    else
                    {
                        block.I_Adr = center_x_2 - block.StartX;
                        block.J_Adr = center_y_2 - block.StartX;

                        double center_x = (block.StartX + block.I_Adr.Value) * picRadio;
                        double center_y = (block.StartY + block.J_Adr.Value) * picRadio;
                        double radio = Math.Sqrt(r_pow) * picRadio;

                        float startArc = (float)(Math.Atan2(block.EndY * picRadio - center_y, block.EndX * picRadio - center_x) * 180 / Math.PI);
                        startArc = startArc > 0 ? 360 - startArc : -startArc;
                        float endArc = (float)(Math.Atan2(-block.J_Adr.Value * picRadio, -block.I_Adr.Value * picRadio) * 180 / Math.PI);
                        endArc = endArc > 0 ? 360 - endArc : -endArc;

                        graphic.DrawArc(
                            pen,
                            (float)(center_x - radio),
                            (float)(center_y - radio),
                            (float)radio * 2,
                            (float)radio * 2,
                            endArc,
                            Math.Abs(startArc - endArc)
                            );
                    }
                }
                else
                {
                    var temp = arr_chord_x * arr_r_y_1 - arr_chord_y * arr_r_x_1;
                    if (temp >= 0)
                    {
                        block.I_Adr = center_x_1 - block.StartX;
                        block.J_Adr = center_y_1 - block.StartX;

                        double center_x = (block.StartX + block.I_Adr.Value) * picRadio;
                        double center_y = (block.StartY + block.J_Adr.Value) * picRadio;
                        double radio = Math.Sqrt(Math.Pow(block.I_Adr.Value, 2) + Math.Pow(block.J_Adr.Value, 2)) * picRadio;

                        float startArc = (float)(Math.Atan2(block.EndY * picRadio - center_y, block.EndX * picRadio - center_x) * 180 / Math.PI);
                        startArc = startArc > 0 ? 360 - startArc : -startArc;
                        float endArc = (float)(Math.Atan2(-block.J_Adr.Value * picRadio, -block.I_Adr.Value * picRadio) * 180 / Math.PI);
                        endArc = endArc > 0 ? 360 - endArc : -endArc;

                        graphic.DrawArc(
                            pen,
                            (float)(center_x - radio),
                            (float)(center_y - radio),
                            (float)radio * 2,
                            (float)radio * 2,
                            startArc,
                            Math.Abs(endArc - startArc)
                            );
                    }
                    else
                    {
                        block.I_Adr = center_x_2 - block.StartX;
                        block.J_Adr = center_y_2 - block.StartX;

                        double center_x = (block.StartX + block.I_Adr.Value) * picRadio;
                        double center_y = (block.StartY + block.J_Adr.Value) * picRadio;
                        double radio = Math.Sqrt(Math.Pow(block.I_Adr.Value, 2) + Math.Pow(block.J_Adr.Value, 2)) * picRadio;

                        float startArc = (float)(Math.Atan2(block.EndY * picRadio - center_y, block.EndX * picRadio - center_x) * 180 / Math.PI);
                        startArc = startArc > 0 ? 360 - startArc : -startArc;
                        float endArc = (float)(Math.Atan2(-block.J_Adr.Value * picRadio, -block.I_Adr.Value * picRadio) * 180 / Math.PI);
                        endArc = endArc > 0 ? 360 - endArc : -endArc;

                        graphic.DrawArc(
                            pen,
                            (float)(center_x - radio),
                            (float)(center_y - radio),
                            (float)radio * 2,
                            (float)radio * 2,
                            startArc,
                            Math.Abs(endArc - startArc)
                            );
                    }
                }
            }
        }

        private void DrawXYCircleWithIJ(DrawBlock block, Graphics graphic, Pen pen, float picRadio)
        {

            if (block.G01Group == 2)
            {
                double center_x = block.StartX + block.I_Adr.Value;
                double center_y = block.StartY + block.J_Adr.Value;
                double radius = Math.Round(Math.Sqrt(Math.Pow(block.I_Adr.Value, 2) + Math.Pow(block.J_Adr.Value, 2)), 2);

                float startArc = (float)(Math.Atan2(-block.J_Adr.Value, -block.I_Adr.Value) * 180 / Math.PI);
                float endArc = (float)(Math.Atan2(block.EndY - center_y, block.EndX - center_x) * 180 / Math.PI);

                graphic.DrawArc(
                    pen,
                    (float)(center_x - radius) * picRadio,
                    (float)(center_y - radius) * picRadio,
                    (float)radius * 2 * picRadio,
                    (float)radius * 2 * picRadio,
                    endArc,
                    startArc.ToGraphicsSweep(endArc, true)
                    );

            }
            else if (block.G01Group == 3)
            {
                double center_x = block.StartX + block.I_Adr.Value;
                double center_y = block.StartY + block.J_Adr.Value;
                double radius = Math.Round(Math.Sqrt(Math.Pow(block.I_Adr.Value, 2) + Math.Pow(block.J_Adr.Value, 2)), 2);

                float startArc = (float)(Math.Atan2(-block.J_Adr.Value, -block.I_Adr.Value) * 180 / Math.PI);
                float endArc = (float)(Math.Atan2(block.EndY - center_y, block.EndX - center_x) * 180 / Math.PI);


                graphic.DrawArc(
                    pen,
                    (float)(center_x - radius) * picRadio,
                    (float)(center_y - radius) * picRadio,
                    (float)radius * 2 * picRadio,
                    (float)radius * 2 * picRadio,
                    endArc,
                    startArc.ToGraphicsSweep(endArc, false)
                );


            }
        }

        private void DrawXYCircle(double inc, bool cw_ccw, DrawBlock block, List<Tuple<double, double>> points)
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

        private void GetInfo(ProgramDetailDto info, string blockStr)
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

    //public void DrawXYCanvasOneShot(List<DrawBlock> dBlocks, double inc, int picWidth, int picHeight, double rWidth, double rHeight, Canvas canvas)
    //{
    //    float radioWidth = (float)(picWidth / rWidth);
    //    float radioHeight = (float)(picHeight / rHeight);

    //    float picradio = radioWidth > radioHeight ? radioHeight : radioWidth;

    //    foreach (var item in dBlocks)
    //    {
    //        //if (item.G01Group == 0)
    //        //{
    //        //    DrawXYLine(item, graphic, pen, picradio);
    //        //}
    //        if (item.G01Group == 1)
    //        {
    //            var line = new System.Windows.Shapes.Line();
    //            line.X1 = (int)(item.StartX * picradio);
    //            line.Y1 = (int)(item.StartY * picradio);
    //            line.X2 = (int)(item.EndX * picradio);
    //            line.Y2 = (int)(item.EndY * picradio);
    //            line.Stroke = System.Windows.Media.Brushes.Black;
    //            line.StrokeThickness = 1;

    //            canvas.Children.Add(line);
    //        }
    //        else if (item.G01Group == 2)
    //        {
    //            //double center_x = item.StartX + item.I_Adr.Value;
    //            //double center_y = item.StartY + item.J_Adr.Value;
    //            double radius = Math.Round(Math.Sqrt(Math.Pow(item.I_Adr.Value, 2) + Math.Pow(item.J_Adr.Value, 2)), 2);

    //            //float startArc = (float)(Math.Atan2(-item.J_Adr.Value, -item.I_Adr.Value) * 180 / Math.PI);
    //            //float endArc = (float)(Math.Atan2(item.EndY - center_y, item.EndX - center_x) * 180 / Math.PI);

    //            Path path = new Path();
    //            PathGeometry pathGeometry = new PathGeometry();
    //            ArcSegment arc = new ArcSegment(
    //                new System.Windows.Point((int)(item.EndX * picradio), (int)(item.EndY * picradio)),
    //                new System.Windows.Size((int)Math.Abs(radius * picradio), (int)Math.Abs(radius * picradio)),
    //                0, false, SweepDirection.Counterclockwise, true);
    //            PathFigure figure = new PathFigure();
    //            figure.StartPoint = new System.Windows.Point((int)(item.StartX * picradio), (int)(item.StartY * picradio));
    //            figure.Segments.Add(arc);
    //            pathGeometry.Figures.Add(figure);
    //            path.Data = pathGeometry;
    //            path.Stroke = System.Windows.Media.Brushes.Black;
    //            path.StrokeThickness = 1;
    //            canvas.Children.Add(path);
    //        }
    //        else if (item.G01Group == 3)
    //        {
    //            //    double center_x = item.StartX + item.I_Adr.Value;
    //            //    double center_y = item.StartY + item.J_Adr.Value;
    //            double radius = Math.Round(Math.Sqrt(Math.Pow(item.I_Adr.Value, 2) + Math.Pow(item.J_Adr.Value, 2)), 2);

    //            //    float startArc = (float)(Math.Atan2(-item.J_Adr.Value, -item.I_Adr.Value) * 180 / Math.PI);
    //            //    float endArc = (float)(Math.Atan2(item.EndY - center_y, item.EndX - center_x) * 180 / Math.PI);

    //            Path path = new Path();
    //            PathGeometry pathGeometry = new PathGeometry();
    //            ArcSegment arc = new ArcSegment(new System.Windows.Point((int)(item.EndX * picradio), (int)(item.EndY * picradio)),
    //                new System.Windows.Size((int)(radius * picradio), (int)(radius * picradio)),
    //                0, false, SweepDirection.Clockwise, true);
    //            PathFigure figure = new PathFigure();
    //            figure.StartPoint = new System.Windows.Point((int)(item.StartX * picradio), (int)(item.StartY * picradio));
    //            figure.Segments.Add(arc);
    //            pathGeometry.Figures.Add(figure);
    //            path.Data = pathGeometry;
    //            path.Stroke = System.Windows.Media.Brushes.Black;
    //            path.StrokeThickness = 1;
    //            canvas.Children.Add(path);
    //        }
    //    }

    //}
}
