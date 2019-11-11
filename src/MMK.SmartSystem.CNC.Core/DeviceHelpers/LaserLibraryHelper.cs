using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using MMK.SmartSystem.WebCommon.Dto;

namespace MMK.SmartSystem.CNC.Core.DeviceHelpers
{
    public class LaserLibraryHelper : BaseHelper
    {
        #region CuttingData
        public string WriteCuttingDatas(List<CuttingDataToCncDto> cuttings)
        {
            if (cuttings == null && cuttings.Count() != LaserLibraryCuttingDataQuantity)
            {
                return "写入切割参数错误,记录数量有误";
            }

            cuttings = cuttings.OrderBy(x => x.ENo).ToList();

            ushort flib = 0;
            var ret_conn = BuildConnect(ref flib);
            if (ret_conn != 0)
            {
                FreeConnect(flib);
                return "写入切割参数错误，连接错误";
            }

            var ret = WriteCuttingDatasFunc(cuttings, flib);

            FreeConnect(flib);
            return ret;
        }

        public string WriteSingleCuttingData(CuttingDataToCncDto cutting)
        {
            ushort flib = 0;
            var ret_conn = BuildConnect(ref flib);
            if (ret_conn != 0)
            {
                FreeConnect(flib);
                return "写入切割参数错误，连接错误";
            }

            Focas1.IODBPSCD2 list = new Focas1.IODBPSCD2();

            var feed = cutting.Feedrate.GetDecimals();
            var disp = cutting.StandardDisplacement.GetDecimalsWithReference(cutting.StandardDisplacement2);
            var disp2 = cutting.StandardDisplacement2.GetDecimalsWithReference(cutting.StandardDisplacement);
            var supple = cutting.Supple.GetDecimals();

            list.data1.slct = 32767;
            list.data1.feed = feed.Item1;
            list.data1.power = cutting.Power;
            list.data1.freq = cutting.Frequency;
            list.data1.duty = cutting.Duty;
            list.data1.g_press = (short)(cutting.GasPressure * 100);
            list.data1.g_kind = cutting.GasCode;
            list.data1.g_ready_t = (short)(cutting.GasSettingTime * 10);
            list.data1.displace = (short)disp.Item1;
            list.data1.supple = supple.Item1;
            list.data1.edge_slt = cutting.EdgeSlt;
            list.data1.appr_slt = cutting.ApprSlt;
            list.data1.pwr_ctrl = cutting.PwrCtrl;
            list.data1.displace2 = disp2.Item1;
            list.data1.gap_axis = cutting.GapAxis;
            list.data1.feed_dec = (char)feed.Item2;
            list.data1.supple_dec = (char)supple.Item2;
            list.data1.dsp2_dec = (char)disp2.Item2;
            list.data1.pb_power = cutting.PbPower;

            short num = 1;
            short start_num = cutting.ENo;
            var ret = Focas1.cnc_wrpscdproc2(flib, start_num, ref num, list);
            if (ret != 0)
            {
                var err_msg = GetCuttingDataErrorMessage(flib);

                FreeConnect(flib);
                return $"写入切割参数错误,{err_msg}";
            }

            FreeConnect(flib);
            return null;

        }

