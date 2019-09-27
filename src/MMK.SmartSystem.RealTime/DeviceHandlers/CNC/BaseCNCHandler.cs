using MMK.SmartSystem.WebCommon.DeviceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.RealTime.DeviceHandlers.CNC
{
    public abstract class BasePollCNCHandler<I, U, R, D> where I : CncReadDecoplilersModel<R, D> where U : BaseCncResultModel
    {
        protected ushort flib = 0;
        public BasePollCNCHandler(ushort f)
        {
            flib = f;
        }
        protected virtual short Connect()
        {
            return ConnectHelper.BuildConnect(ref flib, SmartSystemRealTimeConsts.CncIP,
                SmartSystemRealTimeConsts.CncPort, SmartSystemRealTimeConsts.CncTimeout);


        }
        public event Action<string> ShowErrorLogEvent;
        public event Action<object> GetResultEvent;
        protected abstract Tuple<short, string> PollRead(R item);
        protected abstract object PollDecompiler(List<U> res, D item);
        public string PollHandle(I paraModel)
        {
            string message = null;
            var res = new List<U>();
            foreach (var item in paraModel.Readers)
            {
                var ret = PollRead(item);
                if (ret.Item1 == -16 || ret.Item1 == -8)
                {
                    var ret_conn = Connect();

                    if (ret_conn == 0)
                    {
                        ret = PollRead(item);
                    }
                }
            }

            foreach (var item in paraModel.Decompilers)
            {
                string ret_dec = PollDecompiler(res, item).ToString();
                if (ret_dec != null)
                {
                    message = ret_dec;
                }
               
            }
            GetResultEvent?.Invoke(new BaseCNCResultModel<U>() { Value = res, Id = "" });
            return message;

        }


    }
}
