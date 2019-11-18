using MMK.SmartSystem.CNC.Core.DeviceHelpers;
using MMK.SmartSystem.WebCommon.DeviceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.CNC.Core.DeviceHandlers
{
    public class NoticeHandler : BasePollCNCHandler<ReadNoticeModel, ReadNoticeResultModel, string, string>
    {
        List<ReadNoticeResultItemModel> temp;
        private string message { get; set; }
        public NoticeHandler()
        {
            temp = new List<ReadNoticeResultItemModel>();
        }

        protected override object PollDecompiler(List<ReadNoticeResultModel> res, string item)
        {
            res.Add(new ReadNoticeResultModel()
            {
                Id = item,
                Value = temp
            });
            return message;
        }

        protected override Tuple<short, string> PollRead(string item)
        {
            var ret =new NoticeHelper().ReadNoticeRange(flib, ref temp);
            if (ret.Item1 != 0)
            {
                message = ret.Item2;
            }
            return ret;
        }
    }
}
