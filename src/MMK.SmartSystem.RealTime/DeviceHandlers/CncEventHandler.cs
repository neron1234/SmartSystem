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
using MMK.SmartSystem.RealTime.DeviceModel;

namespace MMK.SmartSystem.RealTime.DeviceHandlers
{

    public class CncEventHandler : BackgroundJob<CncEventData>, ITransientDependency
    {
        string m_ip = "192.168.1.1";
        ushort m_port = 8193;
        int m_timeout = 10;
        ushort m_flib = 0;
        int m_maxConn = 1;

        BlockingCollection<CncEventData> m_EventDatas = new BlockingCollection<CncEventData>();

        static CncEventHandler() {

        }
        
        public void HandleEvent(CncEventData eventData)
        {

        }

        public override void Execute(CncEventData args)
        {
            var ret = ConnectHelper.BuildConnect(ref m_flib, m_ip, m_port, m_timeout);

            while (true)
            {
                List<CncEventData> tempEventDatas = new List<CncEventData>(m_EventDatas.ToArray());

                tempEventDatas.ForEach(item =>
                {
                    switch (item.Kind)
                    {
                        case CncEventEnum.ReadPmc:
                            ReadPmcHandle(ref m_flib, item.Para);
                            break;
                        default:
                            break;
                    }
                });

                System.Threading.Thread.Sleep(100);
            }
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
                        
            foreach(var item in paraModel.Decompilers)
            {
                string data = "";
                var ret_dec = PmcHelper.DecompilerReadPmcInfo(datas[item.AdrType], item, data);
                if(ret_dec!=null)
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

            return message;

        }


    }
}
