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

            LaserSimulater laser = new LaserSimulater(48,1000,96,12000);
            ProgramDetailDto info = new ProgramDetailDto();
            helper.GetProgramCommentInfo(laser, resovleDto.FilePath, info);

            //string progStr = "";
            //helper.GetProgramString(resovleDto.FilePath, ref progStr);

            
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
            //string saveFullName = $"{resovleDto.BmpPath}\\{resovleDto.FileName}.bmp";
            //helper.DrawXYThumbnai(dBlocks, 0.2, 680, 460, rWidth, rHeight, saveFullName);

            return new ProgramResolveResultDto()
            {
                ConnectId = resovleDto.ConnectId,
                Data = info,
                ImagePath = "",
                BmpName = "",
            };

        }
    }

    public class LaserSimulater
    {
        private double _feedAccTime = 32;//sec
        public double FeedAccTime//msec
        {
            get
            {
                return _feedAccTime * 1000.0;
            }
            set
            {
                if (value != _feedAccTime * 1000.0)
                {
                    _feedAccTime = value / 1000.0;

                    _feedAcc = _feedSpeed / _feedAccTime;
                    _wholeMinFeedAccTravel = _feedAcc * _feedAccTime * _feedAccTime;
                }
            }
        }

        private double _feedSpeed = 1000;//mm/sec
        public int FeedSpeed//mm/min
        {
            get
            {
                return (int)(_feedSpeed * 60);
            }
            set
            {
                if (value != _feedSpeed * 60)
                {
                    _feedSpeed = value / 60.0;

                    _feedAcc = _feedSpeed / _feedAccTime;
                    _wholeMinFeedAccTravel = _feedAcc * _feedAccTime * _feedAccTime;
                }
            }
        }

        private double _feedAcc;// mm/(sec*sec)
        private double _wholeMinFeedAccTravel = 0;//mm

        private double _rapidAccTime = 24;//sec
        public double RapidAccTime//msec
        {
            get
            {
                return _rapidAccTime * 1000.0;
            }
            set
            {
                if (value != _rapidAccTime * 1000)
                {
                    _rapidAccTime = value / 1000.0;

                    _rapidAcc = _rapidSpeed / _rapidAccTime;
                    _wholeMinRapidAccTravel = _rapidAcc * _rapidAccTime * _rapidAccTime;
                }
            }
        }

        private double _rapidSpeed = 8000;//mm/sec
        public int RapidSpeed//mm/min
        {
            get
            {
                return (int)(_rapidSpeed * 60);
            }
            set
            {
                if (value != _rapidSpeed * 60)
                {
                    _rapidSpeed = value / 60.0;

                    _rapidAcc = _rapidSpeed / _rapidAccTime;
                    _wholeMinRapidAccTravel = _rapidAcc * _rapidAccTime * _rapidAccTime;

                }
            }
        }

        private double _rapidAcc;// mm/(sec*sec)
        private double _wholeMinRapidAccTravel = 0;//mm

        public LaserSimulater(double feedAcc, int feedSpeed, double rapidAcc, int rapidSpeed)
        {
            FeedAccTime = feedAcc;
            FeedSpeed = feedSpeed;
            RapidAccTime = rapidAcc;
            RapidSpeed = rapidSpeed;
        }

        public double GetFeedTime(double length)
        {
            if (length <= _wholeMinFeedAccTravel)
            {
                return Math.Sqrt(length / _feedAcc) * 2;
            }

            return (length - _wholeMinFeedAccTravel) / ((double)_feedSpeed) + _feedAccTime * 2;
        }

        public double GetRapidTime(double length)
        {
            if (length <= _wholeMinRapidAccTravel)
            {
                return Math.Sqrt(length / _rapidAcc) * 2;
            }

            return (length - _wholeMinRapidAccTravel) / ((double)_rapidSpeed) + _rapidAccTime * 2;
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

        public double? S_Adr { get; set; }

        public double? P_Adr { get; set; }

        public double? Q_Adr { get; set; }

        public double? F_Adr { get; set; }

        public double? H_Adr { get; set; }

        public ProgramBlock()
        {
            G_Codes = new double?[3];
            M_Codes = new double?[3];

            G_Code_Count = 0;
            M_Code_Count = 0;
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

    public class DrawBlock
    {
        public double G01Group { get; set; }
        public double? G00Group { get; set; }

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

        public double? S_Adr { get; set; }

        public double? P_Adr { get; set; }

        public double? Q_Adr { get; set; }

        public double? F_Adr { get; set; }

        public double? H_Adr { get; set; }

    }

    public class LaserProgramHelper
    {
        private int LaserCommentLineCount = 10;

        private void GetProgramString(string ncPath, ref string str)
        {
            using (StreamReader sr = new StreamReader(ncPath))
            {
                str = sr.ReadToEnd();
            }
        }

        private void ProgramBlockPreDecompile(string str, ProgramDetailDto info, List<ProgramBlock> pBlocks)
        {
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
                        case 'F':
                            dblock.F_Adr = double.Parse(nums[i + 1]);
                            break;
                        case 'S':
                            dblock.S_Adr = double.Parse(nums[i + 1]);
                            break;
                        case 'P':
                            dblock.P_Adr = double.Parse(nums[i + 1]);
                            break;
                        case 'Q':
                            dblock.Q_Adr = double.Parse(nums[i + 1]);
                            break;
                        case 'H':
                            dblock.F_Adr = double.Parse(nums[i + 1]);
                            break;
                        default:
                            break;


                    }
                }

                pBlocks.Add(dblock);


                blockIndex++;
            }

        }

        private void ProgramBlockDecompile(List<ProgramBlock> pBlocks, List<DrawBlock> dBlocks)
        {
            double lastG01Group = 0;// G01 G02 G03 G00
            double lastG03Group = 90;// G90 G91

            double lastX = 0;
            double lastY = 0;
            double lastZ = 0;
            double lastF = 1000;

            foreach (var pitem in pBlocks)
            {


                if (pitem.F_Adr != null) lastF = pitem.F_Adr.Value;

                var g03Group = pitem.G_Codes.Where(x => x.HasValue == true).Where(x => x.Value == 91 || x.Value == 90).FirstOrDefault();
                if (g03Group != null)
                {
                    lastG03Group = g03Group.Value;
                }

                var g01Group = pitem.G_Codes.Where(x => x.HasValue == true).Where(x => x.Value >= 0 && x.Value <= 3).FirstOrDefault();
                if (g01Group != null)
                {
                    if (g01Group.Value == 1 || g01Group.Value == 0)
                    {
                        lastG01Group = g01Group.Value;
                    }

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
                    ditem.G01Group = g01Group.Value;
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
                    ditem.F_Adr = lastF;
                    ditem.S_Adr = pitem.S_Adr;
                    ditem.P_Adr = pitem.P_Adr;
                    ditem.Q_Adr = pitem.Q_Adr;

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
                    ditem.F_Adr = lastF;
                    ditem.S_Adr = pitem.S_Adr;
                    ditem.P_Adr = pitem.P_Adr;
                    ditem.Q_Adr = pitem.Q_Adr;

                    dBlocks.Add(ditem);

                    lastX = ditem.EndX;
                    lastY = ditem.EndY;
                    lastZ = ditem.EndZ;
                }

                var g24group = pitem.G_Codes.Where(x => x.HasValue == true).Where(x => x.Value == 24).FirstOrDefault();
                if (g24group != null)
                {
                    var ditem = new DrawBlock();
                    ditem.G01Group = lastG01Group;
                    ditem.G00Group = g24group.Value;
                    ditem.StartX = lastX;
                    ditem.StartY = lastY;
                    ditem.StartZ = lastZ;
                    ditem.EndX = lastX;
                    ditem.EndY = lastY;
                    ditem.EndZ = lastZ;
                    ditem.I_Adr = pitem.I_Adr;
                    ditem.J_Adr = pitem.J_Adr;
                    ditem.K_Adr = pitem.K_Adr;
                    ditem.S_Adr = pitem.S_Adr;
                    ditem.P_Adr = pitem.P_Adr;
                    ditem.Q_Adr = pitem.Q_Adr;
                    ditem.H_Adr = pitem.H_Adr;
                    ditem.R_Adr = pitem.R_Adr;
                    dBlocks.Add(ditem);
                }
            }
        }

        private void ProgramInfoAnalysis(LaserSimulater laser, ProgramDetailDto info, List<DrawBlock> dBlocks)
        {
            int total_pericing_count = 0;
            double total_cutting_length = 0;
            double total_cycletime = 0;

            foreach (var block in dBlocks)
            {
                if (block.G01Group == 0)
                {
                    var length = Math.Sqrt(Math.Pow((block.EndX - block.StartX), 2) + Math.Pow((block.EndY - block.StartY), 2) + Math.Pow((block.EndZ - block.StartZ), 2));
                    total_cycletime += laser.GetRapidTime(length);
                }
                else if (block.G01Group == 1)
                {
                    laser.FeedSpeed = (int)block.F_Adr.Value;
                    var length = Math.Sqrt(Math.Pow((block.EndX - block.StartX), 2) + Math.Pow((block.EndY - block.StartY), 2));
                    total_cycletime += laser.GetFeedTime(length);
                    total_cutting_length += length;
                }
                else if (block.G01Group == 2 && !block.R_Adr.HasValue)
                {
                    double center_x = block.StartX + block.I_Adr.Value;
                    double center_y = block.StartY + block.J_Adr.Value;
                    double radius = Math.Round(Math.Sqrt(Math.Pow(block.I_Adr.Value, 2) + Math.Pow(block.J_Adr.Value, 2)), 2);

                    float startArc = (float)(Math.Atan2(-block.J_Adr.Value, -block.I_Adr.Value) * 180 / Math.PI);
                    float endArc = (float)(Math.Atan2(block.EndY - center_y, block.EndX - center_x) * 180 / Math.PI);

                    var angle = startArc.ToGraphicsSweep(endArc, true);
                    var length = Math.PI * radius * 2 * angle / 360;
                    total_cycletime += laser.GetFeedTime(length);
                    total_cutting_length += length;
                }
                else if (block.G01Group == 2 && block.R_Adr.HasValue)
                {
                    var chord = Math.Sqrt((block.EndX - block.StartX) * (block.EndX - block.StartX) + (block.EndY - block.StartY) * (block.EndY - block.StartY));
                    var angle = Math.Asin(chord / 2 / block.R_Adr.Value) * 2;

                    var length = block.R_Adr.Value * angle;
                    total_cycletime += laser.GetFeedTime(length);
                    total_cutting_length += length;
                }
                else if (block.G01Group == 3 && !block.R_Adr.HasValue)
                {
                    double center_x = block.StartX + block.I_Adr.Value;
                    double center_y = block.StartY + block.J_Adr.Value;
                    double radius = Math.Round(Math.Sqrt(Math.Pow(block.I_Adr.Value, 2) + Math.Pow(block.J_Adr.Value, 2)), 2);

                    float startArc = (float)(Math.Atan2(-block.J_Adr.Value, -block.I_Adr.Value) * 180 / Math.PI);
                    float endArc = (float)(Math.Atan2(block.EndY - center_y, block.EndX - center_x) * 180 / Math.PI);

                    var angle = -startArc.ToGraphicsSweep(endArc, false);
                    var length = radius * 2 * angle / Math.PI;
                    total_cycletime += laser.GetFeedTime(length);
                    total_cutting_length += length;
                }
                else if (block.G01Group == 3 && block.R_Adr.HasValue)
                {
                    var chord = Math.Sqrt((block.EndX - block.StartX) * (block.EndX - block.StartX) + (block.EndY - block.StartY) * (block.EndY - block.StartY));
                    var angle = Math.Asin(chord / 2 / block.R_Adr.Value) * 2;

                    var length = block.R_Adr.Value * angle;
                    total_cycletime += laser.GetFeedTime(length);
                    total_cutting_length += length;
                }
                else if (block.G00Group == 24)
                {
                    total_pericing_count++;
                }
            }

            info.CuttingTime = total_cycletime;
            info.CuttingDistance = total_cutting_length;
            info.PiercingCount = total_pericing_count;
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

        public void GetProgramCommentInfo(LaserSimulater laser, string ncPath, ProgramDetailDto info)
        {
            string str = "";
            GetProgramString(ncPath, ref str);

            List<ProgramBlock> pBlocks = new List<ProgramBlock>();
            ProgramBlockPreDecompile(str, info, pBlocks);

            List<DrawBlock> dBlocks = new List<DrawBlock>();
            ProgramBlockDecompile(pBlocks, dBlocks);

            ProgramInfoAnalysis(laser, info, dBlocks);
        }

    }

    public class LaserProgramThumbnai
    {
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
    }

}
