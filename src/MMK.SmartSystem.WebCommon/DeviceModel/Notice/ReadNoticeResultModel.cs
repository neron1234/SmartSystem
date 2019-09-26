using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.WebCommon.DeviceModel
{
    public class ReadNoticeResultModel
    {
        public string Id { get; set; }

        public List<ReadNoticeResultItemModel> Value { get; set; } = new List<ReadNoticeResultItemModel>();
    }
}
