using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.WebCommon.DeviceModel
{
    public class CncPollEventData
    {
        public string Id { get; set; }

        public string Group { get; set; }

        public CncPollEventKindEnum Kind { get; set; }

        public List<CncEventData> EventDatas { get; set; } = new List<CncEventData>();
    }
}
