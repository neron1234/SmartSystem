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
        public bool IsAll { get; set; }
    }

    public class MachiningGroupInfoEventData: BaseErrorEventData
    {
        public int MaterialId { get; set; }
    }

    public class AddMaterialEventData : BaseErrorEventData
    {
        public int MaterialId { get; set; }
        public double MaterialThickness { get; set; }
    }

}
