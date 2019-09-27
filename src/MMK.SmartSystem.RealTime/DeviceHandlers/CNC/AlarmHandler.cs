using MMK.SmartSystem.WebCommon.DeviceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.RealTime.DeviceHandlers.CNC
{
    public class AlarmHandler : BasePollCNCHandler<ReadAlarmModel, ReadAlarmResultModel, string, string>
    {
        List<ReadAlarmResultItemModel> temp;
        private string message;

        public AlarmHandler(ushort flib) : base(flib)
        {
            temp = new List<ReadAlarmResultItemModel>();
        }

        protected override object PollDecompiler(List<ReadAlarmResultModel> res, string item)
        {
            res.Add(new ReadAlarmResultModel()
            {
                Id = item,
                Value = temp
            });
            return message;
        }

        protected override Tuple<short, string> PollRead(string item)
        {
            var ret = AlarmHelper.ReadAlarmRange(flib, ref temp);

            if (ret.Item1 != 0)
            {
                message = ret.Item2;
            }
            return ret;
        }
    }
}
