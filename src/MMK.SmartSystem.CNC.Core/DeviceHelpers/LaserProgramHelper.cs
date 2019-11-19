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
            helper.GetProgramCommentInfo(laser, resovleDto.FilePath, info, false, "", 0, 0);

            return new ProgramResolveResultDto()
            {
                ConnectId = resovleDto.ConnectId,
                Id= "ProgramResolve",
                Data = info,
                ImagePath = "",
                BmpName = "",
            };

        }
    }

    public enum DrawBlockTypeEnum
    {
        LINE,
        ARC,
    }

    public static class LaserMath
    {
        public static double ToGraphicsArc(this double data)
        {
            if (data < 0) data = 2 * Math.PI + data;
            return data;
        }

        public static double ToGraphicsSweep(this double start, double end, bool cw)
        {
            var temp_start = start;
            var temp_end = end;

            if (cw == true)
            {
                if (temp_start < temp_end)
                {
                    return Math.PI * 2 - (temp_end - temp_start);
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
                    return (temp_start - temp_end) - Math.PI * 2;
                }
            }

        }

        public static double GetAngleFromGraphicsArc(bool iscw, double s_arc, double e_arc)
        {
            if (iscw == true)
            {
                return s_arc > e_arc ? s_arc - e_arc : 2 * Math.PI - e_arc + s_arc;
            }
            else
            {
                return s_arc < e_arc ? e_arc - s_arc : 2 * Math.PI + e_arc - s_arc;
            }
        }

        public static bool GetCenterFromR(bool iscw, double s_x, double s_y, double e_x, double e_y, double r, ref double c_x, ref double c_y)
        {
            double r_pow = r * r;

            var e = (-2 * s_x + 2 * e_x) / (2 * s_y - 2 * e_y);
            var f = (s_x * s_x - e_x * e_x + s_y * s_y - e_y * e_y) /
                (2 * s_y - 2 * e_y);

            var a = 1 + e * e;
            var b = -2 * s_x + 2 * e * f - 2 * s_y * e;
            var c = -r_pow + s_x * s_x + s_y * s_y + f * f - 2 * s_y * f;

            var judge = b * b - 4 * a * c;
            if (judge >= 0)
            {
                var center_x_1 = (-b + Math.Sqrt(b * b - 4 * a * c)) / (2 * a);
                var center_y_1 = e * center_x_1 + f;

                var center_x_2 = (-b - Math.Sqrt(b * b - 4 * a * c)) / (2 * a);
                var center_y_2 = e * center_x_2 + f;

                var arr_chord_x = e_x - s_x;
                var arr_chord_y = e_y - s_y;

                var arr_r_x_1 = center_x_1 - s_x;
                var arr_r_y_1 = center_y_1 - s_y;

                var temp = arr_chord_x * arr_r_y_1 - arr_chord_y * arr_r_x_1;
                if ((temp <= 0 && iscw == true) || (temp >= 0 && iscw == false))
                {
                    c_x = center_x_1;
                    c_y = center_y_1;
                }
                else
                {
                    c_x = center_x_2;
                    c_y = center_y_2;
                }

                return true;
            }

            return false;
        }

        public static void GetQuadrantBoundInfo(bool iscw, double s_arc, double e_arc, out bool[] bs)
        {
            bs = new bool[4] { false, false, false, false };


            if (iscw == true)
            {
                e_arc = e_arc > s_arc ? e_arc - Math.PI * 2 : e_arc;
                var s_arc_b = Math.Floor(s_arc / Math.PI * 2) * Math.PI / 2;
                while (s_arc_b >= e_arc)
                {
                    var temp_index = (int)Math.Floor(s_arc_b / Math.PI * 2);
                    var b_index = temp_index >= 0 ? temp_index : 4 + temp_index;
                    bs[b_index] = true;

                    s_arc_b = s_arc_b - Math.PI / 2;
                }
            }
            else
            {
                e_arc = e_arc < s_arc ? e_arc + Math.PI * 2 : e_arc;
                var s_arc_b = Math.Ceiling(s_arc / Math.PI * 2) * Math.PI / 2;
                while (s_arc_b <= e_arc)
                {
                    var temp_index = (int)Math.Floor(s_arc_b / Math.PI * 2);
                    var b_index = temp_index >= 4 ? temp_index - 4 : temp_index;
                    bs[b_index] = true;

                    s_arc_b = s_arc_b + Math.PI / 2;
                }
            }
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

    public class PreProgramBlock
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

        public PreProgramBlock()
        {
            G_Codes = new double?[3];
            M_Codes = new double?[3];

            G_Code_Count = 0;
            M_Code_Count = 0;
        }

    }

    public class ProgramBlockInfo
    {
        public double CuttingLength { get; set; }

        public double DirectTime { get; set; }

        public int PiercingCount { get; set; }

        public double? Min_X { get; set; }

        public double? Min_Y { get; set; }

        public double? Max_X { get; set; }

        public double? Max_Y { get; set; }

        public DrawBlock DrawBlockInfo { get; set; } = new DrawBlock();
    }

    public class ProgramBlock
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

        public ProgramBlockInfo Info
        {
            get
            {
                var tempInfo = new ProgramBlockInfo();

                if (G01Group == 1)
                {
                    tempInfo.CuttingLength = Math.Sqrt(Math.Pow((EndX - StartX), 2) + Math.Pow((EndY - StartY), 2) + Math.Pow((EndZ - StartZ), 2));
                    tempInfo.Min_X = EndX > StartX ? StartX : EndX;
                    tempInfo.Min_Y = EndY > StartY ? StartY : EndY;
                    tempInfo.Max_X = EndX < StartX ? StartX : EndX;
                    tempInfo.Max_Y = EndY < StartY ? StartY : EndY;

                    tempInfo.DrawBlockInfo.Type = DrawBlockTypeEnum.LINE;
                    tempInfo.DrawBlockInfo.StartX = StartX;
                    tempInfo.DrawBlockInfo.StartY = StartY;
                    tempInfo.DrawBlockInfo.EndX = EndX;
                    tempInfo.DrawBlockInfo.EndY = EndY;
                    return tempInfo;
                }
                else if (G01Group == 2 && !R_Adr.HasValue)
                {

                    double center_x = StartX + I_Adr.Value;
                    double center_y = StartY + J_Adr.Value;
                    double radius = Math.Round(Math.Sqrt(Math.Pow(I_Adr.Value, 2) + Math.Pow(J_Adr.Value, 2)), 2);

                    var startArc = Math.Atan2(-J_Adr.Value, -I_Adr.Value).ToGraphicsArc();
                    var endArc = Math.Atan2(EndY - center_y, EndX - center_x).ToGraphicsArc();

                    var angle = LaserMath.GetAngleFromGraphicsArc(true, startArc, endArc);
                    tempInfo.CuttingLength = radius * angle;

                    bool[] bs;
                    LaserMath.GetQuadrantBoundInfo(true, startArc, endArc, out bs);
                    tempInfo.Max_X = bs[0] == true ? center_x + radius : (EndX > StartX ? EndX : StartX);
                    tempInfo.Max_Y = bs[1] == true ? center_y + radius : (EndY > StartY ? EndY : StartY);
                    tempInfo.Min_X = bs[2] == true ? center_x - radius : (EndX < StartX ? EndX : StartX);
                    tempInfo.Min_Y = bs[3] == true ? center_y - radius : (EndY < StartY ? EndY : StartY);

                    tempInfo.DrawBlockInfo.Type = DrawBlockTypeEnum.ARC;
                    tempInfo.DrawBlockInfo.IsArcCw = true;
                    tempInfo.DrawBlockInfo.StartX = StartX;
                    tempInfo.DrawBlockInfo.StartY = StartY;
                    tempInfo.DrawBlockInfo.EndX = EndX;
                    tempInfo.DrawBlockInfo.EndY = EndY;
                    tempInfo.DrawBlockInfo.StartArc = startArc;
                    tempInfo.DrawBlockInfo.EndArc = endArc;
                    tempInfo.DrawBlockInfo.CenterX = center_x;
                    tempInfo.DrawBlockInfo.CenterY = center_y;
                    tempInfo.DrawBlockInfo.Radius = radius;
                }
                else if (G01Group == 2 && R_Adr.HasValue)
                {
                    double center_x = 0;
                    double center_y = 0;
                    double radius = R_Adr.Value;
                    var rRet = LaserMath.GetCenterFromR(true, StartX, StartY, EndX, EndY, R_Adr.Value, ref center_x, ref center_y);

                    if (rRet == true)
                    {
                        I_Adr = center_x - StartX;
                        J_Adr = center_y - StartY;

                        var startArc = Math.Atan2(-J_Adr.Value, -I_Adr.Value).ToGraphicsArc();
                        var endArc = Math.Atan2(EndY - center_y, EndX - center_x).ToGraphicsArc();

                        var angle = LaserMath.GetAngleFromGraphicsArc(true, startArc, endArc);
                        tempInfo.CuttingLength = R_Adr.Value * angle;

                        bool[] bs;
                        LaserMath.GetQuadrantBoundInfo(true, startArc, endArc, out bs);
                        tempInfo.Max_X = bs[0] == true ? center_x + radius : (EndX > StartX ? EndX : StartX);
                        tempInfo.Max_Y = bs[1] == true ? center_y + radius : (EndY > StartY ? EndY : StartY);
                        tempInfo.Min_X = bs[2] == true ? center_x - radius : (EndX < StartX ? EndX : StartX);
                        tempInfo.Min_Y = bs[3] == true ? center_y - radius : (EndY < StartY ? EndY : StartY);

                        tempInfo.DrawBlockInfo.Type = DrawBlockTypeEnum.ARC;
                        tempInfo.DrawBlockInfo.IsArcCw = true;
                        tempInfo.DrawBlockInfo.StartX = StartX;
                        tempInfo.DrawBlockInfo.StartY = StartY;
                        tempInfo.DrawBlockInfo.EndX = EndX;
                        tempInfo.DrawBlockInfo.EndY = EndY;
                        tempInfo.DrawBlockInfo.StartArc = startArc;
                        tempInfo.DrawBlockInfo.EndArc = endArc;
                        tempInfo.DrawBlockInfo.CenterX = center_x;
                        tempInfo.DrawBlockInfo.CenterY = center_y;
                        tempInfo.DrawBlockInfo.Radius = radius;
                    }
                }
                else if (G01Group == 3 && !R_Adr.HasValue)
                {
                    double center_x = StartX + I_Adr.Value;
                    double center_y = StartY + J_Adr.Value;
                    double radius = Math.Round(Math.Sqrt(Math.Pow(I_Adr.Value, 2) + Math.Pow(J_Adr.Value, 2)), 2);
                    R_Adr = radius;

                    var startArc = Math.Atan2(-J_Adr.Value, -I_Adr.Value).ToGraphicsArc();
                    var endArc = Math.Atan2(EndY - center_y, EndX - center_x).ToGraphicsArc();

                    var angle = LaserMath.GetAngleFromGraphicsArc(false, startArc, endArc);
                    tempInfo.CuttingLength = radius * angle;

                    bool[] bs;
                    LaserMath.GetQuadrantBoundInfo(false, startArc, endArc, out bs);
                    tempInfo.Max_X = bs[0] == true ? center_x + radius : (EndX > StartX ? EndX : StartX);
                    tempInfo.Max_Y = bs[1] == true ? center_y + radius : (EndY > StartY ? EndY : StartY);
                    tempInfo.Min_X = bs[2] == true ? center_x - radius : (EndX < StartX ? EndX : StartX);
                    tempInfo.Min_Y = bs[3] == true ? center_y - radius : (EndY < StartY ? EndY : StartY);

                    tempInfo.DrawBlockInfo.Type = DrawBlockTypeEnum.ARC;
                    tempInfo.DrawBlockInfo.IsArcCw = false;
                    tempInfo.DrawBlockInfo.StartX = StartX;
                    tempInfo.DrawBlockInfo.StartY = StartY;
                    tempInfo.DrawBlockInfo.EndX = EndX;
                    tempInfo.DrawBlockInfo.EndY = EndY;
                    tempInfo.DrawBlockInfo.StartArc = startArc;
                    tempInfo.DrawBlockInfo.EndArc = endArc;
                    tempInfo.DrawBlockInfo.CenterX = center_x;
                    tempInfo.DrawBlockInfo.CenterY = center_y;
                    tempInfo.DrawBlockInfo.Radius = radius;
                }
                else if (G01Group == 3 && R_Adr.HasValue)
                {
                    double center_x = 0;
                    double center_y = 0;
                    double radius = R_Adr.Value;
                    var rRet = LaserMath.GetCenterFromR(false, StartX, StartY, EndX, EndY, R_Adr.Value, ref center_x, ref center_y);

                    if (rRet == true)
                    {
                        I_Adr = center_x - StartX;
                        J_Adr = center_y - StartY;

                        var startArc = Math.Atan2(-J_Adr.Value, -I_Adr.Value).ToGraphicsArc();
                        var endArc = Math.Atan2(EndY - center_y, EndX - center_x).ToGraphicsArc();

                        var angle = LaserMath.GetAngleFromGraphicsArc(false, startArc, endArc);
                        tempInfo.CuttingLength = R_Adr.Value * angle;

                        bool[] bs;
                        LaserMath.GetQuadrantBoundInfo(false, startArc, endArc, out bs);
                        tempInfo.Max_X = bs[0] == true ? center_x + radius : (EndX > StartX ? EndX : StartX);
                        tempInfo.Max_Y = bs[1] == true ? center_y + radius : (EndY > StartY ? EndY : StartY);
                        tempInfo.Min_X = bs[2] == true ? center_x - radius : (EndX < StartX ? EndX : StartX);
                        tempInfo.Min_Y = bs[3] == true ? center_y - radius : (EndY < StartY ? EndY : StartY);

                        tempInfo.DrawBlockInfo.Type = DrawBlockTypeEnum.ARC;
                        tempInfo.DrawBlockInfo.IsArcCw = false;
                        tempInfo.DrawBlockInfo.StartX = StartX;
                        tempInfo.DrawBlockInfo.StartY = StartY;
                        tempInfo.DrawBlockInfo.EndX = EndX;
                        tempInfo.DrawBlockInfo.EndY = EndY;
                        tempInfo.DrawBlockInfo.StartArc = startArc;
                        tempInfo.DrawBlockInfo.EndArc = endArc;
                        tempInfo.DrawBlockInfo.CenterX = center_x;
                        tempInfo.DrawBlockInfo.CenterY = center_y;
                        tempInfo.DrawBlockInfo.Radius = radius;
                    }
                }
                else if(G00Group.HasValue&& G00Group.Value==24)
                {
                    tempInfo.DirectTime = R_Adr.Value;
                    tempInfo.PiercingCount = 1;
                }

                return tempInfo;
            }
        }


    }

    public class DrawBlock
    {
        public DrawBlockTypeEnum Type { get; set; }

        public bool IsArcCw { get; set; }

        public double StartX { get; set; }

        public double StartY { get; set; }

        public double EndX { get; set; }

        public double EndY { get; set; }

        public double CenterX { get; set; }

        public double CenterY { get; set; }

        public double StartArc { get; set; }

        public double EndArc { get; set; }

        public double Radius { get; set; }
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

        private void ProgramBlockPreDecompile(string str, ProgramDetailDto info, List<PreProgramBlock> preBlocks)
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


                var dblock = new PreProgramBlock();
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

                preBlocks.Add(dblock);


                blockIndex++;
            }

        }

        private void ProgramBlockDecompile(List<PreProgramBlock> preBlocks, List<ProgramBlock> pBlocks)
        {
            double lastG01Group = 0;// G01 G02 G03 G00
            double lastG03Group = 90;// G90 G91

            double lastX = 0;
            double lastY = 0;
            double lastZ = 0;
            double lastF = 1000;

            foreach (var pitem in preBlocks)
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

                    var ditem = new ProgramBlock();
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

                    pBlocks.Add(ditem);

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

                    var ditem = new ProgramBlock();
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

                    pBlocks.Add(ditem);

                    lastX = ditem.EndX;
                    lastY = ditem.EndY;
                    lastZ = ditem.EndZ;
                }

                var g24group = pitem.G_Codes.Where(x => x.HasValue == true).Where(x => x.Value == 24).FirstOrDefault();
                if (g24group != null)
                {
                    var ditem = new ProgramBlock();
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
                    pBlocks.Add(ditem);
                }
            }
        }

        private void ProgramInfoAnalysis(LaserSimulater laser, ProgramDetailDto info, List<ProgramBlock> pBlocks, List<DrawBlock> dBlocks)
        {
            int total_pericing_count = 0;
            double total_cutting_length = 0;
            double total_cycletime = 0;

            double? max_x = null, max_y = null, min_x = null, min_y = null;

            foreach (var block in pBlocks)
            {
                var tempInfo = block.Info;

                dBlocks.Add(tempInfo.DrawBlockInfo);

                laser.FeedSpeed = (int)block.F_Adr.Value;
                total_cycletime += laser.GetFeedTime(tempInfo.CuttingLength);
                total_cutting_length += tempInfo.CuttingLength;
                total_pericing_count += tempInfo.PiercingCount;
                total_cycletime += tempInfo.DirectTime / 1000.0;

                if (tempInfo.Max_X.HasValue) max_x = max_x.HasValue ? Math.Max(max_x.Value, tempInfo.Max_X.Value) : tempInfo.Max_X.Value;
                if (tempInfo.Max_Y.HasValue) max_y = max_y.HasValue ? Math.Max(max_y.Value, tempInfo.Max_Y.Value) : tempInfo.Max_Y.Value;
                if (tempInfo.Min_X.HasValue) min_x = min_x.HasValue ? Math.Min(min_x.Value, tempInfo.Min_X.Value) : tempInfo.Min_X.Value;
                if (tempInfo.Min_Y.HasValue) min_y = min_y.HasValue ? Math.Min(min_y.Value, tempInfo.Min_Y.Value) : tempInfo.Min_Y.Value;


            }

            info.CuttingTime = total_cycletime;
            info.CuttingDistance = total_cutting_length;
            info.PiercingCount = total_pericing_count;

            info.UsedPlateSize = $"X最小:{min_x.Value},X最大:{max_x.Value},Y最小:{min_y.Value},Y最大:{max_y.Value}";
            info.Max_X = max_x.Value;
            info.Max_Y = max_y.Value;
            info.Min_X = min_x.Value;
            info.Min_Y = min_y.Value;

            info.UsedPlateSize_W = max_x.Value;
            info.UsedPlateSize_H = max_y.Value;
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
            if (plateSizeMatch.Success == true)
            {
                info.PlateSize = plateSizeMatch.Value;
                var datas = plateSizeMatch.Value.Split('X');
                if(datas.Length==2)
                {
                    info.PlateSize_W = double.Parse(datas[0]);
                    info.PlateSize_H = double.Parse(datas[1]);
                }

            }

            Regex usedPlateSizeRegex = new Regex(@"(?<=\(#USED_SIZE=)\w*(?=\))");
            Match usedPlateSizeMatch = usedPlateSizeRegex.Match(blockStr);
            if (usedPlateSizeMatch.Success == true)
            {
                info.UsedPlateSize = usedPlateSizeMatch.Value;

                var datas = usedPlateSizeMatch.Value.Split('X');
                if (datas.Length == 2)
                {
                    info.UsedPlateSize_W = double.Parse(datas[0]);
                    info.UsedPlateSize_H = double.Parse(datas[1]);
                }
            }

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

        public void GetProgramCommentInfo(LaserSimulater laser, string ncPath, ProgramDetailDto info, bool bmp, string bmpPath, int bmpWidth, int bmpHeight)
        {
            string str = "";
            GetProgramString(ncPath, ref str);

            GetInfo(info, str);

            List<PreProgramBlock> preBlocks = new List<PreProgramBlock>();
            ProgramBlockPreDecompile(str, info, preBlocks);

            List<ProgramBlock> pBlocks = new List<ProgramBlock>();
            ProgramBlockDecompile(preBlocks, pBlocks);

            List<DrawBlock> dBlocks = new List<DrawBlock>();
            ProgramInfoAnalysis(laser, info, pBlocks, dBlocks);

            if (bmp == true)
            {
                var thumbnail = new LaserProgramThumbnai(bmpWidth, bmpHeight, Color.FromArgb(0xFF, 0x00, 0xD8, 0xFF), (float)0.05, Color.Yellow, (float)0.05, Color.FromArgb(0xFF, 0x28, 0x26, 0x20));
                thumbnail.DrawXYThumbnai(dBlocks, info.Max_X + 20, info.Max_Y + 20, bmpPath);
            }
        }

    }

    public class LaserProgramThumbnai
    {
        private Bitmap m_bmp;

        private Pen m_cutPen;

        private Pen m_rapidPen;

        private Color m_bakColor;

        public LaserProgramThumbnai(int picWidth, int picHeight, Color cutColor, float cutPenWidth, Color rapidColor, float rapidPenWidth, Color bakColor)
        {
            m_bmp = new Bitmap(picWidth, picHeight);

            m_cutPen = new Pen(cutColor, cutPenWidth);
            m_rapidPen = new Pen(rapidColor, rapidPenWidth);
            m_bakColor = bakColor;
        }


        public void DrawXYThumbnai(List<DrawBlock> dBlocks, double rWidth, double rHeight, string path)
        {
            float radioWidth = (float)(m_bmp.Width / rWidth);
            float radioHeight = (float)(m_bmp.Height / rHeight);

            float picradio = radioWidth > radioHeight ? radioHeight : radioWidth;

            //利用该图片对象生成画板
            Graphics graphic = Graphics.FromImage(m_bmp);
            //设置黑色背景
            graphic.Clear(m_bakColor);

            foreach (var item in dBlocks)
            {
                if (item.Type == DrawBlockTypeEnum.LINE)
                {
                    DrawXYLine(item, graphic, m_cutPen, picradio);
                }
                else if (item.Type == DrawBlockTypeEnum.ARC)
                {
                    DrawXYCircleWithIJ(item, graphic, m_cutPen, picradio);
                }

            }

            //释放资源
            graphic.Dispose();
            //注意：程序要有该目录下该文件的访问权限
            m_bmp.Save(path, ImageFormat.Bmp);
        }

        private void DrawXYLine(DrawBlock block, Graphics graphic, Pen pen, double picRadio)
        {
            graphic.DrawLine(pen,
                (float)(block.StartX * picRadio),
                (float)(block.StartY * picRadio),
                (float)(block.EndX * picRadio),
                (float)(block.EndY * picRadio));
        }

        private void DrawXYCircleWithIJ(DrawBlock block, Graphics graphic, Pen pen, float picRadio)
        {
            graphic.DrawArc(
                pen,
                (float)(block.CenterX - block.Radius) * picRadio,
                (float)(block.CenterY - block.Radius) * picRadio,
                (float)block.Radius * 2 * picRadio,
                (float)block.Radius * 2 * picRadio,
                (float)(block.EndArc * 180 / Math.PI),
                (float)(block.StartArc.ToGraphicsSweep(block.EndArc, block.IsArcCw) * 180 / Math.PI)
                );
        }
    }

}
