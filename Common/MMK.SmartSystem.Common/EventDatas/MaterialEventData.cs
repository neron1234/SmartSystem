using Abp.Events.Bus;
using MMK.SmartSystem.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Common.EventDatas
{
    public class MaterialInfoEventData: BaseErrorEventData
    {
        public string code { get; set; }
        public string name_EN { get; set; }
        public string name_CN { get; set; }
        public string description { get; set; }
        public string creationTime { get; set; }
        public int id { get; set; }
    }

    public class MachiningGroupInfoEventData: BaseErrorEventData
    {
        public string code { get; set; }
        public string materialId { get; set; }
        public string materialThickness { get; set; }
        public string description { get; set; }
        public string creationTime { get; set; }
        public string id { get; set; }
    }

}