        private string WriteCuttingDatasFunc(List<CuttingDataToCncDto> cuttings, ushort flib)
        {
            Focas1.IODBPSCD2 list = new Focas1.IODBPSCD2();
            short i = 0;
            foreach (var prop in list.GetType().GetProperties())
            {
                if (i >= cuttings.Count()) break;
                var feed = cuttings[i].Feedrate.GetDecimals();
                var disp = cuttings[i].StandardDisplacement.GetDecimalsWithReference(cuttings[i].StandardDisplacement2);
                var disp2 = cuttings[i].StandardDisplacement2.GetDecimalsWithReference(cuttings[i].StandardDisplacement);
                var supple = cuttings[i].Supple.GetDecimals();

                prop.SetValue(list, new Focas1.IODBPSCD2_data()
                {
                    slct = 32767,
                    feed = feed.Item1,
                    power = cuttings[i].Power,
                    freq = cuttings[i].Frequency,
                    duty = cuttings[i].Duty,
                    g_press = (short)(cuttings[i].GasPressure * 100),
                    g_kind = cuttings[i].GasCode,
                    g_ready_t = (short)(cuttings[i].GasSettingTime * 10),
                    displace = (short)disp.Item1,
                    supple = supple.Item1,
                    edge_slt = cuttings[i].EdgeSlt,
                    appr_slt = cuttings[i].ApprSlt,
                    pwr_ctrl = cuttings[i].PwrCtrl,
                    displace2 = disp2.Item1,
                    gap_axis = cuttings[i].GapAxis,
                    feed_dec = (char)feed.Item2,
                    supple_dec = (char)supple.Item2,
                    dsp2_dec = (char)disp2.Item2,
                    pb_power = cuttings[i].PbPower,
                }, null);

                i++;
                if (i >= LaserLibraryCuttingDataQuantity) break;
            }

            short num = LaserLibraryCuttingDataQuantity;
            var ret = Focas1.cnc_wrpscdproc2(flib, 1, ref num, list);
            if (ret != 0)
            {
                var err_msg = GetCuttingDataErrorMessage(flib);

                return $"写入切割参数错误,{err_msg}";
            }

            return null;
        }

        private string GetCuttingDataErrorMessage(ushort flib)
        {
            Focas1.ODBERR err = new Focas1.ODBERR();
            Focas1.cnc_getdtailerr(flib, err);
            string err_msg = "";
            switch (err.err_no)
            {
                case 1:
                    err_msg = "feedrate(第" + err.err_dtno.ToString() + "个)";
                    break;
                case 2:
                    err_msg = "cutting peak power(第" + err.err_dtno.ToString() + "个)";
                    break;
                case 3:
                    err_msg = "cutting frequency(第" + err.err_dtno.ToString() + "个)";
                    break;
                case 4:
                    err_msg = "cutting duty(第" + err.err_dtno.ToString() + "个)";
                    break;
                case 5:
                    err_msg = "assist gas pressure(第" + err.err_dtno.ToString() + "个)";
                    break;
                case 6:
                    err_msg = "assist gas select(第" + err.err_dtno.ToString() + "个)";
                    break;
                case 7:
                    err_msg = "assist gas setting time(第" + err.err_dtno.ToString() + "个)";
                    break;
                case 8:
                    err_msg = "reference displace(第" + err.err_dtno.ToString() + "个)";
                    break;
                case 9:
                    err_msg = "beam radius offset(第" + err.err_dtno.ToString() + "个)";
                    break;
                case 10:
                    err_msg = "edge cutting select(第" + err.err_dtno.ToString() + "个)";
                    break;
                case 11:
                    err_msg = "start-up select(第" + err.err_dtno.ToString() + "个)";
                    break;
                case 12:
                    err_msg = "power control(第" + err.err_dtno.ToString() + "个)";
                    break;
                case 13:
                    err_msg = "reference displace 2(第" + err.err_dtno.ToString() + "个)";
                    break;
                default:
                    err_msg = "其他错误" + err.err_no.ToString() + "(第" + err.err_dtno.ToString() + "个)";
                    break;

            }

            return err_msg;
        }

        #endregion

        #region EdgeCuttingData
        public string WriteEdgeCuttingDatas(List<EdgeCuttingDataToCncDto> edgeCuttings)
        {
            if (edgeCuttings == null && edgeCuttings.Count() != LaserLibraryEdgeCuttingDataQuantity)
            {
                return "写入尖角参数错误,记录数量有误";
            }

            edgeCuttings = edgeCuttings.OrderBy(x => x.ENo).ToList();

            ushort flib = 0;
            var ret_conn = BuildConnect(ref flib);
            if (ret_conn != 0)
            {
                FreeConnect(flib);
                return "写入尖角参数错误，连接错误";
            }

            var ret = WriteEdgeCuttingDatasFunc(edgeCuttings, flib);

            FreeConnect(flib);
            return ret;
        }

