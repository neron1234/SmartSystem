using Abp.BackgroundJobs;
using Abp.Dependency;
using System.Collections.Concurrent;
using Abp.Events.Bus.Handlers;
using Microsoft.AspNetCore.SignalR;
using MMK.SmartSystem.RealTime.Hubs;
using MMK.SmartSystem.WebCommon.DeviceModel;
using MMK.SmartSystem.WebCommon.EventModel;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace MMK.SmartSystem.RealTime.DeviceHandlers
{

    public class CncHandler
    {
        string m_ip = "192.168.21.121";
        ushort m_port = 8193;
        int m_timeout = 10;
        double m_increment = 1000;
        ushort m_flib = 0;

        public event Action<string> ShowErrorLogEvent;
        public event Action<object> GetResultEvent;
        public static BlockingCollection<CncEventData> m_EventDatas = new BlockingCollection<CncEventData>();

        public BaseCNCResultModel<ReadProgramListItemResultModel> ReadProgramList(string folder)
        {

            ushort flib = 0;
            var ret = ConnectHelper.BuildConnect(ref flib, m_ip, m_port, m_timeout);

            if(ret==0)
            {
                var res = new List<ReadProgramListItemResultModel>();

                var ret_1 = ProgramListHelper.ReadProgramList(flib, folder, ref res);
                ConnectHelper.FreeConnect(flib);

                if (ret_1.Item1==0)
                {
                    return new BaseCNCResultModel<ReadProgramListItemResultModel>() { Value = res, ErrorMessage = ret_1.Item2 };
                }
                else
                {
                    return new BaseCNCResultModel<ReadProgramListItemResultModel>() { ErrorMessage = ret_1.Item2 };
                }

                
            }
            else
            {
                return new BaseCNCResultModel<ReadProgramListItemResultModel>() { ErrorMessage = "获得程序列表失败,连接失败" };
            }
            
        }

        public void Connect()
        {
            var ret = ConnectHelper.BuildConnect(ref m_flib, m_ip, m_port, m_timeout);

        }

        public void Execute()
        {

            List<CncEventData> tempEventDatas = new List<CncEventData>(m_EventDatas.ToArray());

            tempEventDatas.ForEach(item =>
            {
                string info = "";
                switch (item.Kind)
                {
                    case CncEventEnum.ReadPmc:
                        info = ReadPmcHandle(ref m_flib, item.Para);
                        break;
                    case CncEventEnum.ReadMacro:
                        info = ReadMacroHandle(ref m_flib, item.Para);
                        break;
                    case CncEventEnum.ReadPosition:
                        info = ReadPositionHandle(ref m_flib, item.Para);
                        break;
                    case CncEventEnum.ReadAlarm:
                        info = ReadAlarmHandle(ref m_flib, item.Para);
                        break;
                    case CncEventEnum.ReadNotice:
                        info = ReadNoticeHandle(ref m_flib, item.Para);
                        break;
                    case CncEventEnum.ReadProgramName:
                        info = ReadProgramNameHandle(ref m_flib, item.Para);
                        break;
                    case CncEventEnum.ReadProgramBlock:
                        info = ReadProgramBlockHandle(ref m_flib, item.Para);
                        break;
                    case CncEventEnum.ReadProgramStr:
                        info = ReadProgramStrHandle(ref m_flib, item.Para);
                        break;
                    case CncEventEnum.ReadProgramInfo:
                        info = ReadProgramInfoHandle(ref m_flib, item.Para);
                        break;
                    case CncEventEnum.ReadModalInfo:
                        info = ReadModalInfoHandle(ref m_flib, item.Para);
                        break;
                    case CncEventEnum.ReadCycleTime:
                        info = ReadCycleTimeHandle(ref m_flib, item.Para);
                        break;
                    case CncEventEnum.ReadWorkpartNum:
                        info = ReadWorkpartNumHandle(ref m_flib, item.Para);
                        break;
                    case CncEventEnum.ReadSpindleSpeed:
                        info = ReadSpindleSpeedHandle(ref m_flib, item.Para);
                        break;
                    case CncEventEnum.ReadFeedrate:
                        info = ReadFeedrateHandle(ref m_flib, item.Para);
                        break;
                    default:
                        break;
                }
                if (info?.Length >= 1)
                {
                    ShowErrorLogEvent?.Invoke(info);
                }

            });
        }

        private string ReadPmcHandle(ref ushort flib, string para)
        {
            string message = null;

            var paraModel = JsonConvert.DeserializeObject<ReadPmcModel>(para);
            var res = new List<ReadPmcResultItemModel>();

            Dictionary<short, int[]> datas = new Dictionary<short, int[]>();

            foreach (var item in paraModel.Readers)
            {
                int[] data = new int[item.DwordQuantity];
                var ret = PmcHelper.ReadPmcRange(flib, item.AdrType, item.StartNum, item.DwordQuantity, ref data);
                if (ret.Item1 == -16 || ret.Item1 == -8)
                {
                    var ret_conn = ConnectHelper.BuildConnect(ref flib, m_ip, m_port, m_timeout);

                    if (ret_conn == 0)
                    {
                        ret = PmcHelper.ReadPmcRange(flib, item.AdrType, item.StartNum, item.DwordQuantity, ref data);
                    }
                }

                if (ret.Item1 == 0)
                {
                    datas.Add(item.AdrType, data);
                }

            }

            foreach (var item in paraModel.Decompilers)
            {
                string data = "";
                var ret_dec = PmcHelper.DecompilerReadPmcInfo(datas[item.AdrType], item, ref data);
                if (ret_dec != null)
                {
                    message = ret_dec;
                }
                else
                {
                    res.Add(new ReadPmcResultItemModel()
                    {
                        Id = item.Id,
                        Value = data
                    });
                }
            }
            GetResultEvent?.Invoke( new BaseCNCResultModel<ReadPmcResultItemModel>() { Value=res,Id=""});
            return message;

        }

        private string ReadMacroHandle(ref ushort flib, string para)
        {
            string message = null;

            var paraModel = JsonConvert.DeserializeObject<ReadMacroModel>(para);
            var res = new List<ReadMacroResultItemModel>();

            var datas = new double[paraModel.Quantity];

            var ret = MacroHelper.ReadMacroRange(flib, paraModel.StartNum, paraModel.Quantity, ref datas);
            if (ret.Item1 == -16)
            {
                var ret_conn = ConnectHelper.BuildConnect(ref flib, m_ip, m_port, m_timeout);

                if (ret_conn == 0)
                {
                    ret = MacroHelper.ReadMacroRange(flib, paraModel.StartNum, paraModel.Quantity, ref datas);
                }
            }

            foreach (var item in paraModel.Decompilers)
            {
                double data = 0;
                var ret_dec = MacroHelper.DecompilerReadMacroInfo(datas, item, ref data);
                if (ret_dec != null)
                {
                    message = ret_dec;
                }
                else
                {
                    res.Add(new ReadMacroResultItemModel()
                    {
                        Id = item.Id,
                        Value = data
                    });
                }
            }
            GetResultEvent?.Invoke(new BaseCNCResultModel<ReadMacroResultItemModel>() { Value = res, Id = "" });

            return message;

        }

        private string ReadPositionHandle(ref ushort flib, string para)
        {
            string message = null;

            var paraModel = JsonConvert.DeserializeObject<ReadPositionModel>(para);
            var res = new List<ReadPositionResultItemModel>();

            Dictionary<CncPositionTypeEnum, int[]> datas = new Dictionary<CncPositionTypeEnum, int[]>();

            foreach (var item in paraModel.Readers)
            {
                int[] data = new int[Focas1.MAX_AXIS];
                var ret = PositionHelper.ReadPositionRange(flib, item.PositionType, ref data);
                if (ret.Item1 == -16)
                {
                    var ret_conn = ConnectHelper.BuildConnect(ref flib, m_ip, m_port, m_timeout);

                    if (ret_conn == 0)
                    {
                        ret = PositionHelper.ReadPositionRange(flib, item.PositionType, ref data);
                    }
                }

                if (ret.Item1 == 0)
                {
                    datas.Add(item.PositionType, data);
                }

            }

            foreach (var item in paraModel.Decompilers)
            {
                int data = 0;
                var ret_dec = PositionHelper.DecompilerReadPositionInfo(datas[item.PositionType], item, ref data);
                if (ret_dec != null)
                {
                    message = ret_dec;
                }
                else
                {
                    res.Add(new ReadPositionResultItemModel()
                    {
                        Id = item.Id,
                        Value = (double)data / m_increment
                    });
                }
            }
            GetResultEvent?.Invoke(new BaseCNCResultModel<ReadPositionResultItemModel>() { Value = res, Id = "" });

            return message;

        }

        private string ReadAlarmHandle(ref ushort flib, string para)
        {
            string message = null;

            var res = new List<ReadAlarmResultItemModel>();

            var ret = AlarmHelper.ReadAlarmRange(flib, ref res);
            if (ret.Item1 == -16)
            {
                var ret_conn = ConnectHelper.BuildConnect(ref flib, m_ip, m_port, m_timeout);

                if (ret_conn == 0)
                {
                    ret = AlarmHelper.ReadAlarmRange(flib, ref res);
                }
            }

            if (ret.Item1 != 0)
            {
                message = ret.Item2;
            }
            GetResultEvent?.Invoke(new BaseCNCResultModel<ReadAlarmResultItemModel>() { Value = res, Id = para });

            return message;
        }

        private string ReadNoticeHandle(ref ushort flib, string para)
        {
            string message = null;

            var res = new List<ReadNoticeResultItemModel>();

            var ret = NoticeHelper.ReadNoticeRange(flib, ref res);
            if (ret.Item1 == -16)
            {
                var ret_conn = ConnectHelper.BuildConnect(ref flib, m_ip, m_port, m_timeout);

                if (ret_conn == 0)
                {
                    ret = NoticeHelper.ReadNoticeRange(flib, ref res);
                }
            }

            if (ret.Item1 != 0)
            {
                message = ret.Item2;
            }
            GetResultEvent?.Invoke(new BaseCNCResultModel<ReadNoticeResultItemModel>() { Value = res, Id = para });

            return message;
        }

        private string ReadProgramNameHandle(ref ushort flib, string para)
        {
            string message = null;

            var res = new ReadProgramNameResultModel();

            var ret = ProgramNameHelper.ReadProgramName(flib, ref res);
            if (ret.Item1 == -16)
            {
                var ret_conn = ConnectHelper.BuildConnect(ref flib, m_ip, m_port, m_timeout);

                if (ret_conn == 0)
                {
                    ret = ProgramNameHelper.ReadProgramName(flib, ref res);
                }
            }

            if (ret.Item1 != 0)
            {
                message = ret.Item2;
            }
            GetResultEvent?.Invoke(new BaseCNCResultModel<ReadProgramNameResultModel>() { Value = new List<ReadProgramNameResultModel>() { res}, Id = para });

            return message;
        }

        private string ReadProgramBlockHandle(ref ushort flib, string para)
        {
            string message = null;

            var res = new ReadProgramBlockResultModel();

            var ret = ProgramBlockHelper.ReadProgramBlock(flib, ref res);
            if (ret.Item1 == -16)
            {
                var ret_conn = ConnectHelper.BuildConnect(ref flib, m_ip, m_port, m_timeout);

                if (ret_conn == 0)
                {
                    ret = ProgramBlockHelper.ReadProgramBlock(flib, ref res);
                }
            }

            if (ret.Item1 != 0)
            {
                message = ret.Item2;
            }
            GetResultEvent?.Invoke(new BaseCNCResultModel<ReadProgramBlockResultModel>() { Value = new List<ReadProgramBlockResultModel>() { res }, Id = para });

            return message;
        }

        private string ReadProgramStrHandle(ref ushort flib, string para)
        {
            string message = null;

            var res = new ReadProgramStrResultModel();

            var ret = ProgramStrHelper.ReadProgramStr(flib, ref res);
            if (ret.Item1 == -16)
            {
                var ret_conn = ConnectHelper.BuildConnect(ref flib, m_ip, m_port, m_timeout);

                if (ret_conn == 0)
                {
                    ret = ProgramStrHelper.ReadProgramStr(flib, ref res);
                }
            }

            if (ret.Item1 != 0)
            {
                message = ret.Item2;
            }
            GetResultEvent?.Invoke(new BaseCNCResultModel<ReadProgramStrResultModel>() { Value = new List<ReadProgramStrResultModel>() { res }, Id = para });

            return message;
        }

        private string ReadProgramInfoHandle(ref ushort flib, string para)
        {
            string message = null;

            var res = new ReadProgramInfoResultModel();

            var ret = ProgramInfoHelper.ReadProgramInfo(flib, ref res);
            if (ret.Item1 == -16)
            {
                var ret_conn = ConnectHelper.BuildConnect(ref flib, m_ip, m_port, m_timeout);

                if (ret_conn == 0)
                {
                    ret = ProgramInfoHelper.ReadProgramInfo(flib, ref res);
                }
            }

            if (ret.Item1 != 0)
            {
                message = ret.Item2;
            }
            GetResultEvent?.Invoke(new BaseCNCResultModel<ReadProgramInfoResultModel>() { Value = new List<ReadProgramInfoResultModel>() { res }, Id = para });

            return message;
        }

        private string ReadModalInfoHandle(ref ushort flib, string para)
        {
            string message = null;

            var res = new ReadModalResultModel();

            var ret = ModalInfoHelper.ReadModalInfo(flib, ref res);
            if (ret.Item1 == -16)
            {
                var ret_conn = ConnectHelper.BuildConnect(ref flib, m_ip, m_port, m_timeout);

                if (ret_conn == 0)
                {
                    ret = ModalInfoHelper.ReadModalInfo(flib, ref res);
                }
            }

            if (ret.Item1 != 0)
            {
                message = ret.Item2;
            }
            GetResultEvent?.Invoke(new BaseCNCResultModel<ReadModalResultModel>() { Value = new List<ReadModalResultModel>() { res }, Id = para });

            return message;
        }

        private string ReadCycleTimeHandle(ref ushort flib, string para)
        {
            string message = null;

            var res = new ReadCycleTimeResultModel();

            var ret = CycleTimeHelper.ReadCycleTime(flib, ref res);
            if (ret.Item1 == -16)
            {
                var ret_conn = ConnectHelper.BuildConnect(ref flib, m_ip, m_port, m_timeout);

                if (ret_conn == 0)
                {
                    ret = CycleTimeHelper.ReadCycleTime(flib, ref res);
                }
            }

            if (ret.Item1 != 0)
            {
                message = ret.Item2;
            }
            GetResultEvent?.Invoke(new BaseCNCResultModel<ReadCycleTimeResultModel>() { Value = new List<ReadCycleTimeResultModel>() { res }, Id = para });

            return message;
        }

        private string ReadWorkpartNumHandle(ref ushort flib, string para)
        {
            string message = null;

            var res = new ReadWorkpartNumResultModel();

            var ret = WorkpartNumHelper.ReadWorkpartNum(flib, ref res);
            if (ret.Item1 == -16)
            {
                var ret_conn = ConnectHelper.BuildConnect(ref flib, m_ip, m_port, m_timeout);

                if (ret_conn == 0)
                {
                    ret = WorkpartNumHelper.ReadWorkpartNum(flib, ref res);
                }
            }

            if (ret.Item1 != 0)
            {
                message = ret.Item2;
            }
            GetResultEvent?.Invoke(new BaseCNCResultModel<ReadWorkpartNumResultModel>() { Value = new List<ReadWorkpartNumResultModel>() { res }, Id = para });

            return message;
        }

        private string ReadSpindleSpeedHandle(ref ushort flib, string para)
        {
            string message = null;

            var res = new ReadSpindleSpeedResultModel();

            var ret = SpindleSpeedHelper.ReadSpindleSpeed(flib, ref res);
            if (ret.Item1 == -16)
            {
                var ret_conn = ConnectHelper.BuildConnect(ref flib, m_ip, m_port, m_timeout);

                if (ret_conn == 0)
                {
                    ret = SpindleSpeedHelper.ReadSpindleSpeed(flib, ref res);
                }
            }

            if (ret.Item1 != 0)
            {
                message = ret.Item2;
            }
            GetResultEvent?.Invoke(new BaseCNCResultModel<ReadSpindleSpeedResultModel>() { Value = new List<ReadSpindleSpeedResultModel>() { res }, Id = para });

            return message;
        }

        private string ReadFeedrateHandle(ref ushort flib, string para)
        {
            string message = null;

            var res = new ReadFeedrateResultModel();

            var ret = FeedrateHelper.ReadFeedrate(flib, ref res);
            if (ret.Item1 == -16)
            {
                var ret_conn = ConnectHelper.BuildConnect(ref flib, m_ip, m_port, m_timeout);

                if (ret_conn == 0)
                {
                    ret = FeedrateHelper.ReadFeedrate(flib, ref res);
                }
            }

            if (ret.Item1 != 0)
            {
                message = ret.Item2;
            }
            GetResultEvent?.Invoke(new BaseCNCResultModel<ReadFeedrateResultModel>() { Value = new List<ReadFeedrateResultModel>() { res }, Id = para });

            return message;
        }
    }
}
