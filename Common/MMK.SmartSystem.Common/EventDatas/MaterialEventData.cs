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

    public class AddMachiningInfoEventData : BaseErrorEventData
    {
        public CreateMaterialDto CreateMaterial { get; set; }
    }

    public class AddMachiningGroupInfoEventData : BaseErrorEventData
    {
        public CreateMachiningGroupDto CreateMachiningGroup { get; set; }
    }

    public class DeleteMachiningGroupInfoEventData : BaseErrorEventData
    {
        public int MachiningGroupId { get; set; }
    }

    public class DeleteMachiningInfoEventData : BaseErrorEventData
    {
        public int MaterialId { get; set; }
    }
}