        public string WriteSingleEdgeCuttingData(EdgeCuttingDataToCncDto edgeCutting)
        {
            ushort flib = 0;
            var ret_conn = BuildConnect(ref flib);
            if (ret_conn != 0)
            {
                FreeConnect(flib);
                return "写入尖角参数错误，连接错误";
            }

            Focas1.IODBEDGE2 list = new Focas1.IODBEDGE2();

            var angle = edgeCutting.Angle.GetDecimals();
            var gap = edgeCutting.Gap.GetDecimals();
            var r_len = edgeCutting.RecoveryDistance.GetDecimals();
            var r_feed = edgeCutting.RecoveryFeedrate.GetDecimals();

            list.data1.slct = 32767;
            list.data1.power = edgeCutting.Power;
            list.data1.freq = edgeCutting.Frequency;
            list.data1.duty = edgeCutting.Duty;
            list.data1.g_press = (short)(edgeCutting.GasPressure * 100);
            list.data1.g_kind = edgeCutting.GasCode;
            list.data1.pier_t = edgeCutting.PiercingTime;
            list.data1.angle = angle.Item1;
            list.data1.gap = gap.Item1;
            list.data1.r_len = r_len.Item1;
            list.data1.r_feed = r_feed.Item1;
            list.data1.r_freq = edgeCutting.RecoveryFrequency;
            list.data1.r_duty = edgeCutting.RecoveryDuty;
            list.data1.gap_axis = edgeCutting.GapAxis;
            list.data1.angle_dec = (char)angle.Item2;
            list.data1.gap_dec = (char)gap.Item2;
            list.data1.r_len_dec = (char)r_len.Item2;
            list.data1.r_feed_dec = (char)r_feed.Item2;
            list.data1.pb_power = edgeCutting.PbPower;

            short num = 1;
            short start_num = edgeCutting.ENo;
            var ret = Focas1.cnc_wrpscdedge2(flib, start_num, ref num, list);
            if (ret != 0)
            {
                var err_msg = GetEdgeCuttingDataErrorMessage(flib);

                FreeConnect(flib);
                return $"写入尖角参数错误,{err_msg}";
            }

            FreeConnect(flib);
            return null;

        }

        private string WriteEdgeCuttingDatasFunc(List<EdgeCuttingDataToCncDto> edgeCuttings,ushort flib)
        {
            Focas1.IODBEDGE2 list = new Focas1.IODBEDGE2();
            int i = 0;
            foreach (var prop in list.GetType().GetProperties())
            {
                if (i >= edgeCuttings.Count()) break;

                var angle = edgeCuttings[i].Angle.GetDecimals();
                var gap = edgeCuttings[i].Gap.GetDecimals();
                var r_len = edgeCuttings[i].RecoveryDistance.GetDecimals();
                var r_feed = edgeCuttings[i].RecoveryFeedrate.GetDecimals();

                prop.SetValue(list, new Focas1.IODBEDGE2_data()
                {
                    slct = 32767,
                    power = edgeCuttings[i].Power,
                    freq = edgeCuttings[i].Frequency,
                    duty = edgeCuttings[i].Duty,
                    g_press = (short)(edgeCuttings[i].GasPressure * 100),
                    g_kind = edgeCuttings[i].GasCode,
                    pier_t = edgeCuttings[i].PiercingTime,
                    angle = angle.Item1,
                    gap = gap.Item1,
                    r_len = r_len.Item1,
                    r_feed = r_feed.Item1,
                    r_freq = edgeCuttings[i].RecoveryFrequency,
                    r_duty = edgeCuttings[i].RecoveryDuty,
                    gap_axis = edgeCuttings[i].GapAxis,
                    angle_dec = (char)angle.Item2,
                    gap_dec = (char)gap.Item2,
                    r_len_dec = (char)r_len.Item2,
                    r_feed_dec = (char)r_feed.Item2,
                    pb_power = edgeCuttings[i].PbPower,
                }, null);

                i++;

                if (i >= LaserLibraryEdgeCuttingDataQuantity) break;
            }

            short num = LaserLibraryEdgeCuttingDataQuantity;
            var ret = Focas1.cnc_wrpscdedge2(flib, 201, ref num, list);
            if (ret != 0)
            {
                var err_msg = GetEdgeCuttingDataErrorMessage(flib);

                return $"写入尖角参数错误,{err_msg}";
            }

            return null;
        }

