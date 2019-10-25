using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Common.EventDatas
{
    public class EdgeCuttingByGroupIdEventData : BaseApiEventData<List<EdgeCuttingDataDto>>
    {
        public int machiningDataGroupId { get; set; }
    }
    public class UpdateEdgeCuttingEventData : BaseApiEventData<EdgeCuttingDataDto>
    {
        public UpdateEdgeCuttingDataDto UpdateEdgeCuttingData { get; set; }
    }
}
