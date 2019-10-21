using MMK.SmartSystem.CNC.Core.DeviceHelpers;
using MMK.SmartSystem.WebCommon.DeviceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.CNC.Core.DeviceHandlers
{
    public class CycleTimeHandler : BasePollCNCHandler<ReadCycleTimeModel, ReadCycleTimeResultModel, string, string>
    {
        ReadCycleTimeResultModel temp;
        private string message;
        public CycleTimeHandler()
        {
            temp = new ReadCycleTimeResultModel();
        }

        protected override object PollDecompiler(List<ReadCycleTimeResultModel> res, string item)
        {
            res.Add(new ReadCycleTimeResultModel()
            {
                Id = item,
                Value = temp.Value
            });
            return message;
        }

        protected override Tuple<short, string> PollRead(string item)
        {
            var ret = CycleTimeHelper.ReadCycleTime(flib, ref temp);
            if (ret.Item1 != 0)
            {
                message = ret.Item2;
            }
            return ret;
        }
    }
}
