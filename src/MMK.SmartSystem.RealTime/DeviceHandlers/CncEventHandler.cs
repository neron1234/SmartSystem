using Abp.Dependency;
using Abp.Events.Bus.Handlers;
using Microsoft.AspNetCore.SignalR;
using MMK.SmartSystem.RealTime.Hubs;
using MMK.SmartSystem.WebCommon.DeviceModel;
using MMK.SmartSystem.WebCommon.EventModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMK.SmartSystem.RealTime.DeviceHandlers
{

    public class CncEventHandler : IEventHandler<CncEventData>, ITransientDependency
    {
        string m_ip = "192.168.1.1";
        ushort m_flib = 0;
        int m_maxConn = 1;
        static CncEventHandler() {

        }
        
        public void HandleEvent(CncEventData eventData)
        {
            switch(eventData.Kind)
            {
                case CncEventEnum.IP:
                    m_ip = eventData.Para;
                    FreeConnect();
                    break;
                case CncEventEnum.READPMC:
                    short type;
                    ushort adr;
                    ushort num;
                    var ret = DecompileReadPmcPara(eventData.Para, out type, out adr, out num);
                    int[] data = new int[num];
                    ret = ReadPmc(type, adr, num, data);
                    break;
                default:
                    break;
            }
        }

        #region 连接
        private short BuildConnect()
        {
            Focas1.cnc_freelibhndl(m_flib);

            var ret = Focas1.cnc_allclibhndl3("192.168.1.1", 8193, 10, out m_flib);
            return ret;
        }

        private void FreeConnect()
        {
            Focas1.cnc_freelibhndl(m_flib);
            m_flib = 0;
        }
        #endregion

        #region PMC

        private string DecompileReadPmcPara(string para,out short type, out ushort adr, out ushort num)
        {
            type = 0;
            adr = 0;
            num = 0;

            //PMC;type;adr;num
            var temps = para.Split(";");

            if (temps.Length < 4) return "PMC通讯参数错误";

            var ret_T = short.TryParse(temps[1], out type);
            var ret_A = ushort.TryParse(temps[2], out adr);
            var ret_N = ushort.TryParse(temps[3], out num);

            if(ret_T==false || ret_A==false || ret_N==false)
            {
                return "PMC通讯参数错误";
            }

            return null;

        }

        private string ReadPmc(short type, ushort adr, ushort num, int[] data)
        {
            if (num > 10) return "读取PMC信号错误,读取数量超限";
            if (data.Length < num) return "读取PMC信号错误,数据存储区域过小";

            short ret = -16;
            int connTime = 0;


            Focas1.IODBPMC2 buf = new Focas1.IODBPMC2();
            buf.ldata = new int[num];
            ushort adr_end = (ushort)(adr + num * 4 - 1);
            ret = Focas1.pmc_rdpmcrng(m_flib, type, 2, adr, adr_end, 48, buf);

            while (ret==-16 && connTime< m_maxConn)
            {
                var ret_conn = BuildConnect();

                if (ret_conn == 0)
                {
                    ret = Focas1.pmc_rdpmcrng(m_flib, type, 2, adr, adr_end, 48, buf);
                }
                connTime++;
            }

            if(ret==0)
            {
                buf.ldata.CopyTo(data, 0);
                return null;
            }

            return $"读取PMC信号错误,返回:{ret}";
        }

        #endregion
    }
}
