using MMK.SmartSystem.CNC.Core.DeviceHelpers;
using MMK.SmartSystem.WebCommon.DeviceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.CNC.Core.DeviceHandlers
{
    public class FeedrateHandler : BasePollCNCHandler<ReadFeedrateModel, ReadFeedrateResultModel, string, string>
    {
        ReadFeedrateResultModel temp;
        private string message;
        public FeedrateHandler()
        {
            temp = new ReadFeedrateResultModel();
        }

        protected override object PollDecompiler(List<ReadFeedrateResultModel> res, string item)
        {
            res.Add(new ReadFeedrateResultModel()
            {
                Id = item,
                Value = temp.Value
            });
            return message;
        }

        protected override Tuple<short, string> PollRead(string item)
        {
            var ret =new FeedrateHelper().ReadFeedrate(flib, ref temp);
            if (ret.Item1 != 0)
            {
                message = ret.Item2;
            }
            return ret;
        }
    }
}
