using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.WebCommon.DeviceModel
{
    public class ReadAlarmResultModel
    {
        public string Id { get; set; }

        public List<ReadAlarmResultItemModel> Value { get; set; } = new List<ReadAlarmResultItemModel>();
    }
}
