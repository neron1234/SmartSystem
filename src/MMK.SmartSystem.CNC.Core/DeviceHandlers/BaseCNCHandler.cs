using Abp.Dependency;
using MMK.SmartSystem.CNC.Core.DeviceHelpers;
using MMK.SmartSystem.WebCommon.DeviceModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.CNC.Core.DeviceHandlers
{
    public abstract class BasePollCNCHandler<I, U, R, D> : ITransientDependency where I : CncReadDecoplilersModel<R, D> where U : BaseCncResultModel
    {
        protected static ushort flib = 0;

        protected virtual short Connect()
        {
            return ConnectHelper.BuildConnect(ref flib, SmartSystemCNCCoreConsts.CncIP,
                SmartSystemCNCCoreConsts.CncPort, SmartSystemCNCCoreConsts.CncTimeout);


        }

        protected abstract Tuple<short, string> PollRead(R item);
        protected abstract object PollDecompiler(List<U> res, D item);
        public BaseCNCResultModel<U> PollHandle(I paraModel)
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
                    else
                    {
                        return new BaseCNCResultModel<U>() { Value = res, Id = "", ErrorMessage = "连接CNC通信失败" };

                    }
                }
            }

            foreach (var item in paraModel.Decompilers)
            {
                string ret_dec = PollDecompiler(res, item)?.ToString();
                if (ret_dec != null)
                {
                    message = ret_dec;
                }

            }

            return new BaseCNCResultModel<U>() { Value = res, Id = "", ErrorMessage = message };

        }

        protected virtual Tuple<short,T> MargePollRequest<T>(T current, CncEventData data) where T : I
        {
            var paraModel = JsonConvert.DeserializeObject<T>(data.Para);

            current.Decompilers.AddRange(paraModel.Decompilers);

            return new Tuple<short, T>(0, current);
        }
    }
}
