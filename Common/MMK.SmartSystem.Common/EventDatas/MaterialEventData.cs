using Abp.Events.Bus;
using MMK.SmartSystem.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Common.EventDatas
{
    public class MaterialInfoEventData : BaseApiEventData<List<MeterialGroupThicknessDto>>
    {
        public bool IsCheckSon { get; set; }
    }

    public class MachiningGroupInfoEventData : BaseApiEventData<List<MachiningGroupDto>>
    {
        public int MaterialId { get; set; }
    }

    public class AddMachiningInfoEventData : BaseApiEventData<MaterialDto>
    {
        public CreateMaterialDto CreateMaterial { get; set; }
    }

    public class AddMachiningGroupInfoEventData : BaseApiEventData<MachiningGroupDto>
    {
        public CreateMachiningGroupDto CreateMachiningGroup { get; set; }
    }

    public class DeleteMachiningGroupInfoEventData : BaseApiEventData<object>
    {
        public int MachiningGroupId { get; set; }
    }

    public class DeleteMachiningInfoEventData : BaseApiEventData<object>
    {
        public int MaterialId { get; set; }
    }
}