        private string GetEdgeCuttingDataErrorMessage(ushort flib)
        {
            Focas1.ODBERR err = new Focas1.ODBERR();
            Focas1.cnc_getdtailerr(flib, err);
            string err_msg = "";
            switch (err.err_no)
            {
                case 1:
                    err_msg = "Edge operation angle(第" + err.err_dtno.ToString() + "个)";
                    break;
                case 2:
                    err_msg = "Peak power in piercing(第" + err.err_dtno.ToString() + "个)";
                    break;
                case 3:
                    err_msg = "Frequency in piercing(第" + err.err_dtno.ToString() + "个)";
                    break;
                case 4:
                    err_msg = "Duty in piercing(第" + err.err_dtno.ToString() + "个)";
                    break;
                case 5:
                    err_msg = "Time in piercing(第" + err.err_dtno.ToString() + "个)";
                    break;
                case 6:
                    err_msg = "Assist gas pressure in piercing(第" + err.err_dtno.ToString() + "个)";
                    break;
                case 7:
                    err_msg = "Assist gas type in piercing(第" + err.err_dtno.ToString() + "个)";
                    break;
                case 8:
                    err_msg = "Return distance(第" + err.err_dtno.ToString() + "个)";
                    break;
                case 9:
                    err_msg = "Return speed(第" + err.err_dtno.ToString() + "个)";
                    break;
                case 10:
                    err_msg = "Return frequency(第" + err.err_dtno.ToString() + "个)";
                    break;
                case 11:
                    err_msg = "Return pulse duty(第" + err.err_dtno.ToString() + "个)";
                    break;
                case 12:
                    err_msg = "Standard displacement(第" + err.err_dtno.ToString() + "个)";
                    break;
                default:
                    err_msg = "其他错误" + err.err_no.ToString() + "(第" + err.err_dtno.ToString() + "个)";
                    break;

            }

            return err_msg;
        }
        #endregion

        #region PiercingData
        public string WritePiercingDatas(List<PiercingDataToCncDto> piercings)
        {
            if (piercings == null && piercings.Count() != LaserLibraryPiercingDataQuantity)
            {
                return "写入穿孔参数错误,记录数量有误";
            }

            piercings = piercings.OrderBy(x => x.ENo).ToList();

            ushort flib = 0;
            var ret_conn = BuildConnect(ref flib);
            if (ret_conn != 0)
            {
                FreeConnect(flib);
                return "写入穿孔参数错误，连接错误";
            }

            var ret = WritePiercingDatasFunc(piercings, flib);

            FreeConnect(flib);
            return ret;
        }

