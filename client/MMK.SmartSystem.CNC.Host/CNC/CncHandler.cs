using MMK.SmartSystem.CNC.Host.DeviceHandlers;
using MMK.SmartSystem.CNC.Host.DeviceModel;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.CNC.Host.CNC
{
    public class CncEventData 
    {
        public CncEventEnum Kind { get; set; }

        public string Para { get; set; }
    }

    public enum CncEventEnum
    {
        ReadMacro,
        ReadPmc,
        Position,
        LampStatus,
        AlarmMessage,
        NoticeMessage,
        ProgramInfo,
        ModelInfo,
        CycleTime,
        WorkpartNum,
    }
    public class CncHandler
    {
        string m_ip = "192.168.21.177";
        ushort m_port = 8193;
        int m_timeout = 10;
        double m_increment = 1000;
        ushort m_flib = 0;
        int m_maxConn = 1;

        public event Action<string> ShowErrorLogEvent;
        public event Action<object> GetResultEvent;
        public static BlockingCollection<CncEventData> m_EventDatas = new BlockingCollection<CncEventData>();



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
                    case CncEventEnum.Position:
                        info = ReadPositionHandle(ref m_flib, item.Para);
                        break;
                    case CncEventEnum.AlarmMessage:
                        info = ReadAlarmHandle(ref m_flib, item.Para);
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
                if (ret.Item1 == -16)
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
            GetResultEvent?.Invoke(res);
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
            GetResultEvent?.Invoke(res);

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
            GetResultEvent?.Invoke(res);

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
            GetResultEvent?.Invoke(res);

            return message;
        }
    }
}
