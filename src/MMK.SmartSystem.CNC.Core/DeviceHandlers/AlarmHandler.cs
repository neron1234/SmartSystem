using MMK.SmartSystem.CNC.Core.DeviceHelpers;
using MMK.SmartSystem.WebCommon.DeviceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.CNC.Core.DeviceHandlers
{
    public class AlarmHandler : BasePollCNCHandler<ReadAlarmModel, ReadAlarmResultModel, string, string>
    {
        List<ReadAlarmResultItemModel> temp;
        private string message;

        public AlarmHandler() 
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
            var ret = new AlarmHelper().ReadAlarmRange(flib, ref temp);

            if (ret.Item1 != 0)
            {
                message = ret.Item2;
            }
            return ret;
        }
    }
}