        public string WriteSinglePiercingData(PiercingDataToCncDto piercing)
        {
            ushort flib = 0;
            var ret_conn = BuildConnect(ref flib);
            if (ret_conn != 0)
            {
                FreeConnect(flib);
                return "写入穿孔参数错误，连接错误";
            }

            Focas1.IODBPIRC list = new Focas1.IODBPIRC();

            var def_pos = piercing.StandardDisplacement.GetDecimalsWithReference(piercing.StandardDisplacement2);
            var def_pos2 = piercing.StandardDisplacement2.GetDecimalsWithReference(piercing.StandardDisplacement);

            list.data1.slct = 32767;
            list.data1.power = piercing.Power;
            list.data1.freq = piercing.Frequency;
            list.data1.duty = piercing.Duty;
            list.data1.i_freq = piercing.StepFrequency;
            list.data1.i_duty = piercing.StepDuty;
            list.data1.step_t = piercing.StepTime;
            list.data1.step_sum = piercing.StepQuantity;
            list.data1.pier_t = piercing.PiercingTime;
            list.data1.g_press = (short)(piercing.GasPressure * 100);
            list.data1.g_kind = piercing.GasCode;
            list.data1.g_time = (short)(piercing.GasSettingTime * 10);
            list.data1.def_pos = (short)def_pos.Item1;
            list.data1.def_pos2 = (short)def_pos2.Item1;
            list.data1.gap_axis = piercing.GapAxis;
            list.data1.def_pos2_dec = (char)def_pos2.Item2;
            list.data1.pb_power = piercing.PbPower;

            short num = 1;
            short start_num = piercing.ENo;
            var ret = Focas1.cnc_wrpscdpirc(flib, start_num, ref num, list);
            if (ret != 0)
            {
                var err_msg = GetPiercingDataErrorMessage(flib);

                FreeConnect(flib);
                return $"写入穿孔参数错误,{err_msg}";
            }

            FreeConnect(flib);
            return null;

        }

        private string WritePiercingDatasFunc(List<PiercingDataToCncDto> piercings, ushort flib)
        {
            Focas1.IODBPIRC list = new Focas1.IODBPIRC();
            short i = 0;
            foreach (var prop in list.GetType().GetProperties())
            {
                if (i >= piercings.Count()) break;

                var def_pos = piercings[i].StandardDisplacement.GetDecimalsWithReference(piercings[i].StandardDisplacement2);
                var def_pos2 = piercings[i].StandardDisplacement2.GetDecimalsWithReference(piercings[i].StandardDisplacement);

                prop.SetValue(list, new Focas1.IODBPIRC_data()
                {
                    slct = 32767,
                    power = piercings[i].Power,
                    freq = piercings[i].Frequency,
                    duty = piercings[i].Duty,
                    i_freq = piercings[i].StepFrequency,
                    i_duty = piercings[i].StepDuty,
                    step_t = piercings[i].StepTime,
                    step_sum = piercings[i].StepQuantity,
                    pier_t = piercings[i].PiercingTime,
                    g_press = (short)(piercings[i].GasPressure * 100),
                    g_kind = piercings[i].GasCode,
                    g_time = (short)(piercings[i].GasSettingTime * 10),
                    def_pos = (short)def_pos.Item1,
                    def_pos2 = (short)def_pos2.Item1,
                    gap_axis = piercings[i].GapAxis,
                    def_pos2_dec = (char)def_pos2.Item2,
                    pb_power = piercings[i].PbPower,
                }, null);

                i++;
                if (i >= LaserLibraryPiercingDataQuantity) break;
            }

            short num = LaserLibraryPiercingDataQuantity;
            var ret = Focas1.cnc_wrpscdpirc(flib, 101, ref num, list);
            if (ret != 0)
            {
                var err_msg = GetPiercingDataErrorMessage(flib);

                return $"写入穿孔参数错误,{err_msg}";
            }

            return null;
        }

