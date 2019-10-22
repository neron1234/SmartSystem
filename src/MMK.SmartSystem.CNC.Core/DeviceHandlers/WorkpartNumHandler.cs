using MMK.SmartSystem.CNC.Core.DeviceHelpers;
using MMK.SmartSystem.WebCommon.DeviceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.CNC.Core.DeviceHandlers
{
    public class WorkpartNumHandler : BasePollCNCHandler<ReadWorkpartNumModel, ReadWorkpartNumResultModel, string, string>
    {
        ReadWorkpartNumResultModel res;
        private string message { get; set; }
        public WorkpartNumHandler()
        {
            res = new ReadWorkpartNumResultModel();
        }

        protected override object PollDecompiler(List<ReadWorkpartNumResultModel> res, string item)
        {
            return message;
        }

        protected override Tuple<short, string> PollRead(string item)
        {
            var ret = WorkpartNumHelper.ReadWorkpartNum(flib, ref res);
            if (ret.Item1 != 0)
            {
                message = ret.Item2;
            }
            return ret;
        }
    }
}
