using Abp.Dependency;
using MMK.SmartSystem.CNC.Core.DeviceHelpers;
using MMK.SmartSystem.WebCommon;
using MMK.SmartSystem.WebCommon.DeviceModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.CNC.Core.Workers
{
    class DyHandlerProxy
    {
        public MethodInfo Method { get; set; }

        public object Handler { get; set; }

        public object Parms { get; set; }
    }
    public class CncCoreWorker
    {
        string m_ip = "192.168.21.97";
        ushort m_port = 8193;
        int m_timeout = 10;
        double m_increment = 1000;
        ushort m_flib = 0;

        public event Action<string> ShowErrorLogEvent;
        public event Action<object> GetResultEvent;
        IIocManager iocManager;

        static BlockingCollection<CncEventData> m_EventDatas = new BlockingCollection<CncEventData>();

        public CncCoreWorker()
        {
            m_ip = SmartSystemCNCCoreConsts.CncIP;
            m_port = SmartSystemCNCCoreConsts.CncPort;
            m_timeout = SmartSystemCNCCoreConsts.CncTimeout;
            m_increment = SmartSystemCNCCoreConsts.CncIncrement;
        }
        public void CncPollEventHandle(List<CncPollEventData> pollDatas)
        {
            var pmc = new ReadPmcModel();
            var macro = new ReadMacroModel().Readers[0];

            foreach (var poll in pollDatas)
            {
                foreach (var eve in poll.EventDatas)
                {
                    if (eve.Kind == CncEventEnum.ReadPmc)
                    {
                        var paraModel = JsonConvert.DeserializeObject<ReadPmcModel>(eve.Para);

                        foreach (var read in paraModel.Readers)
                        {
                            var temp_read = pmc.Readers.Where(x => x.AdrType == read.AdrType).FirstOrDefault();
                            if (temp_read != null)
                            {
                                var start = temp_read.StartNum < read.StartNum ? temp_read.StartNum : read.StartNum;
                                var end = (temp_read.StartNum + temp_read.DwordQuantity) > (read.StartNum + read.DwordQuantity) ? (temp_read.StartNum + temp_read.DwordQuantity) : (read.StartNum + read.DwordQuantity);

                                temp_read.StartNum = start;
                                temp_read.DwordQuantity = (ushort)(end - start);
                            }
                            else
                            {
                                pmc.Readers.Add(read);
                            }
                        }

                        pmc.Decompilers.AddRange(paraModel.Decompilers);
                    }
                    else if (eve.Kind == CncEventEnum.ReadMacro)
                    {
                        var paraModel = JsonConvert.DeserializeObject<ReadMacroModel>(eve.Para).Readers[0];

                        var start = paraModel.StartNum > macro.StartNum ? paraModel.StartNum : macro.StartNum;
                        var end = (paraModel.StartNum + paraModel.Quantity) > (macro.StartNum + macro.Quantity) ? (paraModel.StartNum + paraModel.Quantity) : (macro.StartNum + macro.Quantity);

                        macro.StartNum = start;
                        macro.Quantity = end - start;

                        //   macro.Decompilers.AddRange(paraModel.Decompilers);
                    }
                }
            }


        }

        public BaseCNCResultModel<ReadProgramListItemResultModel> ReadProgramList(string folder)
        {

            ushort flib = 0;
            var ret = ConnectHelper.BuildConnect(ref flib, m_ip, m_port, m_timeout);

            if (ret == 0)
            {
                var res = new List<ReadProgramListItemResultModel>();

                var ret_1 = ProgramListHelper.ReadProgramList(flib, folder, ref res);
                ConnectHelper.FreeConnect(flib);

                if (ret_1.Item1 == 0)
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

        public CncCoreWorker(IIocManager _iocManager)
        {
            iocManager = _iocManager;
        }

        private void HandlerExecute()
        {
            List<CncEventData> tempEventDatas = new List<CncEventData>();
            foreach (var item in MMKSmartSystemWebCommonConsts.PageCncEventDict)
            {
                tempEventDatas.AddRange(item.Value);
            }
            List<DyHandlerProxy> dyHandlers = new List<DyHandlerProxy>();
            var listGroup = tempEventDatas.GroupBy(d => d.Kind);
            foreach (var item in listGroup)
            {
                Type kindtype = item.Key.GetType();
                FieldInfo fd = kindtype.GetField(item.Key.ToString());
                var cncCustom = fd.GetCustomAttribute<CncCustomEventAttribute>();
                if (cncCustom == null)
                {
                    ShowErrorLogEvent?.Invoke($"未定义与前端定义的映射模型{item.Key.ToString()} CncCustomEvent");

                    continue;
                }
                var handlerType = Type.GetType(cncCustom.HandlerName);
                if (handlerType == null)
                {
                    ShowErrorLogEvent?.Invoke($"未定义{cncCustom.HandlerName} Handler");
                    continue;
                }
                try
                {
                    var handler = iocManager.Resolve(handlerType);
                    var methodInfo = handlerType.GetMethod("MargePollRequest");
                    var methodType = methodInfo.GetParameters()[0].ParameterType;

                    var firstParm = typeof(CncEventData).Assembly.CreateInstance(methodType.FullName);

                    foreach (var data in item)
                    {
                        var jsonData = JsonConvert.DeserializeObject(data.Para, methodType);

                        firstParm = methodInfo.Invoke(handler, new object[] { firstParm, jsonData });

                    }
                    dyHandlers.Add(new DyHandlerProxy()
                    {
                        Handler = handler,
                        Method = handlerType.GetMethod("PollHandle"),
                        Parms = firstParm

                    });

                }
                catch (Exception ex)
                {

                    ShowErrorLogEvent?.Invoke($"CncCustomEnum:【{item.Key}】{ex.Message}");
                }
            }


            foreach (var item in dyHandlers)
            {
                try
                {
                    var info = item.Method.Invoke(item.Handler, new object[] { item.Parms });
                    JObject jObject = JObject.FromObject(info);

                    if (!string.IsNullOrEmpty(jObject["ErrorMessage"]?.ToString()))
                    {
                        string message = jObject["ErrorMessage"]?.ToString();
                        string error = "\r\n==============Begin【Hander Exception】==============\n";
                        error += $"Hander:【{item.Handler.ToString()}】 Method:【{item.Method.Name}】\n";
                        error += $"【HanderError】:{message}\n";
                        error += "==============End【Hander Exception】==============\r\n";
                        ShowErrorLogEvent?.Invoke(error);
                        continue;
                    }
                    GetResultEvent(info);
                }
                catch (Exception ex)
                {
                    string error = "\r\n==============Begin【Hander Exception】==============\n";
                    error += $"Hander:【{item.Handler.ToString()}】 Method:【{item.Method.Name}】\n";
                    error += $"【Exception】:{ex.Message}\n";
                    error += $"【InnerException】:{ex.InnerException?.Message}\n";
                    error += "==============End【Hander Exception】==============\r\n";
                    ShowErrorLogEvent?.Invoke(error);
                    
                }

            }
        }

        public void Execute()
        {

            HandlerExecute();
        }



        #region OLD 取消的
        private void HandlerSwitchExecute()
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
                var ret = new PmcHelper().ReadPmcRange(flib, item.AdrType, item.StartNum, item.DwordQuantity, ref data);
                if (ret.Item1 == -16 || ret.Item1 == -8)
                {
                    var ret_conn = ConnectHelper.BuildConnect(ref flib, m_ip, m_port, m_timeout);

                    if (ret_conn == 0)
                    {
                        ret = new PmcHelper().ReadPmcRange(flib, item.AdrType, item.StartNum, item.DwordQuantity, ref data);
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
                string ret_dec = "";
                if (datas.ContainsKey(item.AdrType))
                {
                    ret_dec = new PmcHelper().DecompilerReadPmcInfo(datas[item.AdrType], item, ref data);

                }
                // var ret_dec = PmcHelper.DecompilerReadPmcInfo(datas[item.AdrType], item, ref data);
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
            GetResultEvent?.Invoke(new BaseCNCResultModel<ReadPmcResultItemModel>() { Value = res, Id = "" });
            return message;

        }

        private string ReadMacroHandle(ref ushort flib, string para)
        {
            string message = null;

            var paraModel = JsonConvert.DeserializeObject<ReadMacroModel>(para);
            var res = new List<ReadMacroResultItemModel>();
            foreach (var item in paraModel.Readers)
            {
                var datas = new double[item.Quantity];

                var ret = new MacroHelper().ReadMacroRange(flib, item.StartNum, item.Quantity, ref datas);
                if (ret.Item1 == -16)
                {
                    var ret_conn = ConnectHelper.BuildConnect(ref flib, m_ip, m_port, m_timeout);

                    if (ret_conn == 0)
                    {
                        ret = new MacroHelper().ReadMacroRange(flib, item.StartNum, item.Quantity, ref datas);
                    }
                }

            }

            //foreach (var item in paraModel.Decompilers)
            //{
            //    double[] data = ;
            //    var ret_dec = MacroHelper.DecompilerReadMacroInfo(datas, item, ref data);
            //    if (ret_dec != null)
            //    {
            //        message = ret_dec;
            //    }
            //    else
            //    {
            //        res.Add(new ReadMacroResultItemModel()
            //        {
            //            Id = item.Id,
            //            Value = data
            //        });
            //    }
            //}
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
                if (ret.Item1 == -16 || ret.Item1 == -8)
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
                string ret_dec = "";
                if (datas.ContainsKey(item.PositionType))
                {
                    ret_dec = PositionHelper.DecompilerReadPositionInfo(datas[item.PositionType], item, ref data);
                }

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

            var res = new ReadProgramNameResultItemModel();

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
            //GetResultEvent?.Invoke(new BaseCNCResultModel<ReadProgramNameResultModel>() { Value = new List<ReadProgramNameResultModel>() {  Id = para });

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

        #endregion

    }
}