        private string GetPiercingDataErrorMessage(ushort flib)
        {
            Focas1.ODBERR err = new Focas1.ODBERR();
            Focas1.cnc_getdtailerr(flib, err);
            string err_msg = "";
            switch (err.err_no)
            {
                case 1:
                    err_msg = "peak power(第" + err.err_dtno.ToString() + "个)";
                    break;
                case 2:
                    err_msg = "initial frequency(第" + err.err_dtno.ToString() + "个)";
                    break;
                case 3:
                    err_msg = "initial duty(第" + err.err_dtno.ToString() + "个)";
                    break;
                case 4:
                    err_msg = "step frequency(第" + err.err_dtno.ToString() + "个)";
                    break;
                case 5:
                    err_msg = "step duty(第" + err.err_dtno.ToString() + "个)";
                    break;
                case 6:
                    err_msg = "step time(第" + err.err_dtno.ToString() + "个)";
                    break;
                case 7:
                    err_msg = "step number(第" + err.err_dtno.ToString() + "个)";
                    break;
                case 8:
                    err_msg = "piercing time(第" + err.err_dtno.ToString() + "个)";
                    break;
                case 9:
                    err_msg = "assist gas pressure(第" + err.err_dtno.ToString() + "个)";
                    break;
                case 10:
                    err_msg = "assist gas select(第" + err.err_dtno.ToString() + "个)";
                    break;
                case 11:
                    err_msg = "assist gas setting time(第" + err.err_dtno.ToString() + "个)";
                    break;
                case 12:
                    err_msg = "reference displace(第" + err.err_dtno.ToString() + "个)";
                    break;
                case 13:
                    err_msg = "reference displace 2(第" + err.err_dtno.ToString() + "个)";
                    break;
                default:
                    err_msg = "其他错误" + err.err_no.ToString() + "(第" + err.err_dtno.ToString() + "个)";
                    break;

            }

            return err_msg;
        }

        #endregion

        #region SlopeControlData
        public string WriteSlopeControlDatas(List<SlopeControlDataToCncDto> slopeControls)
        {
            if (slopeControls == null && slopeControls.Count() != LaserLibrarySlopeControlDataQuantity)
            {
                return "写入功率控制参数错误,记录数量有误";
            }

            slopeControls = slopeControls.OrderBy(x => x.ENo).ToList();

            ushort flib = 0;
            var ret_conn = BuildConnect(ref flib);
            if (ret_conn != 0)
            {
                FreeConnect(flib);
                return "写入功率控制参数错误，连接错误";
            }

            var ret = WriteSlopeControlDatasFunc(slopeControls, flib);

            FreeConnect(flib);
            return ret;
        }

        public string WriteSingleSlopeControlData(SlopeControlDataToCncDto slopeControl)
        {
            ushort flib = 0;
            var ret_conn = BuildConnect(ref flib);
            if (ret_conn != 0)
            {
                FreeConnect(flib);
                return "写入功率控制参数错误，连接错误";
            }

            Focas1.IODBPWRCTL list = new Focas1.IODBPWRCTL();

            var feed_r = slopeControl.FeedrateR.GetDecimals();

            list.data1.slct = 16383;
            list.data1.power_min = slopeControl.PowerMin;
            list.data1.pwr_sp_zr = slopeControl.PowerSpeedZero;
            list.data1.freq_min = slopeControl.FrequencyMin;
            list.data1.freq_sp_zr = slopeControl.FrequencySpeedZero;
            list.data1.duty_min = slopeControl.DutyMin;
            list.data1.duty_sp_zr = slopeControl.DutySpeedZero;
            list.data1.feed_r_dec = (char)feed_r.Item2;
            list.data1.feed_r = feed_r.Item1;
            list.data1.ag_press_min = slopeControl.GasPressMin;
            list.data1.ag_press_sp_zr = slopeControl.GasPressSpeedZero;
            list.data1.pb_power_min = slopeControl.PbPowerMin;
            list.data1.pb_pwr_sp_zr = slopeControl.PbPowerSpeedZero;

            short num = 1;
            short start_num = slopeControl.ENo;
            var ret = Focas1.cnc_wrlpscdpwrctl(flib, start_num, ref num, list);
            if (ret != 0)
            {
                var err_msg = GetSlopeControlDataErrorMessage(flib);

                FreeConnect(flib);
                return $"写入功率控制参数错误,{err_msg}";
            }

            FreeConnect(flib);
            return null;

        }

        private string WriteSlopeControlDatasFunc(List<SlopeControlDataToCncDto> slopeControls,ushort flib)
        {
            Focas1.IODBPWRCTL list = new Focas1.IODBPWRCTL();
            short i = 0;
            foreach (var prop in list.GetType().GetProperties())
            {
                if (i >= slopeControls.Count()) break;

                var feed_r = slopeControls[i].FeedrateR.GetDecimals();

                prop.SetValue(list, new Focas1.IODBPWRCTL_data()
                {
                    slct = 16383,
                    power_min = slopeControls[i].PowerMin,
                    pwr_sp_zr = slopeControls[i].PowerSpeedZero,
                    freq_min = slopeControls[i].FrequencyMin,
                    freq_sp_zr = slopeControls[i].FrequencySpeedZero,
                    duty_min = slopeControls[i].DutyMin,
                    duty_sp_zr = slopeControls[i].DutySpeedZero,
                    feed_r_dec = (char)feed_r.Item2,
                    feed_r = feed_r.Item1,
                    ag_press_min = slopeControls[i].GasPressMin,
                    ag_press_sp_zr = slopeControls[i].GasPressSpeedZero,
                    pb_power_min = slopeControls[i].PbPowerMin,
                    pb_pwr_sp_zr = slopeControls[i].PbPowerSpeedZero,
                }, null);

                i++;
                if (i >= LaserLibrarySlopeControlDataQuantity) break;
            }

            short num = LaserLibrarySlopeControlDataQuantity;
            var ret = Focas1.cnc_wrlpscdpwrctl(flib, 901, ref num, list);
            if (ret != 0)
            {
                var err_msg = GetSlopeControlDataErrorMessage(flib);

                return $"写入功率控制参数错误,{err_msg}";
            }

            return null;
        }

        private string GetSlopeControlDataErrorMessage(ushort flib)
        {
            Focas1.ODBERR err = new Focas1.ODBERR();
            Focas1.cnc_getdtailerr(flib, err);
            string err_msg = "";
            switch (err.err_no)
            {
                case 1:
                    err_msg = "peak power(第" + err.err_dtno.ToString() + "个)";
                    break;
                case 2:
                    err_msg = "initial frequency(第" + err.err_dtno.ToString() + "个)";
                    break;
                case 3:
                    err_msg = "initial duty(第" + err.err_dtno.ToString() + "个)";
                    break;
                case 4:
                    err_msg = "step frequency(第" + err.err_dtno.ToString() + "个)";
                    break;
                case 5:
                    err_msg = "step duty(第" + err.err_dtno.ToString() + "个)";
                    break;
                case 6:
                    err_msg = "step time(第" + err.err_dtno.ToString() + "个)";
                    break;
                case 7:
                    err_msg = "step number(第" + err.err_dtno.ToString() + "个)";
                    break;
                case 8:
                    err_msg = "piercing time(第" + err.err_dtno.ToString() + "个)";
                    break;
                case 9:
                    err_msg = "assist gas pressure(第" + err.err_dtno.ToString() + "个)";
                    break;
                case 10:
                    err_msg = "assist gas select(第" + err.err_dtno.ToString() + "个)";
                    break;
                case 11:
                    err_msg = "assist gas setting time(第" + err.err_dtno.ToString() + "个)";
                    break;
                case 12:
                    err_msg = "reference displace(第" + err.err_dtno.ToString() + "个)";
                    break;
                case 13:
                    err_msg = "reference displace 2(第" + err.err_dtno.ToString() + "个)";
                    break;
                default:
                    err_msg = "其他错误" + err.err_no.ToString() + "(第" + err.err_dtno.ToString() + "个)";
                    break;

            }

            return err_msg;
        }

        #endregion
    }
}
